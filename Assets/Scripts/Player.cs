using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _lives = 3;
    [SerializeField] private bool _isShieldActive = false;

    [SerializeField] private float _speed = 3.5f;
    [SerializeField] private float _speedBoosted = 8.5f;
    [SerializeField] private bool _isSpeedBoostActive = false;

    [SerializeField] private float _xMaxBound = 12f;
    [SerializeField] private float _xMinBound = -12f;
    [SerializeField] private float _yMinBound = -3.8f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;

    private SpawnManager _spawnManager;

    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private bool _isTripleShotActive = false;

    [SerializeField] private GameObject _shieldVisualizer;

    void Start()
    {
       // transform.position = new Vector3(0f, 0f, 0f);
        _spawnManager = FindObjectOfType<SpawnManager>();

        if (_spawnManager == null) 
            Debug.LogError("The spawn manager is NULL");
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }

    private void CalculateMovement()
    {
        // get directional input from player and convert it to a Vector3(horizontalInput, verticalInput, 0f)
        Func<float, float, Vector3> direction = (float horizontalInput, float verticalInput) => new Vector3(horizontalInput, verticalInput, 0f);

        float speed = !_isSpeedBoostActive ? _speed : _speedBoosted;
        transform.Translate(direction(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed * Time.deltaTime);

        if (transform.position.x > _xMaxBound)
        {
            transform.position = new Vector3(_xMinBound, transform.position.y, 0f);
        }
        else if (transform.position.x < _xMinBound)
        {
            transform.position = new Vector3(_xMaxBound, transform.position.y, 0f);
        }

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yMinBound, 0f), 0f);

    }

    private void ShootLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1.05f, 0f), Quaternion.identity);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        _lives--;

        if(_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(gameObject);
        }
    }

    public void ActivateTripleShot()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isTripleShotActive = false;
    }

    public void ActivateSpeedBoost()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ActivateShields()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
}
