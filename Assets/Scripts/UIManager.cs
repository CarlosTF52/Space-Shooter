using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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

    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Image _thrustersbar;


    [SerializeField]
    private float _thrusterMaxAmount, _thrusterAmount;

    [SerializeField]
    private bool _refreshThrusterAmount;

    [SerializeField]
    private Text _enemyQtyText;



    // Start is called before the first frame update
    void Start()
    {
     
        //assign text component to handle
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _scoreText.text =  "Score: " + 0.0f ;
        _ammoText.text = "Ammo: " + 15;
        _enemyQtyText.text = "Enemies Left: ";
      
        
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

    public void UpdateAmmo(int playerammo)
    {
        _ammoText.text = "Ammo: " + playerammo.ToString();
    }

    public void UpdateEnemyQuantity(int enemyQty)
    {
        _enemyQtyText.text = "Enemies Left: " + enemyQty.ToString();
    }

    public void UpdateThrusters(float playerthrusters)
    {
      

        _thrusterAmount -= playerthrusters * Time.deltaTime;

        if (_thrusterAmount < 0 && _refreshThrusterAmount == false)
        {
            _thrusterAmount = 0;
            _refreshThrusterAmount = true;
            

        }
        

        if (_refreshThrusterAmount == true)
        {
            _thrusterAmount = 25f;
            _thrustersbar.fillAmount = _thrusterAmount;
            _refreshThrusterAmount = false;
           
        }

        _thrustersbar.fillAmount = _thrusterAmount / _thrusterMaxAmount;

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
