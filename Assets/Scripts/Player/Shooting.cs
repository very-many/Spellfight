using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Shooting : NetworkBehaviour
{
    [SerializeField]
    private InputActionReference pointerPosition;
    public SpriteRenderer characterRenderer, weaponRenderer;
    public bool isFiring = false;


    public GameObject bullet;
    public Transform bulletTransform;
    public bool canFire = true;
    private float timer;
    public float timeBetweenFiring;

    public Quaternion gunRotation;
    public Vector2 direction;
    private float safeState1;
    private float safeState2;

    void Start()
    {
        safeState1 = transform.localScale.y;
        safeState2 = -1 * transform.localScale.y;
    }

    void Update()
    {
        Aim();
        if(!isServer)
            return;
        ShouldShoot();
    }

    public void onFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isFiring = true;
        }
        if (context.canceled)
        {
            isFiring = false;
        }
    }

    private void ShouldShoot()
    {
        if (canFire && isFiring)
        {
            Firing();
        }
        else if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > timeBetweenFiring)
            {
                canFire = true;
                timer = 0;
            }
        }
    }

    private void Aim()
    {
        Vector2 pointerPosition = GetPointerInput();
        direction = (pointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;
        gunRotation = transform.rotation;

        Vector2 scale = transform.localScale;
        if (gunRotation.y == 1)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (direction.x <= 0)
        {
            scale.y = safeState2;
        }
        else if (direction.x > 0)
        {
            scale.y = safeState1;
        }
        transform.localScale = scale;

        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 2;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 2;
        }
    }

    private Vector2 GetPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void Firing()
    {
        canFire = false;
        //var spawnedBullet = Instantiate(bullet, bulletTransform.position, gunRotation * Quaternion.Euler(0, 0, -90));
        //NetworkServer.Spawn(spawnedBullet.gameObject);
        //Bullet spawnedBulletScript = spawnedBullet.GetComponent<Bullet>();
        //spawnedBulletScript.SetGun(this);
        //spawnedBulletScript.InitaializeBulletStats();

        GameObject spawnedBullet = ObjectPool.instance.GetPooledObject();

        if (spawnedBullet != null)
        {
            Bullet spawnedBulletScript = spawnedBullet.GetComponent<Bullet>();
            spawnedBulletScript.SetGun(this);
            spawnedBulletScript.Launch(direction, gunRotation, bulletTransform.position);
        }
    }
}

