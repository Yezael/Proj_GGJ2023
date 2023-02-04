using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIAfterAction : MonoBehaviour
{
    public RectTransform winPanel;
    public RectTransform losePanel;

    public UIProgressBar progressBar;

    public TMP_Text scoreTxt;

    public Button btn_restart;
    public Button btn_exit;

    public void Init()
    {
        progressBar.Init();
        btn_restart.onClick.AddListener(Restart);
        btn_exit.onClick.AddListener(Exit);
    }

    public void GameOver(bool won)
    {
        gameObject.SetActive(true);
        winPanel.gameObject.SetActive(won);
        losePanel.gameObject.SetActive(!won);
        if (!won)
        {
            progressBar.StartProgressAnim(GameManager.Instance.GetCurrProgress());
        }
        scoreTxt.SetText("Score: " + ScoreManager.Instance.currScore);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
