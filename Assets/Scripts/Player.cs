﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class Player : MonoBehaviour
{
    public delegate void OnBulletStored(Rigidbody targetBullet);
    public OnBulletStored onBulletStored;

    public const int PLAYER_LIVES = 3;

    private const float PLAYER_RADIUS = 0.4F;

    private UIController uiController;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 1F;

    [SerializeField]
    private int scoreAdd = 10;

    private BulletPool bulletPool;

    private float hVal;

    #region Bullet

    private Rigidbody bullet;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private float bulletSpeed = 3F;

    #endregion Bullet

    #region BoundsReferences

    private float referencePointComponent;
    private float leftCameraBound;
    private float rightCameraBound;

    #endregion BoundsReferences

    #region StatsProperties

    public int Score { get; set; }
    public int Lives { get; set; }

    #endregion StatsProperties

    #region MovementProperties

    public bool ShouldMove
    {
        get =>
            (hVal != 0F && InsideCamera) ||
            (hVal > 0F && ReachedLeftBound) ||
            (hVal < 0F && ReachedRightBound);
    }

    private bool InsideCamera
    {
        get => !ReachedRightBound && !ReachedLeftBound;
    }

    private bool ReachedRightBound { get => referencePointComponent >= rightCameraBound; }
    private bool ReachedLeftBound { get => referencePointComponent <= leftCameraBound; }

    private bool CanShoot { get => bulletSpawnPoint != null; }

    #endregion MovementProperties

    public Action OnPlayerDied;
    public Action OnPlayerHit;
    public Action OnPlayerScoreChanged;
    private void Awake()
    {
        bulletPool = GetComponent<BulletPool>();
    }

    private void Start()
    {
        leftCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(
            0F, 0F, 0F)).x + PLAYER_RADIUS;

        rightCameraBound = Camera.main.ViewportToWorldPoint(new Vector3(
            1F, 0F, 0F)).x - PLAYER_RADIUS;

        Lives = PLAYER_LIVES;
        uiController = FindObjectOfType<UIController>();
        if (this != null)
        {
            this.OnPlayerHit += HealthChanged;
            this.OnPlayerScoreChanged += ScoreChanged;
        }
    }

    public void ScoreChanged()
    {
        this.Score += scoreAdd;
    }

    public void HealthChanged()
    {
        this.Lives -= 1;
    }

    private void Update()
    {
        if (Lives <= 0)
        {
            uiController.Subscribe3();
            this.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            uiController.Subscribe2();
            uiController.Subscribe1();
            hVal = Input.GetAxis("Horizontal");

            if (ShouldMove)
            {
                transform.Translate(transform.right * hVal * moveSpeed * Time.deltaTime);
                referencePointComponent = transform.position.x;
            }

            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && CanShoot)
            {
                bullet = bulletPool.SpawnBullet();
                bullet.transform.position = bulletSpawnPoint.position;
                bullet.AddForce(transform.up * bulletSpeed, ForceMode.Impulse);
                Invoke("StoreBullet", 3F);
            }
        }
    }
    private void StoreBullet()
    {
        if (onBulletStored != null)
        {
            onBulletStored(bullet);
        }
    }
}