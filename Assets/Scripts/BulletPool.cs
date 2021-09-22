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
            //player.enabled = true;
        }
        else
        {
             bullet = Instantiate<Rigidbody>(defaultBullet);
        }
        
        player.onBulletStored += StoreBullet;       
      
        return bullet;
    }

    private void StoreBullet(Rigidbody targetBullet)
    {
        //player.onBulletStored -= StoreBullet;

        bulletList.Add(targetBullet);
        targetBullet.velocity = Vector3.zero;
        targetBullet.gameObject.SetActive(false);
        //player.enabled = false;
        targetBullet.transform.position = transform.position;
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
