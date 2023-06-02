using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float _speed = 8f;
    private bool _isEnemyLaser = false;
    private bool _isSmartEnemyLaser = false;

    // Update is called once per frame
    void Update()
    {
        if (!_isEnemyLaser)
        {
            MoveUp();
        }
        else if (_isSmartEnemyLaser && _isEnemyLaser)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (gameObject.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            if (gameObject.transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);
        }
    }
    
    public bool IsEnemyLaser
    {
        get { return this._isEnemyLaser; }
        set
        {
            _isEnemyLaser = value;
        }
    }

    public bool IsSmartEnemyLaser
    {
        get { return this._isSmartEnemyLaser; }
        set
        {
            _isSmartEnemyLaser = value;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.gameObject.GetComponent<Player>();

            if(player != null)
            {
                player.Damage();
                Destroy(gameObject);
            }
        }
    }
}
