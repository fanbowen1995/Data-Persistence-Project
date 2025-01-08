using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScoreManager : MonoBehaviour
{
    static private ScoreManager instance = null;
    public TMP_InputField inputField;
    private BestScore bestScore;
    private string path;
    public string curPlayerName;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            path = Application.persistentDataPath + "/bestScore.json";
            ReadBestScoreFromFile();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    static public ScoreManager Instance() { return instance; }

    public BestScore GetBestScore()
    {
        return bestScore;
    }

    public void SaveScore(BestScore score)
    {
        bestScore = score;
        SaveBestScoreToFile();
    }

    private void SaveBestScoreToFile()
    {
        string json = JsonUtility.ToJson(bestScore);
        File.WriteAllText(path, json);
    }

    private void ReadBestScoreFromFile()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            bestScore = JsonUtility.FromJson<BestScore>(json);
        }
        else
        {
            bestScore = new BestScore();
            bestScore.name = "";
            bestScore.score = 0;
        }
    }


    public void StartGame()
    {
        curPlayerName = inputField.text;
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        SaveBestScoreToFile();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

[Serializable]
public class BestScore
{
    public string name;
    public int score;
}