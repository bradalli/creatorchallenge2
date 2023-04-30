using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class LeaderboardScores : MonoBehaviour
{
    //Private values
    [SerializeField] Dictionary<string, int> PlayerScoreDictionary = new Dictionary<string, int>();
    [System.Serializable]
    public struct PlayerScore
    {
        public int userID;
        public int score;
        public string name;
    }
    [SerializeField] PlayerScore[] leaderboardScoresArray;
    [SerializeField] Transform[] uiLeaderboardScorePanels;
    int localPlayerScore;
    string localPlayerName;

    //UI
    [SerializeField] Transform uiContentScores;

    private void Awake()
    {
        //Collect references to leaderboard score panels
        uiLeaderboardScorePanels = new Transform[uiContentScores.childCount];
        for(int i = 0; i < uiLeaderboardScorePanels.Length; i++)
        {
            uiLeaderboardScorePanels[i] = uiContentScores.GetChild(i);
        }
        RetrieveSavedLeaderboard();
        RefreshLeaderboard();
    }
    public void RetrieveLocalScoreValue(int score)
    {
        localPlayerScore = score;
    }
    public void RetrieveLocalNameValue(string name)
    {
        localPlayerName = name;
    }
    void DictionaryToArray()
    {
        leaderboardScoresArray = new PlayerScore[PlayerScoreDictionary.Count];
        for(int i = 0; i < PlayerScoreDictionary.Count; i++)
        {
            leaderboardScoresArray[i].name = PlayerScoreDictionary.ElementAt(i).Key;
            leaderboardScoresArray[i].score = PlayerScoreDictionary.ElementAt(i).Value;
        }
    }
    public void TrySubmitScore()
    {
        if(PlayerScoreDictionary.Count < uiLeaderboardScorePanels.Length)
        {
            AddNewScore(localPlayerName, localPlayerScore);
            RefreshLeaderboard();
        }

        else if(localPlayerScore >= PlayerScoreDictionary.Last().Value)
        {
            if (PlayerScoreDictionary.Count == uiLeaderboardScorePanels.Length)
            {
                RemoveLastScore();
            }

            AddNewScore(localPlayerName, localPlayerScore);
            RefreshLeaderboard();
        }
    }
    public void ResetLeaderboard()
    {
        PlayerPrefs.DeleteAll();
        PlayerScoreDictionary = new Dictionary<string, int>();
        DictionaryToArray();
        RefreshLeaderboard();
    }
    void RefreshLeaderboard()
    {
        DictionaryToArray();
        if (PlayerScoreDictionary.Count > 0) { ReorderLeaderboard(); }


        for (int i = 0; i < uiLeaderboardScorePanels.Length; i++)
        {
            //Set panel active depending on length of scores array
            uiLeaderboardScorePanels[i].gameObject.SetActive(i < leaderboardScoresArray.Length);

            if (i < leaderboardScoresArray.Length)
            {
                RefreshLeaderboardPanel(i, uiLeaderboardScorePanels[i], leaderboardScoresArray[i].name, leaderboardScoresArray[i].score);
            }
        }
    }
    void RetrieveSavedLeaderboard()
    {
        if (PlayerPrefs.HasKey("LeaderboardName_0"))
        {
            for(int i = 0; i < uiLeaderboardScorePanels.Length; i++)
            {
                if(PlayerPrefs.HasKey("LeaderboardName_" + i))
                {
                    string savedName = PlayerPrefs.GetString("LeaderboardName_" + i);
                    int savedScore = PlayerPrefs.GetInt("LeaderboardScore_" + i);
                    PlayerScoreDictionary.Add(savedName, savedScore);
                }

                else
                {
                    return;
                }
            }
        }
    }
    void RefreshLeaderboardPanel(int index, Transform panel, string name, int score)
    {
        panel.Find("Text_ScorePosition").GetComponent<TextMeshProUGUI>().text = (index + 1).ToString();
        panel.Find("Text_ScorePlayerName").GetComponent<TextMeshProUGUI>().text = name;
        panel.Find("Text_ScoreTotal").GetComponent<TextMeshProUGUI>().text = score.ToString("n0");

    }
    void ReorderLeaderboard()
    {
        leaderboardScoresArray = leaderboardScoresArray.OrderByDescending(x => x.score).ToArray();
    }
    void AddNewScore(string name, int score)
    {
        if (PlayerScoreDictionary.ContainsKey(name))
        {
            if(score > PlayerScoreDictionary[name])
            {
                PlayerScoreDictionary[name] = score;
            }

            else
            {
                Debug.Log("Score is not greater than previous best");
            }
        }

        else 
        {
            PlayerScoreDictionary.Add(name, score);
        }
        
    }
    void RemoveLastScore()
    {
        PlayerScoreDictionary.Remove(leaderboardScoresArray.Last().name);
    }
    private void OnApplicationQuit()
    {
        for (int i = 0; i < leaderboardScoresArray.Length; i++)
        {
            PlayerPrefs.SetString("LeaderboardName_" + i, leaderboardScoresArray[i].name);
            PlayerPrefs.SetInt("LeaderboardScore_" + i, leaderboardScoresArray[i].score);
        }
    }
}
