using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{

    private GameManager _gameManager;

    private UIManager _uiManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager.GameOver();
        _uiManager.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
