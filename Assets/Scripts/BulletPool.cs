using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private Rigidbody defaultBullet;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private int poolSize = 20;

    private List<Rigidbody> bulletList = new List<Rigidbody>();

    public Rigidbody SpawnBullet()
    {
        Rigidbody bullet = null;

        if (bulletList.Count > 0)
        {
            bullet = bulletList[0];
            bulletList.RemoveAt(0);
            bullet.gameObject.SetActive(true);
        }
        else
        {
             bullet = Instantiate<Rigidbody>(defaultBullet);
        }
        
        player.onBulletStored += StoreBullet;       
      
        return bullet;
    }

    public void StoreBullet(Rigidbody targetBullet)
    {
        bulletList.Add(targetBullet);  
        targetBullet.gameObject.SetActive(false);
    }
    private void Start()
    {
        player = GetComponent<Player>();
    }
    private void Awake()
    {
        if (defaultBullet != null)
        {
            for (int i = 0; i < poolSize; i++)
            {
                Rigidbody bulletInstance = Instantiate<Rigidbody>(defaultBullet);
                StoreBullet(bulletInstance);
            }
        }
    }

}
