using UnityEngine;
using System.Collections.Generic;

public class SpawnController : MonoBehaviour
{
    private List<Target> targetList = new List<Target>();

    private Target spawnGO;

    [SerializeField]
    private float spawnRate = 1f;

    [SerializeField]
    private float firstSpawnDelay = 0f;

    [SerializeField]
    private Player player;

    private Vector3 spawnPoint;

    private LowTargetPool lowTargetPool;
    private MidTargetPool midTargetPool;
    private HardTargetPool hardTargetPool;

    private void Awake()
    {
        lowTargetPool = GetComponent<LowTargetPool>();
        midTargetPool = GetComponent<MidTargetPool>();
        hardTargetPool = GetComponent<HardTargetPool>();
    }

    private void Start()
    {
        if(midTargetPool != null && hardTargetPool != null && lowTargetPool != null)
        {
            InvokeRepeating("SpawnObject", firstSpawnDelay, spawnRate);
            if (player != null)
            {
                player.OnPlayerDied += StopSpawning;
            }
        }
    }

    private void SpawnObject()
    {
        if (midTargetPool != null && hardTargetPool != null && lowTargetPool != null)
        {
            int random = Random.Range(1, 4);
            if (random == 1)
            {
                spawnGO = lowTargetPool.GetTarget();
                spawnGO.currentHP = 1;
            }
            else if (random == 2)
            {
                spawnGO = midTargetPool.GetTarget();
                spawnGO.currentHP = 2;
            }
            else
            {
                spawnGO = hardTargetPool.GetTarget();
                spawnGO.currentHP = 3;
            }

            spawnGO.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(
                    Random.Range(0F, 1F), 1F, transform.position.z));
        }          
    }

    private void StopSpawning()
    {
        CancelInvoke();
    }
}