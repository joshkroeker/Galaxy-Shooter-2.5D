using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitBox : MonoBehaviour
{
    [SerializeField] private int _hitBoxID;
    private Boss _boss;

    private void Start()
    {
        _boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerLaser(other))
        {
            if(_hitBoxID == 0) // do damage to right hitbox
            {
                _boss.ReceiveDamage(_hitBoxID, 10);
            }
            else if(_hitBoxID == 1) // do damage to left hitbox
            {
                _boss.ReceiveDamage(_hitBoxID, 10);
            }
        }
    }

    private bool IsPlayerLaser(Collider2D other)
    {
        return other.tag == "Laser" && other.TryGetComponent<Laser>(out Laser laser) && !laser.IsEnemyLaser;
    }
}
