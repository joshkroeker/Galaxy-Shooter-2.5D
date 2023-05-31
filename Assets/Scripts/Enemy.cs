using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { laserBasic, chargeBeam, shielded, aggressive }
public class Enemy : MonoBehaviour
{
    [Header("Basic Enemy Attributes")]
    [SerializeField] float animationTime = 2.3f;
    private Animator _anim;

    [Header("Base Attributes")]
    [SerializeField] protected EnemyTypes _enemyType;
    [SerializeField] protected float _speed = 4f;
    [SerializeField] protected int _pointsToAdd = 10;
    [SerializeField] protected List<Transform> _path = new List<Transform>();
    [SerializeField] protected int _moveIndex = 1;
    [SerializeField] protected GameObject _explosionPrefab;
    [SerializeField] protected GameObject _laserPrefab;
    [SerializeField] protected AudioClip _fireLaserClip;
    protected Player _player;
    protected bool _isAlive = true;
    protected float _canFire = -1f;
    protected float _fireRate = 3.0f;

    protected virtual void Start()
    {       
        _player = FindObjectOfType<Player>();
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
            Destroy(this);
        }

        if (_enemyType == EnemyTypes.laserBasic)
        {
            _anim = GetComponent<Animator>();
            if (_anim == null)
            {
                Debug.LogError("The Animator is NULL");
            }
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_enemyType != EnemyTypes.chargeBeam)
        {
            CalculateMovement();

            if (_isAlive && Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;

                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(_fireLaserClip, transform.position);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].IsEnemyLaser = true;
                }
            }
        }
    }

    private void CalculateMovement()
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
        }
    }

    public virtual void ReceivePathContainer(Transform pathContainer)
    {
        for (int i = 0; i < pathContainer.childCount; i++)
        {
            _path.Add(pathContainer.GetChild(i));
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAlive && other.TryGetComponent<Laser>(out Laser laser) && !laser.IsEnemyLaser)
        {
            Destroy(laser.gameObject);
            if (_player != null)
            {
                _player.AddScore(_pointsToAdd);
            }
            ReceiveDamage();
        }
        else if (_isAlive && other.TryGetComponent<Player>(out Player player))
        {
            player.Damage();
            ReceiveDamage();
        }
    }

    protected virtual void ReceiveDamage()
    {
        _isAlive = false;
        _speed = 0f;
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

        if(_enemyType == EnemyTypes.laserBasic)
        {
            _anim.SetTrigger("OnEnemyDeath");
            Destroy(gameObject, animationTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
