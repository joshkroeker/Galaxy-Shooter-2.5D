using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes { laserBasic, laserBeam }
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyTypes _enemyType;

    [Header("Generic Enemy Settings")]
    [SerializeField] private float _speed = 4f;
    [SerializeField] private int pointsToAdd = 10;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] GameObject _laserBeamPrefab;
    [SerializeField] float animationTime;
    [SerializeField] private List<Transform> _path = new List<Transform>();
    [SerializeField] private int _moveIndex = 1;
    private Player _player;
    private bool _isAlive = true;

    [Header("Enemy Basic Type Attributes")]
    private Animator _anim;
    [SerializeField] GameObject _laserPrefab;
    [SerializeField] AudioClip _fireLaserClip;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;

    private void Start()
    {       
        _player = FindObjectOfType<Player>();
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        if(_enemyType == EnemyTypes.laserBasic)
        {
            _anim = GetComponent<Animator>();

            if (_anim == null)
            {
                Debug.LogError("The Animator is NULL");
            }
        }

        if(_enemyType == EnemyTypes.laserBeam)
        {
            StartCoroutine(CalculatePathing());
        }
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
        GameObject laserBeam = Instantiate(_laserBeamPrefab, new Vector3(transform.position.x,
            transform.position.y + -7.75f, 0f), Quaternion.identity);
        Destroy(laserBeam, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemyType == EnemyTypes.laserBasic)
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

    void CalculateMovement()
    {
        if(_moveIndex <= _path.Count - 1 && transform.position != _path[_moveIndex].position)
        {
            transform.position = Vector2.MoveTowards(transform.position, _path[_moveIndex].position, _speed * Time.deltaTime);
        }
        else if(_moveIndex <= _path.Count - 1 && transform.position == _path[_moveIndex].position)
        {
            _moveIndex++;

            if (_moveIndex > _path.Count - 1)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void ReceivePathContainer(Transform pathContainer)
    {
        for (int i = 0; i < pathContainer.childCount; i++)
        {
            _path.Add(pathContainer.GetChild(i));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isAlive && other.TryGetComponent<Laser>(out Laser laser) && !laser.IsEnemyLaser)
        {
            Destroy(laser.gameObject);
            if (_player != null)
            {
                _player.AddScore(pointsToAdd);
            }
            InitiateEnemyDeathSequence();
        }
        else if (_isAlive && other.TryGetComponent<Player>(out Player player))
        {
            player.Damage();
            InitiateEnemyDeathSequence();
        }
    }

    private void InitiateEnemyDeathSequence()
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
