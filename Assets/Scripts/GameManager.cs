using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver = false;

    [SerializeField] private Player _player;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            _isGameOver = false;
            _player.IsLocked = false;
            SceneManager.LoadScene("Game");
        }

        // two ways of reading player input that the Escape button has been pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public bool IsGameOver()
    {
        return _isGameOver;
    }

    public void GameOver()
    {
        Debug.Log("GameManager::GameOver() Called");
        _isGameOver = true;
    }
}
