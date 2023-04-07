using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ScoreInput : MonoBehaviour
{
    //Private Values
    int currentLocalPlayerScore;
    string currentLocalPlayerName;

    //UI
    [Header("UI - Score Fields")]
    [SerializeField] TMP_InputField uiNameField;
    [SerializeField] TextMeshProUGUI uiScoreText;
    [SerializeField] TextMeshProUGUI uiScoreText2;

    [Header("Score Reward Amounts")]
    [SerializeField] int ballHitReward;

    public bool gameEnd;

    public void BallHitRewardIncrement()
    {
        if (!gameEnd)
        {
            Debug.Log("here");
            currentLocalPlayerScore += ballHitReward;
            uiScoreText.text = currentLocalPlayerScore.ToString("n0");
            uiScoreText2.text = currentLocalPlayerScore.ToString("n0");
        }
    }

    public void GameEndBool(bool state)
    {
        gameEnd = state;
    }

    public void ResetScore()
    {
        currentLocalPlayerScore = 0;
        uiScoreText.text = currentLocalPlayerScore.ToString("n0");
        uiScoreText2.text = currentLocalPlayerScore.ToString("n0");
    }

    public void GetScore()
    {
        uiScoreText2.text = currentLocalPlayerScore.ToString("n0");
        currentLocalPlayerName = uiNameField.text;
    }

    public void PassScoreToLeaderboard()
    {
        GetScore();
        gameObject.SendMessage("RetrieveLocalScoreValue", currentLocalPlayerScore);
        gameObject.SendMessage("RetrieveLocalNameValue", currentLocalPlayerName);
        gameObject.SendMessage("TrySubmitScore");
    }
}
