using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidingMissile : MonoBehaviour
{
    // TODO: Add functionality to track Boss Hitboxes

    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _detectionRange = 5f;

    private Enemy _target;
    private bool _hasFoundTarget = false;

    private void Update()
    {
        if (!_hasFoundTarget)
        {
            Move();

            if(FindObjectsOfType<Enemy>().Length > 0)
                FindClosestTarget();
        }
        else if(_hasFoundTarget && _target != null)
        {
            MoveToTarget();
        }
        else if(_hasFoundTarget && _target == null)
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.up * _speed * Time.deltaTime);
    }

    private void FindClosestTarget()
    {
        foreach(Enemy enemy in FindObjectsOfType<Enemy>())
        {
            if(_target == null)
            {
                _target = enemy;
                return;
            }
            else if(Vector2.Distance(enemy.transform.position, transform.position) > _detectionRange)
            {
                return;
            }

            if(Vector2.Distance(_target.transform.position, transform.position) > Vector2.Distance(enemy.transform.position, transform.position))
            {
                _target = enemy;
            }
        }

        if(_target != null)
            _hasFoundTarget = true;
    }

    private void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, (_speed * 2f) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
