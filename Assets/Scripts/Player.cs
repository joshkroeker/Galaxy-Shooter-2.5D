using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 3.5f;

    private float _xMaxBound = 12f;
    private float _xMinBound = -12f;
    private float _yMinBound = -3.8f;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
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
}
