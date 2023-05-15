using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] float _delayBetweenSpawns = 1.0f;
    [SerializeField] int _enemiesInWave = 3;
    [SerializeField] bool _canSpawnNextWave = true;
    [SerializeField] int _increaseWaveLimitCounter = 0;
    [SerializeField] UIManager _uiManager;
    [SerializeField] GameObject[] _powerups;
    [SerializeField] GameObject _wideSweepPowerup;
    [SerializeField] private int _pathID;
    [SerializeField] private Transform[] _pathContainers;

    private bool _stopSpawning = false;

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnWideSweepRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        int currentEnemyCounter;

        while (!_stopSpawning && _canSpawnNextWave)
        {
            _pathID = Random.Range(0, 2);
            for (currentEnemyCounter = 0; currentEnemyCounter < _enemiesInWave; currentEnemyCounter++)
            {
                Transform pathContainer = _pathContainers[_pathID];
                Transform spawnPoint = pathContainer.GetChild(0);
                GameObject enemy = Instantiate(_enemyPrefab, spawnPoint.position, Quaternion.identity);
                enemy.transform.parent = _enemyContainer.transform;
                enemy.GetComponent<Enemy>().ReceivePathContainer(pathContainer);

                yield return new WaitForSeconds(_delayBetweenSpawns);
            }

            if (currentEnemyCounter < _enemiesInWave)
            {
                _canSpawnNextWave = false;
            }
            else if (currentEnemyCounter >= _enemiesInWave)
            {
                yield return new WaitForSeconds(8.0f);
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
            }
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            int randomPowerup = Random.Range(0, _powerups.Length);
            Instantiate(_powerups[randomPowerup], SetRandomPosition(), Quaternion.identity);

            int randomWait = Random.Range(4, 8);
            yield return new WaitForSeconds(randomWait);
        }
    }

    IEnumerator SpawnWideSweepRoutine()
    {
        yield return new WaitForSeconds(60f);

        while (!_stopSpawning)
        {
            Instantiate(_wideSweepPowerup, SetRandomPosition(), Quaternion.identity);
            int randomWait = Random.Range(20, 30);
            yield return new WaitForSeconds(randomWait);
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
