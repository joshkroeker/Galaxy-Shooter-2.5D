using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] float _enemySpawnDelay = 3.0f;
    [SerializeField] GameObject[] _powerups;
    [SerializeField] GameObject _wideSweepPowerup;

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

        while (!_stopSpawning)
        {
            GameObject enemy = Instantiate(_enemyPrefab, SetRandomPosition(), Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(_enemySpawnDelay);
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
