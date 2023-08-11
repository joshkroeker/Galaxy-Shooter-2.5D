using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCannon : MonoBehaviour
{
    [SerializeField] private int _cannonID;
    [SerializeField] private GameObject _laserProngAttackPrefab;
    [SerializeField] private GameObject _chargeBeamAttackPrefab;
    [SerializeField] private Transform[] _cannonTransforms;
    [SerializeField] private GameObject _explosionPrefab;
    private List<GameObject> _liveProjectilesList = new List<GameObject>();

    public bool isLaserProngAttack = false;
    public bool isChargeBeamAttack = false;

    private Boss _boss;

    private int lastAttackPerformed = -1; // -1 = NA, 0 = Three Prong, 1 = Charge Beam

    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {
        _boss = GetComponentInParent<Boss>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Boss.IsPlayerLaser(other) && _boss.BossPhase != -1)
        {
            if(_cannonID == 0) // do damage to right hitbox
            {
                _boss.ReceiveDamage(_cannonID, 10);
                GameObject explosion = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(explosion, 0.6f);
            }
            else if(_cannonID == 1) // do damage to left hitbox
            {
                _boss.ReceiveDamage(_cannonID, 10);
                GameObject explosion = Instantiate(_explosionPrefab, other.transform.position, Quaternion.identity);
                Destroy(explosion, 0.6f);
            }
        }
    }

    private IEnumerator BossPhaseOneRoutine()
    { 
        if (gameObject.activeSelf)
        {
            if (isLaserProngAttack)
            {
                float maxTimer = 5f;
                float currentTimer = 0.0f;

                lastAttackPerformed = 0;

                while (currentTimer < maxTimer && _boss.BossPhase == 0)
                {
                    foreach (Transform firePoint in _cannonTransforms)
                    {
                        if (firePoint == null)
                            break;

                        GameObject laserProng = Instantiate(_laserProngAttackPrefab, firePoint.position, Quaternion.identity);
                        _liveProjectilesList.Add(laserProng);
                    }

                    _audioSource.Play();

                    currentTimer++;
                    yield return new WaitForSeconds(1.5f);
                }
            }
            else if(isChargeBeamAttack)
            {
                lastAttackPerformed = 1;

                GameObject chargeBeam = Instantiate(_chargeBeamAttackPrefab, new Vector3(transform.position.x, -3.11f, 0f), Quaternion.identity);
                _liveProjectilesList.Add(chargeBeam);
                _boss.isChargeBeaming = true;

                yield return new WaitForSeconds(5f);

                _boss.isChargeBeaming = false;
                _liveProjectilesList.Remove(chargeBeam);
                Destroy(chargeBeam);
            }

            DetermineAttack();
        }
        else
        {
            foreach (GameObject gameObject in _liveProjectilesList)
            {
                _liveProjectilesList.Remove(gameObject);
                Destroy(gameObject);
            }

            isLaserProngAttack = false;
            isChargeBeamAttack = false;
        }
    }

    private void DetermineAttack()
    {
        if(lastAttackPerformed == 0)
        {
            isLaserProngAttack = false;
            isChargeBeamAttack = true;
        }
        else if(lastAttackPerformed == 1)
        {
            isLaserProngAttack = true;
            isChargeBeamAttack = false;
        }

        ActivateCannons();
    }

    public void ActivateCannons()
    {
        StartCoroutine(BossPhaseOneRoutine());
    }
}
