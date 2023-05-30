using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChargeBeam : Enemy
{
    [Header("Charge Beam Enemy")]
    [SerializeField] GameObject _chargeBeamPrefab;  

    protected override void Start()
    {
        base.Start();
        StartCoroutine(CalculatePathing());
        _pointsToAdd = 50;
    }

    private IEnumerator CalculatePathing()
    {
        while (_isAlive)
        {
            if (_moveIndex <= _path.Count - 1 && transform.position != _path[_moveIndex].position)
            {
                transform.position = Vector2.MoveTowards(transform.position, _path[_moveIndex].position, _speed * Time.deltaTime);
            }
            else if (_moveIndex <= _path.Count - 1 && transform.position == _path[_moveIndex].position)
            {
                _moveIndex++;

                if (_moveIndex > _path.Count - 1)
                {
                    Destroy(this.gameObject);
                }
                FireLaserBeam();

                yield return new WaitForSeconds(5f);
            }
        }
    }

    private void FireLaserBeam()
    {
        GameObject laserBeam = Instantiate(_chargeBeamPrefab, new Vector3(transform.position.x,
            transform.position.y + -7.75f, 0f), Quaternion.identity);
        Destroy(laserBeam, 5f);
    }

    public override void ReceivePathContainer(Transform pathContainer)
    {
        base.ReceivePathContainer(pathContainer);
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}
