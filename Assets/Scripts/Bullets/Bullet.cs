using Mirror;
using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{
    private SpriteRenderer renderer;
    private Rigidbody2D rb;
    private Spellcasting spellcasting;
    private TrailRenderer trailRenderer;
    private GameObject currentTrail;
    Vector2 direction;

    [Header("Bullet Stats")]
    [SerializeField]
    private LayerMask whatDestroysBullet;
    [SerializeField]
    private LayerMask bulletPlayerCollision;
    [SerializeField]
    private LayerMask otherBulletsCollision;
    [SerializeField]
    public GameObject trailPrefab;


    //these stats are in BulletStats
    private List<BulletType> bulletTypes = new List<BulletType>();
    private float bulletDamage;
    private float bulletHealth;
    private float bulletSize;
    private float bulletSpeed;
    private float timeToLive;
    private float timeToEscape;
    private int bounces;
    private List<BulletStats> splitBullets;
    private float explosionRadius;
    private float explosionDamage;
    private float explosionDamageMultMaxRange;
    private float trailLength;
    private GameObject _owner;

    [Header("Normal Bullets")]
    [SerializeField]
    private float normalBulletSpeed;

    [Header("Physics Bullets")]
    [SerializeField]
    private float physicsBulletSpeed;
    [SerializeField]
    private float physicsBulletGravity;

    private float _disableTime;
    private float _escapeTime;


    public enum BulletType
    {
        Normal, //straight velocity, no gravity
        Physics, //affected by gravity, rotates in direction of velocity
        Trail, //leaves a trail behind it
        Bounce, //bounces off surfaces a certain amount of times before being destroyed
        Split, //on collision, spawns multiple smaller bullets that fly in different directions
        Explosion //on codestruction, spawns an explosion that damages everything in a radius
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        currentTrail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        currentTrail.transform.SetParent(transform);
        trailRenderer = currentTrail.GetComponent<TrailRenderer>();
    }

    void OnEnable()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    private void Update()
    {
        if (!isServer) return;

        if (Time.time > _disableTime)
            DestroyBullet();

        this.transform.localScale = Vector3.one * bulletSize;
    }

    private void FixedUpdate()
    {
        if (bulletTypes.Contains(BulletType.Physics))
        {
            //rotate bullet in direction of velocity;
            transform.right = rb.linearVelocity;
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        }

        
        //Collision
        Vector2 velocity = rb.linearVelocity;
        float travelDistance = velocity.magnitude * Time.fixedDeltaTime;
        Vector2 direction = velocity.normalized;

        if (travelDistance <= 0) return;

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, bulletSize*0.5f, direction, travelDistance, whatDestroysBullet);

        if (hit.collider != null)
        {
            if (bulletTypes.Contains(BulletType.Bounce) && bounces > 0)
            {
                bounces--;
                transform.position = hit.centroid;

                Vector2 newDirection = Vector2.Reflect(direction, hit.normal);
                transform.right = newDirection;
                float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle -90);
                rb.linearVelocity = newDirection * velocity.magnitude;
            }
            else
            {
                transform.position = hit.centroid;
                DestroyBullet();
                Debug.Log("hit Wall");
            }
        }
    }



    [Server]
    public void LaunchServer(Vector2 direction, Quaternion rotation, Vector2 position)
    {
        this.direction = direction;
        transform.SetPositionAndRotation(position, rotation * Quaternion.Euler(0, 0, -90));
        gameObject.SetActive(true);
        InitializeBulletType();
        InitializeBullet();
    }

    [ClientRpc]
    public void RpcLaunch(Vector2 direction, Quaternion rotation, Vector2 position)
    {
        // In host mode, the server already initialized the bullet.
        if (isServer) return;

        this.direction = direction;
        transform.SetPositionAndRotation(position, rotation * Quaternion.Euler(0, 0, -90));
        gameObject.SetActive(true);
        InitializeBulletType();
        InitializeBullet();
    }

    private void InitializeBullet()
    {
        _disableTime = Time.time + timeToLive;
        _escapeTime = Time.time + timeToEscape;

        spellcasting = _owner.GetComponent<Spellcasting>();
    }

    public void Cast(Vector2 direction, Quaternion rotation, Vector2 position, BulletStats stats)
    {
        ApplyStatsClient(stats);
        ApplyStatsServer(stats);

        LaunchServer(direction, rotation, position);
        RpcLaunch(direction, rotation, position);
    }

    [Server]
    private void ApplyStatsServer(BulletStats stats)
    {
        ApplyStats(stats);
    }

    [ClientRpc]
    private void ApplyStatsClient(BulletStats stats)
    {
        ApplyStats(stats);
    }

    private void ApplyStats(BulletStats stats)
    {
        this.bulletTypes = stats.bulletTypes;
        this.bulletDamage = stats.bulletDamage;
        this.bulletHealth = stats.bulletHealth;
        this.bulletSize = stats.bulletSize;
        this.bulletSpeed = stats.bulletSpeed;
        renderer.material.color = stats.bulletColor;
        this._owner = stats.owner;
        this.bounces = stats.bounces;
        this.splitBullets = stats.splitBullets;
        this.explosionRadius = stats.explosionRadius;
        this.explosionDamage = stats.explosionDamage;
        this.explosionDamageMultMaxRange = stats.explosionDamageMultMaxRange;
        this.trailLength = stats.trailLength;
        this.timeToLive = stats.timeToLive;
        this.timeToEscape = stats.timeToEscape;
    }

    private void InitializeBulletType()
    {
        if (bulletTypes.Contains(BulletType.Normal))
        {
            SetStraightVelocity();
            rb.gravityScale = 0;
        }
        if (bulletTypes.Contains(BulletType.Physics))
        {
            SetPhysicsVelocity();
            rb.gravityScale = physicsBulletGravity;
        }
        trailRenderer.enabled = false;
        if (bulletTypes.Contains(BulletType.Trail)){
            trailRenderer.Clear();
            trailRenderer.enabled = true;
            trailRenderer.widthMultiplier = bulletSize;
            trailRenderer.time = trailLength;
            trailRenderer.material.color = renderer.material.color;
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * normalBulletSpeed * bulletSpeed;
    }

    private void SetPhysicsVelocity()
    {
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * physicsBulletSpeed * bulletSpeed;
    }


    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;

        if ((otherBulletsCollision.value & (1 << collision.gameObject.layer)) > 0)
        {

            //spawn Paritcles
            //Soundeffect

            Bullet bullet = collision.GetComponent<Bullet>();

            if (!(_owner = bullet._owner)) {
                if (bullet != null)
                {
                    bullet.bulletHealth -= bulletDamage;
                    this.bulletHealth -= bullet.bulletDamage;
                    if (bullet.bulletHealth <= 0)
                    {
                        bullet.DestroyBullet();
                    }
                    if (this.bulletHealth <= 0)
                    {
                        DestroyBullet();
                    }
                }
            }
        }

        if ((bulletPlayerCollision.value & (1 << collision.gameObject.layer)) > 0)
        {
            //spawn Paritcles
            //Soundeffect

            if (collision.gameObject == _owner && _escapeTime > Time.time) { return; } //attempt to fix problem where the bullet hits the player immediately whem shooting

            Health health = collision.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage((int)bulletDamage);
            }
            //Screen shake
            DestroyBullet();
            Debug.Log("hit Player");
        }
    }

    private void DestroyBullet()
    {
        if (bulletTypes.Contains(BulletType.Split)){
            foreach(BulletStats bullet in splitBullets)
            {
                Vector2 castDirection = Quaternion.Euler(0, 0, bullet.splitAngleOffset) * this.transform.right;

                Quaternion castAngle = Quaternion.Euler(0, 0, Mathf.Atan2(castDirection.y, castDirection.x) * Mathf.Rad2Deg);

                spellcasting.CastBullet(castDirection,this.transform.position,castAngle,bullet);
            }
        }
        if (bulletTypes.Contains(BulletType.Explosion))
        {
            spellcasting.Explosion(this.transform.position, this.explosionRadius ,this.explosionDamage, this.explosionDamageMultMaxRange);
        }
        if (bulletTypes.Contains(BulletType.Trail))
        {
            DetachAndSwapTrail();
        }
        ReturnToPoolServer();
    }

    [Server]
    private void ReturnToPoolServer()
    {
        // prevent double-return (TTL + collision in same frame, etc.)
        if (!isServer || !gameObject.activeSelf) return;

        // UnSpawn tells clients to remove it. With a registered UnspawnHandler,
        // clients will return it to their local pool instead of Destroy().
        NetworkServer.UnSpawn(gameObject);

        // In dedicated-server mode, also keep a server-side reference.
        // In host mode, Mirror already invoked the client-side unspawn handler.
        if (!isClient && ObjectPool.instance != null)
            ObjectPool.instance.ReturnServerObject(gameObject);
    }

    private void DetachAndSwapTrail()
    {
        trailRenderer.transform.SetParent(null);
        trailRenderer.emitting = false;
        Destroy(trailRenderer.gameObject, trailRenderer.time);

        currentTrail = Instantiate(trailPrefab, transform.position, Quaternion.identity);
        currentTrail.transform.SetParent(transform);
        trailRenderer = currentTrail.GetComponent<TrailRenderer>();
    }
}
