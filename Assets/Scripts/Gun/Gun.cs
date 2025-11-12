using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Unity.Cinemachine;

public class Gun : MonoBehaviour
{
    public static Action onShoot;

    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = 0.5f;

    private ObjectPool<Bullet> _bulletPool;
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Vector2 _mousePos;
    private float _lastFireTime = 0f;

    private CinemachineImpulseSource _impulseSource;
    private Animator _animator;


    private void Awake()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
        onShoot += ShootProjectile;
        onShoot += ResetLastFireTime;
        onShoot += FireAnimation;
        onShoot += GunScreenShake;
    }

    private void OnDisable()
    {
        onShoot -= ShootProjectile;
        onShoot -= ResetLastFireTime;
        onShoot -= FireAnimation;
        onShoot -= GunScreenShake;
    }

    public void ReleaseBulletFromPool(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }

    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(() =>
        {
            return Instantiate(_bulletPrefab);
        }, bullet => {
            bullet.gameObject.SetActive(true);
        }, Bullet =>
        {
            Bullet.gameObject.SetActive(false);
        }, bullet =>
        {
            Destroy(bullet);
        });
    }

    private void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime)
        {
            onShoot?.Invoke();
        }
    }

    private void ShootProjectile()
    {

        Bullet newBullet = _bulletPool.Get();
        newBullet.Intit(this, _bulletSpawnPoint.position, _mousePos);
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;

    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePos);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
