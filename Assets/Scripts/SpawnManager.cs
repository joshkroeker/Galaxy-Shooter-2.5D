using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawnables")]
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _shieldedEnemyPrefab;
    [SerializeField] GameObject _enemyChargeBeamPrefab;
    [SerializeField] GameObject _aggressiveEnemyPrefab;
    [SerializeField] GameObject _smartEnemyPrefab;
    [SerializeField] GameObject[] _commmonPowerups;
    [SerializeField] GameObject[] _rarePowerups;

    [Header("Wave Settings")]
    [SerializeField] bool _canSpawnNextWave = true;
    [SerializeField] int _enemiesInWave = 3;
    [SerializeField] float _delayBetweenSpawns = 1.0f;
    [SerializeField] int _increaseWaveLimitCounter = 0;
    [SerializeField] private int _pathID;
    [SerializeField] private Transform[] _pathContainers;

    [Header("References")]
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] UIManager _uiManager;
    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        int currentEnemyCounter;

        while (!_stopSpawning && _canSpawnNextWave)
        {
            _pathID = Random.Range(0, 3);
            for (currentEnemyCounter = 0; currentEnemyCounter < _enemiesInWave; currentEnemyCounter++)
            {
                if(currentEnemyCounter == (_enemiesInWave - 1))
                {                    
                    EnemySpawnPathSetup(_shieldedEnemyPrefab);
                }
                else
                {
                    int rand = Random.Range(0, 5);
                    if (rand == 2)
                    {
                        EnemySpawnPathSetup(_aggressiveEnemyPrefab);
                    }
                    else if(rand == 1 || rand == 4)
                    {
                        EnemySpawnPathSetup(_smartEnemyPrefab);
                    }
                    else
                    {
                        EnemySpawnPathSetup(_enemyPrefab);
                    }
                }

                yield return new WaitForSeconds(_delayBetweenSpawns);
            }

            if (currentEnemyCounter < _enemiesInWave)
            {
                _canSpawnNextWave = false;
            }
            else if (currentEnemyCounter >= _enemiesInWave)
            {
                yield return new WaitForSeconds(6.0f);
                _canSpawnNextWave = true;
                currentEnemyCounter = 0;
                _increaseWaveLimitCounter++;
            }

            if (_increaseWaveLimitCounter > 5)
            {
                _increaseWaveLimitCounter = 0;
                _enemiesInWave++;
                _uiManager.ActivateWaveIncomingEffect();
                yield return new WaitForSeconds(5f);

                _pathID = 3; // path type for this specific enemy
                EnemySpawnPathSetup(_enemyChargeBeamPrefab);
            }
        }
    }

    private void EnemySpawnPathSetup(GameObject enemyPrefab)
    {
        Transform pathContainer = _pathContainers[_pathID];
        Transform spawnPoint = pathContainer.GetChild(0);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        enemy.transform.parent = _enemyContainer.transform;
        enemy.GetComponent<Enemy>().ReceivePathContainer(pathContainer);
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        int counter = 0;
        while (!_stopSpawning)
        {
            counter++;

            if(counter % 10 == 0)
            {
                int rand = Random.Range(0, _commmonPowerups.Length);
                Instantiate(_commmonPowerups[rand], SetRandomPosition(), Quaternion.identity);
            }
            else if(counter % 28 == 0)
            {
                int rand = Random.Range(0, _rarePowerups.Length);
                Instantiate(_rarePowerups[rand], SetRandomPosition(), Quaternion.identity);
                counter = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    private Vector3 SetRandomPosition()
    {
        float randomX = Random.Range(-7f, 7f);
        return new Vector3(randomX, 7f, 0f);
    }
}
