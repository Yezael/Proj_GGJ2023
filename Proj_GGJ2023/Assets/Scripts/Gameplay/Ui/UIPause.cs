using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPause : MonoBehaviour
{
    public Button btn_continue;
    public Button btn_Restart;
    public Button btn_Quit;

    private void Start()
    {
        btn_continue.onClick.AddListener(Continue);
        btn_Restart.onClick.AddListener(Restart);
        btn_Quit.onClick.AddListener(Quit);
    }

    private void Continue()
    {
        GameManager.Instance.gameState = GameState.Playing;
        gameObject.SetActive(false);
    }

    private void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void Quit()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
