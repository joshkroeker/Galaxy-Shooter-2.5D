using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] AudioClip _explosionClip;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the Explosion is NULL");
        }
        else
        {
            _audioSource.clip = _explosionClip;
            _audioSource.Play();
        }

        Destroy(gameObject, 2.4f);
    }
}
