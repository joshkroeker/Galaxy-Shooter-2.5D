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
    [SerializeField] private bool _turnOnUI = false;
    [SerializeField] UIManager _uiManager;

    [Header("Boss Health Attributes")]
    [SerializeField] private int _leftHitBoxCurrentHP = 100;
    [SerializeField] private int _rightHitBoxCurrentHP = 100;
    [SerializeField] private GameObject[] _leftCannonDamages;
    [SerializeField] private GameObject[] _rightCannonDamages;


    private void Start()
    {
        if(_uiManager == null)
        {
            _uiManager = FindObjectOfType<UIManager>();
        }

    }

    private void Update()
    {
        Entrance();
    }

    private void Entrance()
    {
        if (transform.position != _targetMovePositionOnScreen)
        {
            _cameraToShake.ShakeCamera();
            transform.position = Vector2.MoveTowards(transform.position, _targetMovePositionOnScreen, _entrySpeed * Time.deltaTime);
        }
        else
        {
            _turnOnUI = true;
        }

        if (_turnOnUI)
        {
            _uiManager.ShowBossHealthBars(100);
            _turnOnUI = false;
        }
    }

    public void ReceiveDamage(int hitbox, int damageReceived)
    {
        if(hitbox == 0) // do damage to right hitbox
        {
            _rightHitBoxCurrentHP -= damageReceived;
            _uiManager.UpdateRightCannonHP(_rightHitBoxCurrentHP);

            if(_rightHitBoxCurrentHP == 50)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                _rightCannonDamages[rand].SetActive(true);
                _cameraToShake.ShakeCamera();
            }
            else if(_rightHitBoxCurrentHP == 0)
            {
                _rightCannonDamages[0].SetActive(true);
                _rightCannonDamages[1].SetActive(true);
                _cameraToShake.ShakeCamera();
            }
        }
        else if(hitbox == 1) // do damage to left hitbox
        {
            _leftHitBoxCurrentHP -= damageReceived;
            _uiManager.UpdateLeftCannonHP(_leftHitBoxCurrentHP);

            if (_leftHitBoxCurrentHP == 50)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                _leftCannonDamages[rand].SetActive(true);
                _cameraToShake.ShakeCamera();

            }
            else if(_leftHitBoxCurrentHP == 0)
            {
                _leftCannonDamages[0].SetActive(true);
                _leftCannonDamages[1].SetActive(true);
                _cameraToShake.ShakeCamera();
            }
        }
    }
}
