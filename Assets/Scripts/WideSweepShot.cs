using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideSweepShot : MonoBehaviour
{
    // speed to move at
    [SerializeField] private float _speed;
    // destination point
    [SerializeField] private GameObject _destination;
    // explosion VFX reference
    [SerializeField] private GameObject _explosionPrefab;

    private void Update()
    {
        if(transform.position != _destination.transform.position)
        {
            transform.position = 
                Vector2.MoveTowards(transform.position, 
                _destination.transform.position, 
                _speed * Time.deltaTime);
        }
        else if(transform.position == _destination.transform.position)
        {
            Destroy(gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Destroy(gameObject);
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
        }
    }
}
