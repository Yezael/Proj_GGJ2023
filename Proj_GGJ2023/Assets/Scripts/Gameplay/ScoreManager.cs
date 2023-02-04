using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance => instance;

    public int currScore;

    public UITxt_CorrectAnswer txtScorePrefab;
    public UITxt_CorrectAnswer txtComboPrefab;
    public Pool<UITxt_CorrectAnswer> comboTxtsPool;
    public Pool<UITxt_CorrectAnswer> scoreTxtPool;
    public RectTransform scoretxtsParents;
    public RectTransform combostxtsParents;


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
        scoreTxtPool = new Pool<UITxt_CorrectAnswer>(scoretxtsParents, txtScorePrefab, 30);

        comboTxtsPool = new Pool<UITxt_CorrectAnswer>(combostxtsParents, txtComboPrefab, 30);
    }

    public void AddScore(int scoreToAdd)
    {
        currScore += scoreToAdd;
        var newItem = scoreTxtPool.GetItem();
        newItem.Init(scoreToAdd, scoreTxtPool);
        newItem.transform.localPosition = Vector3.zero;
        if(scoreToAdd > 10)
        {
            var comboItem = comboTxtsPool.GetItem();
            comboItem.transform.localPosition = Vector3.zero;
            comboItem.Init(comboTxtsPool);
        }
    }
}
