using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveEnemy : Enemy
{
    [Header("Aggressive Enemy Attribute")]
    [SerializeField] private int _minDistanceToRam = 3;

    protected override void Update()
    {
        if(_enemyType == EnemyTypes.aggressive && Vector2.Distance(transform.position, _player.transform.position) <= _minDistanceToRam)
        {
            Ram();
        }

        base.Update();
    }

    private void Ram()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
    }
}
