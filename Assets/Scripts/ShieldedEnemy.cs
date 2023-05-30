using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldedEnemy : Enemy
{
    [Header("Shielded Enemy Attributes")]
    [SerializeField] GameObject _shields;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ReceiveDamage()
    {
        if (_enemyType == EnemyTypes.shielded && _shields.activeSelf)
        {
            _shields.SetActive(false);
            return;
        }

        base.ReceiveDamage();
    }
}
