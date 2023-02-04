using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    private int currScore;

    public UITxt_CorrectAnswer txtScorePrefab;
    public Pool<UITxt_CorrectAnswer> textsPool;
    public RectTransform txtsParents;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            GameObject.DestroyImmediate(instance);
        }
        else if (instance == null)
        {
            instance = this;
        }
    }

    public void Init()
    {
        currScore = 0;
        textsPool = new Pool<UITxt_CorrectAnswer>(txtsParents, txtScorePrefab, 30);
    }

    public void AddScore(int scoreToAdd)
    {
        Debug.Log("Adding score");
        currScore += scoreToAdd;
        var newItem = textsPool.GetItem();
        newItem.Init(scoreToAdd, textsPool);
        newItem.transform.localPosition = Vector3.zero;
    }

}
