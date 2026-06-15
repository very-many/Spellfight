using Mirror;
using System.Collections.Generic;
using UnityEngine;
using static Bullet;

public class BulletStats
{
    public List<BulletType> bulletTypes = new List<BulletType>() { BulletType.Normal };

    public float bulletDamage = 10;
    public float bulletHealth = 1;
    public float bulletSize = 1;
    public float bulletSpeed = 1;

    public Color bulletColor = Color.red;

    public GameObject owner;

    public float timeToLive = 5;
    public float timeToEscape = 0.2f;

    public float trailLength = 1;

    public int bounces = 3;

    public List<BulletStats> splitBullets = new List<BulletStats>();
    public float splitAngleOffset = 0;

    public float explosionRadius = 0;
    public float explosionDamage = 0;
    public float explosionDamageMultMaxRange = 1;

    //public float bulletScaleX = 1;
    //public float bulletScaleY = 1;

    //public float timeToLive = 3;

    //public float timeToEscape = 0,2;

    //public imagesrc = ???;

    //public hitbox = ???;

    public BulletStats() { }

    public BulletStats(List<BulletType> bulletTypes, float bulletDamage, float bulletHealth, float bulletSize, float bulletSpeed, Color bulletColor, GameObject owner)
    {
        this.bulletTypes = bulletTypes;
        this.bulletDamage = bulletDamage;
        this.bulletHealth = bulletHealth;
        this.bulletSize = bulletSize;
        this.bulletSpeed = bulletSpeed;
        this.bulletColor = bulletColor;
        this.owner = owner;
    }

    public BulletStats Clone()
    {
        BulletStats bulletStats = new BulletStats(bulletTypes, bulletDamage, bulletHealth, bulletSize, bulletSpeed, bulletColor, owner);

        bulletStats.trailLength = trailLength;

        bulletStats.timeToLive = timeToLive;
        bulletStats.timeToEscape = timeToEscape;

        bulletStats.bounces = bounces;

        bulletStats.splitBullets = splitBullets;
        bulletStats.splitAngleOffset = splitAngleOffset;

        bulletStats.explosionRadius = explosionRadius;
        bulletStats.explosionDamage = explosionDamage;
        bulletStats.explosionDamageMultMaxRange = explosionDamageMultMaxRange;

        return bulletStats;
    }
}
