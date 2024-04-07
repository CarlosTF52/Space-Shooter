using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _livesImage; 

    // Start is called before the first frame update
    void Start()
    {
        //assign text component to handle
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _scoreText.text =  "Score: " + 0.0f ;
    }

    
    public void GameOver()
    {
        _gameOverText.gameObject.SetActive(true);    
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    // Update is called once per frame
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _liveSprites[currentLives];
    }

    IEnumerator GameOverFlicker()
    {
        while(true)
        {
            _gameOverText.text = "Game Over";
            _restartText.text = "Press 'R' Key to Restart The Level";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            _restartText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

   

}
