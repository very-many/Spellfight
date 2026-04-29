using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{

    private Vector2 mousePos;
    private Rigidbody2D rb;
    Vector3 direction;
    private Shooting shooting;

    [Header("Bullet Stats")]
    [SerializeField]
    private LayerMask whatDestroysBullet;
    [SerializeField]
    private int timeToLive = 3;
    public BulletType bulletType;


    [Header("Normal Bullets")]
    [SerializeField]
    private float normalBulletSpeed;

    [Header("Physics Bullets")]
    [SerializeField]
    private float physicsBulletSpeed;
    [SerializeField]
    private float physicsBulletGravity;

    private Shooting _gun;
    private float _disableTime;

    public enum BulletType
    {
        Normal,
        Physics
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        //Get Components
        rb = GetComponent<Rigidbody2D>();
        _disableTime = Time.time + timeToLive;
        //Set + Get Direction and Rotation
        //direction = _gun.direction;
        //transform.rotation = _gun.gunRotation * Quaternion.Euler(0, 0, -90);

        //InitaializeBulletStats();
        //transform.right = ((mousePos - (Vector2)transform.position)).normalized;
        //Vector3 rotation = transform.position - (Vector3)mousePos;
        //float rot = Mathf.Atan2(rotation.x, rotation.y);
        //transform.rotation = Quaternion.Euler(0, 0, rot + 90);
    }

    private void Update()
    {
        if (Time.time > _disableTime)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (bulletType == BulletType.Physics)
        {
            //rotate bullet in direction of velocity;
            transform.right = rb.linearVelocity;
            transform.rotation = transform.rotation * Quaternion.Euler(0, 0, -90);
        }
    }

    [ClientRpc]
    public void SetGun(Shooting gun) => _gun = gun;

    [ClientRpc]
    public void Launch(Vector2 direction, Quaternion rotation, Vector2 position)
    {
        //Debug.Log("Launch Bullet in direction: " + direction + " with rotation: " + rotation + " at position: " + position);
        this.direction = direction;
        transform.SetPositionAndRotation(position, rotation * Quaternion.Euler(0, 0, -90));
        this.gameObject.SetActive(true);
        InitaializeBulletStats();
    }
    private void InitaializeBulletStats()
    {
        if (bulletType == BulletType.Normal)
        {
            SetStraightVelocity();
            rb.gravityScale = 0;
        }
        else if (bulletType == BulletType.Physics)
        {
            SetPhysicsVelocity();
            rb.gravityScale = physicsBulletGravity;
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * normalBulletSpeed;
    }

    private void SetPhysicsVelocity()
    {
        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * physicsBulletSpeed;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //collision is within layermask
        if ((whatDestroysBullet.value & (1 << collision.gameObject.layer)) > 0)
        {
            //spawn Paritcles
            //Soundeffect
            //damage enemy
            //Screen shake
            gameObject.SetActive(false);
            Debug.Log("hit");
        }
    }
}
