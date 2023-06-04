using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidEnemy : Enemy
{
    [Header("Avoid Enemy Attributes")]
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _distance = 5.5f;
    [SerializeField] private LayerMask _playerProjectileLayerMask;
    private float _dodgeAmount = 3f;
    private float _dodgeSpeed = 4f;

    protected override void Start()
    {
        base.Start();

        int rand = Random.Range(0, 2);
        if(rand == 0)
        {
            _dodgeAmount = 3f;
        }
        else if(rand == 1)
        {
            _dodgeAmount = -3f;
        }
    }

    protected override void Update()
    {
        if (_enemyType == EnemyTypes.avoid && IsProjectileInRange())
        {
            Vector2 newPos = new Vector2(transform.position.x + _dodgeAmount, transform.position.y);

            if (Mathf.Sign(_dodgeAmount) == -1f)
            {
                if(Mathf.Sign(transform.position.x) == -1f)
                {
                    newPos = new Vector2(transform.position.x - (-1f *_dodgeAmount), transform.position.y);
                }
                else if(Mathf.Sign(transform.position.x) == 1f)
                {
                    newPos = new Vector2(transform.position.x + _dodgeAmount, transform.position.y);
                }
            }

            if (Vector2.Distance(transform.position, newPos) > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, newPos, _dodgeSpeed * Time.deltaTime);
            }
        }
        else
        {
            base.Update();
        }
    }

    private bool IsProjectileInRange()
    {
        return Physics2D.CircleCast(transform.position, _radius, Vector2.zero, _distance, _playerProjectileLayerMask);
    }
}
