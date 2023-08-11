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
    [SerializeField] private Text _currentAmmoText;
    [SerializeField] private Text _maxAmmoText;
    [SerializeField] private Slider _thrusterSlider;
    [SerializeField] private float _flickerDelay = 0.5f;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Slider _bossLeftCannonHPBar;
    [SerializeField] private Slider _bossRightCannonHPBar;
    [SerializeField] private Slider _bossMainHPBar;
    [SerializeField] private GameObject _winScreenObject;

    [SerializeField] private Text _roundStartingText;

    private Player _player;

    private void Start()
    {
        if (_gameManager == null) { _gameManager = FindObjectOfType<GameManager>(); }
        if(_player == null) { _player = FindObjectOfType<Player>(); }

        _scoreText.text = "Score: 0";
        _gameOverTextObject.SetActive(false);
        _pressRTORestartObj.SetActive(false);
        _thrusterSlider.value = 100;
        _roundStartingText.gameObject.SetActive(false);
        _bossMainHPBar.value = 100;
    }

    public void ShowMainShipHP()
    {
        _bossMainHPBar.gameObject.SetActive(true);

        if(_bossLeftCannonHPBar.IsActive() || _bossRightCannonHPBar.IsActive())
        {
            _bossRightCannonHPBar.gameObject.SetActive(false);
            _bossLeftCannonHPBar.gameObject.SetActive(false);
        }
    }

    public void ShowWinScreen()
    {
        _winScreenObject.SetActive(true);
        StartCoroutine(StartFlickerEffectSpecificRoutine(_winScreenObject));

        _pressRTORestartObj.SetActive(true);
 
        LockPlayer();
        _gameManager.GameOver();
    }

    private void LockPlayer()
    {
        _player.IsLocked = true;
    }
   
    public void UpdateMainShipHP(int hp)
    {
        _bossMainHPBar.value = hp;
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

        LockPlayer();

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
    private IEnumerator StartFlickerEffectSpecificRoutine(GameObject toFlicker)
    {
        while (true)
        {
            toFlicker.SetActive(false);
            yield return new WaitForSeconds(_flickerDelay);
            toFlicker.SetActive(true);
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

    public void ActivateNewRoundEffect(int currentRound)
    {
        _roundStartingText.text = "Round " + currentRound.ToString();

        StartCoroutine(StartFlickerEffectRoutine(_roundStartingText.gameObject, 5f));
    }

    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        _currentAmmoText.text = currentAmmo.ToString();
        _maxAmmoText.text = maxAmmo.ToString();
    }

    public void UpdateThrusterFuel(float fuelAmount)
    {
        _thrusterSlider.value = fuelAmount;
    }

    public void ShowBossHealthBars(int maxHP)
    {
        _bossRightCannonHPBar.gameObject.SetActive(true);
        _bossLeftCannonHPBar.gameObject.SetActive(true);

        _bossLeftCannonHPBar.maxValue = maxHP;
        _bossRightCannonHPBar.maxValue = maxHP;
    }

    public void UpdateRightCannonHP(int currentHPValue)
    {
        _bossRightCannonHPBar.value = currentHPValue;

        if(currentHPValue == 0)
        {
            _bossRightCannonHPBar.gameObject.SetActive(false);
        }
    }

    public void UpdateLeftCannonHP(int currentHPValue)
    {
        _bossLeftCannonHPBar.value = currentHPValue;

        if (currentHPValue == 0)
        {
            _bossLeftCannonHPBar.gameObject.SetActive(false);
        }
    }
}
