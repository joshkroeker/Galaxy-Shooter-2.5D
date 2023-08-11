using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Entry Scene Attributes")]
    [SerializeField] private float _entrySpeed = 3f;
    [SerializeField] CameraShake _cameraToShake;
    [SerializeField] private Vector3 _targetMovePositionOnScreen;
    [SerializeField] UIManager _uiManager;

    [Header("Boss Health Attributes")]
    [SerializeField] private GameObject[] _leftCannonDamages;
    [SerializeField] private GameObject[] _rightCannonDamages;
    [SerializeField] private int _leftHitBoxCurrentHP = 100;
    [SerializeField] private int _rightHitBoxCurrentHP = 100;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("Boss Phase One Attributes")]
    [SerializeField] private int _bossPhase = -1; // -1 for no phase, 0 = first stage, 1 = second phase
    [SerializeField] private BossCannon _leftCannon;
    [SerializeField] private BossCannon _rightCannon;
    public bool isChargeBeaming = false;

    [Header("Boss Phase Two Attributes")]
    [SerializeField] private int _mainBossHP = 200;
    [SerializeField] private Transform _posToSpawnLaserWaves;
    [SerializeField] private float _laserWaveShotDelay = 1.5f;
    [SerializeField] private GameObject _leftCornerLaserWave;
    [SerializeField] private GameObject _centerLaserWave;
    [SerializeField] private GameObject _rightCornerLaserWave;
    [SerializeField] private GameObject _bossFleetEnemy;
    [SerializeField] private BoxCollider2D _leftCannonCollider;
    [SerializeField] private BoxCollider2D _rightCannonCollider;
    [SerializeField] private Transform[] _fleetSpawnPositions;

    private AudioSource _audioSource;
    private Player _player;

    private void Start()
    {
        if(_uiManager == null)
        {
            _uiManager = FindObjectOfType<UIManager>();
        }
        if(_audioSource == null)
        {
            _audioSource = GetComponent<AudioSource>();
        }
        if(_player == null)
        {
            _player = FindObjectOfType<Player>();
        }
        if(_cameraToShake == null)
        {
            _cameraToShake = FindObjectOfType<CameraShake>();
        }
        _player.ReplenishAmmo();

    }

    public IEnumerator EntranceRoutine()
    {
        while(transform.position != _targetMovePositionOnScreen)
        {
            _cameraToShake.ShakeCamera();
            transform.position = Vector2.MoveTowards(transform.position, _targetMovePositionOnScreen, _entrySpeed);

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(2f);

        _uiManager.ShowBossHealthBars(100);

        StartCoroutine(StartAttackRoutine());
    }

    IEnumerator StartAttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        _bossPhase = 0;
        int rand = UnityEngine.Random.Range(0, 2);

        if(rand == 0)
        {
            _leftCannon.isLaserProngAttack = true;
            _leftCannon.isChargeBeamAttack = false;
            _rightCannon.isLaserProngAttack = true;
            _rightCannon.isChargeBeamAttack = false;
        }
        else if(rand == 1)
        {
            _leftCannon.isLaserProngAttack = false;
            _leftCannon.isChargeBeamAttack = true;
            _rightCannon.isLaserProngAttack = false;
            _rightCannon.isChargeBeamAttack = true;
        }

        _leftCannon.ActivateCannons();
        _rightCannon.ActivateCannons();
    }

    public void ReceiveDamage(int hitbox, int damageReceived)
    {
        if (_bossPhase == 0)
        {
            if (isChargeBeaming)
            {
                return;
            }

            if (hitbox == 0) // do damage to right hitbox
            {
                _rightHitBoxCurrentHP -= damageReceived;
                _uiManager.UpdateRightCannonHP(_rightHitBoxCurrentHP);

                if (_rightHitBoxCurrentHP == 50)
                {
                    int rand = UnityEngine.Random.Range(0, 2);
                    _rightCannonDamages[rand].SetActive(true);
                    _cameraToShake.ShakeCamera();
                }
                else if (_rightHitBoxCurrentHP == 0)
                {
                    _rightCannonDamages[0].SetActive(true);
                    _rightCannonDamages[1].SetActive(true);
                    _cameraToShake.ShakeCamera();

                    _rightCannon.gameObject.SetActive(false);
                }
            }
            else if (hitbox == 1) // do damage to left hitbox
            {
                _leftHitBoxCurrentHP -= damageReceived;
                _uiManager.UpdateLeftCannonHP(_leftHitBoxCurrentHP);

                if (_leftHitBoxCurrentHP == 50)
                {
                    int rand = UnityEngine.Random.Range(0, 2);
                    _leftCannonDamages[rand].SetActive(true);
                    _cameraToShake.ShakeCamera();
                }
                else if (_leftHitBoxCurrentHP == 0)
                {
                    _leftCannonDamages[0].SetActive(true);
                    _leftCannonDamages[1].SetActive(true);
                    _cameraToShake.ShakeCamera();

                    _leftCannon.gameObject.SetActive(false);
                }
            }

            if (_rightHitBoxCurrentHP == 0 && _leftHitBoxCurrentHP == 0)
            {
                _bossPhase = 1;
                _cameraToShake.ShakeCamera();

                StartCoroutine(SecondPhaseAttackRoutine());
                _player.ReplenishAmmo();

            }
        }
        else if(_bossPhase == 1)
        {
            this._mainBossHP -= damageReceived;
            _uiManager.UpdateMainShipHP(_mainBossHP);

            if(_mainBossHP <= 0)
            {
                _uiManager.ShowWinScreen();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayerLaser(other) && _bossPhase != -1)
        {
            ReceiveDamage(-1, 10);

            GameObject explosion = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
            Destroy(explosion, 0.6f);
        }
    }

    public static bool IsPlayerLaser(Collider2D other)
    {
        return other.tag == "Laser" && other.TryGetComponent<Laser>(out Laser laser) && !laser.IsEnemyLaser;
    }

    private IEnumerator SecondPhaseAttackRoutine()
    {
        _uiManager.ShowMainShipHP();
        _leftCannonCollider.enabled = false;
        _rightCannonCollider.enabled = false;

        yield return new WaitForSeconds(1f);

        _bossPhase = 1;
        int rand = UnityEngine.Random.Range(0, 2);

        if (rand == 0) // laser wave
        {
            StartCoroutine(CallLaserWavesRoutine());
        }
        else if(rand == 1) // boss fleet 
        {
            StartCoroutine(BossFleetAttackRoutine());
        }

    }

    IEnumerator BossFleetAttackRoutine()
    {
        yield return new WaitForSeconds(1f);

        int duration = 10;

        for (int i = 0; i < duration; i++)
        {
            if (_mainBossHP <= 0)
                break;

            // choose a random point to spawn from
            int rand = UnityEngine.Random.Range(0, _fleetSpawnPositions.Length);
            switch (rand)
            {
                case 0:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[0].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[3].position, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[1].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[5].position, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[2].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[4].position, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[3].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[6].position, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[4].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[3].position, Quaternion.identity);
                    break;
                case 5:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[5].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[1].position, Quaternion.identity);
                    break;
                case 6:
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[6].position, Quaternion.identity);
                    Instantiate(_bossFleetEnemy, _fleetSpawnPositions[0].position, Quaternion.identity);
                    break;
            }

            yield return new WaitForSeconds(1f);
        }

        if(_mainBossHP > 0)
        {
            StartCoroutine(CallLaserWavesRoutine());
        }
    }

    IEnumerator CallLaserWavesRoutine()
    {
        for (int i = 0; i < 2; i++)
        {
            if (_mainBossHP <= 0)
                break;

            Instantiate(_leftCornerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(_laserWaveShotDelay);

            Instantiate(_centerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(_laserWaveShotDelay);

            Instantiate(_rightCornerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(_laserWaveShotDelay);

            Instantiate(_leftCornerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(_laserWaveShotDelay);

            Instantiate(_centerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(_laserWaveShotDelay);

            Instantiate(_rightCornerLaserWave, _posToSpawnLaserWaves.position, Quaternion.identity);

            yield return new WaitForSeconds(1f);
        }

        if(_mainBossHP > 0)
        {
            StartCoroutine(BossFleetAttackRoutine());
        }
    }

    public int BossPhase
    {
        get { return _bossPhase; }
        set
        {
            if(value == 0 || value == 1)
            {
                _bossPhase = value;
            }
        }
    }

    public int RightCannonHP
    {
        get { return _rightHitBoxCurrentHP; }
    }

    public int LeftCannonHP
    {
        get { return _leftHitBoxCurrentHP; }
    }
}
