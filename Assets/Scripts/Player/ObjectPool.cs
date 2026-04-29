using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectPool : NetworkBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 20;

    [SerializeField]
    private GameObject bulletPrefab;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            InstantiateBullet();
        }
    }

    private GameObject InstantiateBullet()
    {
        GameObject obj = Instantiate(bulletPrefab);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        NetworkServer.Spawn(obj);
        return obj;
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        return InstantiateBullet();
    }
}
