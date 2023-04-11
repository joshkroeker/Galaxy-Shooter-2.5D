using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Health-related Variables")]
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject[] _engines;
    private int _lastEngineDamaged;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Power-up Related Variables")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedBoosted = 10f;
    [SerializeField] private float _thrusterSpeed = 8f;
    [SerializeField] private bool _isSpeedBoostActive = false;
    [SerializeField] private bool _isShieldActive = false;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private bool _isTripleShotActive = false;

    [Header("Ship Movement & Firing Variables")]
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField] AudioClip _fireLaserClip;
    [SerializeField] private float _xMaxBound = 12f;
    [SerializeField] private float _xMinBound = -12f;
    [SerializeField] private float _yMinBound = -3.8f;

    [Header("Miscellaneous Variables & Cached References")]
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private AudioSource _audioSource;
    [SerializeField] private int _score = 0;


    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
            Debug.LogError("The UI Manager is NULL");
        if (_spawnManager == null) 
            Debug.LogError("The spawn manager is NULL");
        if (_audioSource == null)
            Debug.LogError("The AudioSource on the Player is NULL");
        else
            _audioSource.clip = _fireLaserClip;
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

        float speed = CalculateSpeed();
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

    private float CalculateSpeed()
    {
        if(!_isSpeedBoostActive)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                return _thrusterSpeed;
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                return _speed;
            }
            else
            {
                return _speed;
            }

        }
        else if(_isSpeedBoostActive)
        {
            return _speedBoosted;
        }

        return -1f;
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

        _audioSource.Play();
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

        if (_lives == 2)
        {
            int engine = UnityEngine.Random.Range(0, 2);
            _engines[engine].SetActive(true);
            _lastEngineDamaged = engine;
        }
        else if (_lives == 1)
        {
            if (_lastEngineDamaged == 0)
            {
                _engines[1].SetActive(true);
            }
            else if (_lastEngineDamaged == 1)
            {
                _engines[0].SetActive(true);
            }
        }

        _uiManager.UpdateLives(_lives);

        if(_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
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

    public void AddScore(int scoreToAdd)
    {
        _score += scoreToAdd;
        _uiManager.AddScoreToText(_score);
    }

}
