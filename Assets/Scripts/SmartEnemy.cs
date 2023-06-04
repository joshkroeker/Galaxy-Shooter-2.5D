using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : Enemy
{
    [Header("Smart Enemy Attributes")]
    [SerializeField] private float _distance;
    [SerializeField] private LayerMask _playerLayerMask;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;

    protected override void Start()
    {
        base.Start();

        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if(IsPlayerBehind())
        {
            Vector2 lookDirection = _player.transform.position - transform.position;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 90f;
            _rigidbody.rotation = angle;

            // shoot at player
            ShootPlayer();
        }
        else
        {
            _rigidbody.rotation = 0;
        }
    }

    private bool IsPlayerBehind()
    {
        return Physics2D.BoxCast(transform.position, _collider.bounds.size, 0f, Vector2.up, _distance, _playerLayerMask); 
    }

    private void ShootPlayer()
    {
        if (_isAlive && Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 5f);
            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_fireLaserClip, transform.position);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].IsSmartEnemyLaser = true;
                lasers[i].IsEnemyLaser = true;
            }
        }
    }
}
