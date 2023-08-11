using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFleetEnemy : Enemy
{
    [Header("Boss Fleet Enemy Attributes")]
    [SerializeField] private float _moveSpeed = 5.5f;

    // Update is called once per frame
    protected override void Update()
    {
        transform.Translate(Vector2.down * _moveSpeed * Time.deltaTime);
    }

}
