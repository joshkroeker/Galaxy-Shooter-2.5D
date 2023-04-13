using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;

    [SerializeField] private float _xScreenMinBound = -11f;
    [SerializeField] private float _xScreenMaxBound = 11f;

    [SerializeField] private int pointsToAdd = 10;

    private Player _player;
    private Animator _anim;
    [SerializeField] float animationTime; // length of destroy anim

    private bool _isAlive = true;

    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] GameObject _laserPrefab;
    [SerializeField] AudioClip _fireLaserClip;

    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private void Start()
    {
        _player = FindObjectOfType<Player>();

        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        _anim = GetComponent<Animator>();

        if(_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (_isAlive && Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(_fireLaserClip, transform.position);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].SetEnemyLaser();
            }
        }
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            float randomX = Random.Range(_xScreenMinBound, _xScreenMaxBound);
            transform.position = new Vector3(randomX, 7f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAlive && other.TryGetComponent<Laser>(out Laser laser))
        {
            Destroy(laser.gameObject);
            if (_player != null)
            {               
                _player.AddScore(pointsToAdd);
            }
            InitiateEnemyDeathSequence();
        }
        else if (_isAlive && other.TryGetComponent<Player>(out Player player))
        {
            player.Damage();
            InitiateEnemyDeathSequence();
        }
    }

    private void InitiateEnemyDeathSequence()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        _isAlive = false;
        _speed = 0f;
        _anim.SetTrigger("OnEnemyDeath");

        Destroy(gameObject, animationTime);
    }
}
