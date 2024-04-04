using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionManager : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        //gameManager.currentGameState = GameManager.GameStateIndex.Intro;
    }

    private void OnEnable()
    {
        gameManager.currentGameState = GameManager.GameStateIndex.Intro;
        gameManager.ChangeGameState();
    }
}
