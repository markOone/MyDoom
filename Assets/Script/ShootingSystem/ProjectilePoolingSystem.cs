using System.Collections;
using System.Collections.Generic;
using MyDoom.ShootingSystem;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolingSystem: MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    private IObjectPool<GameObject> objectPool;
    private bool collectionCheck = true;

    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 100;
    
    public IObjectPool<GameObject> ObjectPool => objectPool;
    
    void Awake()
    {
        objectPool = new ObjectPool<GameObject>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
    }

    GameObject CreateProjectile()
    {
        GameObject projectileInstance = Instantiate(projectilePrefab);
        projectileInstance.GetComponent<ProjectileScript>().ObjectPool = objectPool;
        return projectileInstance;
    }

    void OnGetFromPool(GameObject pooledProjectile)
    {
        pooledProjectile.gameObject.SetActive(true);
    }

    void OnReleaseToPool(GameObject releasedProjectile)
    {
        releasedProjectile.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject pooledProjectile)
    {
        Destroy(pooledProjectile.gameObject);
    }
}
