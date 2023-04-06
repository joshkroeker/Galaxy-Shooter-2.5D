using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] GameObject _explosionPrefab;

    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _rotateSpeed = 3f;

    private SpawnManager _spawnManager;

    private void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();

        if(_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Laser")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(gameObject, 0.25f);
        }
    }
}
