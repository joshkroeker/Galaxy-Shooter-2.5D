using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _lives = 3;

    [SerializeField] private float _speed = 3.5f;

    private float _xMaxBound = 12f;
    private float _xMinBound = -12f;
    private float _yMinBound = -3.8f;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private float _fireRate = 0.5f;
    private float _canFire = -1f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
    }

    private void CalculateMovement()
    {
        // get directional input from player and convert it to a Vector3(horizontalInput, verticalInput, 0f)
        Func<float, float, Vector3> direction = (float horizontalInput, float verticalInput)
            => new Vector3(horizontalInput, verticalInput, 0f);

        transform.Translate(direction(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * _speed * Time.deltaTime);

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

    public void Damage()
    {
        _lives--;

        if(_lives <= 0)
        {
            Destroy(gameObject);
        }
    }
}
