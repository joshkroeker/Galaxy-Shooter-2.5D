using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject _enemyLaserPrefab;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] float _delayBetweenSpawns = 1.0f;
    [SerializeField] int _enemiesInWave = 3;
    [SerializeField] bool _canSpawnNextWave = true;
    [SerializeField] int _increaseWaveLimitCounter = 0;
    [SerializeField] UIManager _uiManager;
    [SerializeField] GameObject[] _commmonPowerups;
    [SerializeField] GameObject[] _rarePowerups;
    [SerializeField] private int _pathID;
    [SerializeField] private Transform[] _pathContainers;

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
            _pathID = Random.Range(0, 2);
            for (currentEnemyCounter = 0; currentEnemyCounter < _enemiesInWave; currentEnemyCounter++)
            {
                EnemySpawnPathSetup(_enemyPrefab);

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

                _pathID = 2; // path type for this specific enemy
                EnemySpawnPathSetup(_enemyLaserPrefab);
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

    // potentially condense some of the coroutines
    // list out which powerups should be rare - widesweep, health, and shield
    // declare a time when rare spawns should happen - every 35 seconds
    // list out which powerups should be not rare - triple shot, speed, and ammo
    // declare a time when not rare spawns should happen - every 15 seconds
    // sync this up with enemy spawning timings - enemy waves spawn every 15 seconds roughly

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        // counter 
        int counter = 0;
        while (!_stopSpawning)
        {
            //increment counter
            counter++;

            // check counter % 15 = 0
            if(counter % 15 == 0)
            {
                // spawn random common powerup
                int rand = Random.Range(0, _commmonPowerups.Length);
                Instantiate(_commmonPowerups[rand], SetRandomPosition(), Quaternion.identity);
            }
            else if(counter % 35 == 0)
            {
                // spawn random rare powerup
                int rand = Random.Range(0, _rarePowerups.Length);
                Instantiate(_rarePowerups[rand], SetRandomPosition(), Quaternion.identity);
                //counter = 0
                counter = 0;
            }

            // delay by 1 second
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
