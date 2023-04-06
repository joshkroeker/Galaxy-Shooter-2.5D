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

    private bool _canStillCollide = true;

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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -5.5f)
        {
            float randomX = Random.Range(_xScreenMinBound, _xScreenMaxBound);
            transform.position = new Vector3(randomX, 7f, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Laser>(out Laser laser) && _canStillCollide)
        {
            Destroy(laser.gameObject);
            if (_player != null)
            {               
                _player.AddScore(pointsToAdd);
            }
            InitiateEnemyDeathSequence();
        }
        else if (other.TryGetComponent<Player>(out Player player) && _canStillCollide)
        {
            player.Damage();
            InitiateEnemyDeathSequence();
        }
    }

    private void InitiateEnemyDeathSequence()
    {
        _canStillCollide = false;
        _speed = 0f;
        _anim.SetTrigger("OnEnemyDeath");
        Destroy(gameObject, animationTime);
    }
}
