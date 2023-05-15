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
    [SerializeField] private Slider _thrusterSlider;
    [SerializeField] private float _flickerDelay = 0.5f;
    [SerializeField] private GameManager _gameManager;

    //add a reference to the text object
    [SerializeField] private Text _waveIncomingText;

    private void Start()
    {
        if (_gameManager == null) { _gameManager = FindObjectOfType<GameManager>(); }

        _scoreText.text = "Score: 0";
        _gameOverTextObject.SetActive(false);
        _pressRTORestartObj.SetActive(false);
        _ammoText.text = "Ammo: 15 / 15";
        _thrusterSlider.value = 100;
        _waveIncomingText.gameObject.SetActive(false);
    }

    public void AddScoreToText(int playerScore)
    {
        _scoreText.text = "";
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives > 0 && currentLives < 4)
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

    private IEnumerator StartFlickerEffectRoutine(GameObject toFlicker, float duration)
    {
        while(duration > 0)
        {
            toFlicker.SetActive(false);
            yield return new WaitForSeconds(_flickerDelay);
            toFlicker.SetActive(true);
            duration--;
            yield return new WaitForSeconds(_flickerDelay);
        }

        toFlicker.SetActive(false);
    }

    public void ActivateWaveIncomingEffect()
    {
        StartCoroutine(StartFlickerEffectRoutine(_waveIncomingText.gameObject, 5f));
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        _ammoText.text = "Ammo: " + currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    public void UpdateThrusterFuel(float fuelAmount)
    {
        _thrusterSlider.value = fuelAmount;
    }
}
