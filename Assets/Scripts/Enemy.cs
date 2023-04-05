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

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5.5f)
        {
            float randomX = Random.Range(_xScreenMinBound, _xScreenMaxBound);
            transform.position = new Vector3(randomX, 7f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Laser>(out Laser laser))
        {
            Destroy(laser.gameObject);
            if(_player != null)
            {
                _player.AddScore(pointsToAdd);
            }
            Destroy(gameObject);
        }
        else if (other.TryGetComponent<Player>(out Player player))
        {
            player.Damage();
            Destroy(gameObject);
        }
    }
}
