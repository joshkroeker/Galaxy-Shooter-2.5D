using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Image _livesImg;
    [SerializeField] Sprite[] _livesSprites;
    [SerializeField] private GameObject _gameOverTextObject;
    [SerializeField] private GameObject _pressRTORestartObj;
    [SerializeField] private Text _ammoText;
    //reference the slider
    [SerializeField] private Slider _thrusterSlider;

    [SerializeField] private float _flickerDelay = 0.5f;

    [SerializeField] private GameManager _gameManager;

    private void Start()
    { 
        if(_gameManager == null) { _gameManager = FindObjectOfType<GameManager>(); }

        _scoreText.text = "Score: 0";
        _gameOverTextObject.SetActive(false);
        _pressRTORestartObj.SetActive(false);
        _ammoText.text = "Ammo: 15";
        _thrusterSlider.value = 100;
    }

    public void AddScoreToText(int playerScore)
    {
        _scoreText.text = "";
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if(currentLives > 0 && currentLives < 4)
            _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives <= 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameOverTextObject.SetActive(true);
        StartCoroutine(StartFlickerEffectRoutine());

        _pressRTORestartObj.SetActive(true);
        _gameManager.GameOver();
    }

    private IEnumerator StartFlickerEffectRoutine()
    {
        while (true)
        {
            _gameOverTextObject.SetActive(false);
            yield return new WaitForSeconds(_flickerDelay);
            _gameOverTextObject.SetActive(true);
            yield return new WaitForSeconds(_flickerDelay);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        _ammoText.text = "Ammo: " + ammo.ToString();
    }

    public void UpdateThrusterFuel(float fuelAmount)
    {
        _thrusterSlider.value = fuelAmount;
    }
}
