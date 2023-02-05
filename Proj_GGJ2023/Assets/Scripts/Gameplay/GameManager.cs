using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    public GameProgresionData LevelData;

    private static GameManager instance;
    public static GameManager Instance => instance;

    public PlayerController player;
    public EnemySpawner enemySpawner;
    public DefenseAngelBehaviour angelB;
    public InputsManager inputs;

    public GameState gameState = GameState.StartingRound;

    public GameObject introUiPanel;
    public GameObject gameplayUiPanel;
    public UIAfterAction afterAction;
    public UIProgressBar inGameProgressBar;

    public TMP_Text startRoundTxt;

    public float startRoundTime = 3;

    //Progression
    private float timer;
    private float initialProgression;
    private AnimationCurve progressionChangeRate;
    private float roundTime;

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
        gameState = GameState.StartingRound;
        initialProgression = LevelData.initialAttackTimeMultiplier;
        progressionChangeRate = LevelData.progressionChangeRate;
        roundTime = LevelData.roundTotalTime;
        gameplayUiPanel.SetActive(false);
    }

    private void Start()
    {
        enemySpawner.Init(LevelData.enemiesSpawnerData);
        ScoreManager.Instance.Init();
        afterAction.Init();
        gameState = GameState.StartingRound;
        inGameProgressBar.Init();
        timer = startRoundTime;
        startRoundTxt.gameObject.SetActive(true);
        introUiPanel.SetActive(true);
    }

    private void Update()
    {
        if (gameState == GameState.StartingRound)
        {
            timer -= Time.deltaTime;
            var label = (((int)timer) + 1).ToString();
            startRoundTxt.SetText(label);
            if(timer <= 0)
            {
                StartGame();
            }
        }
        else if (gameState == GameState.Playing)
        {
            timer += Time.deltaTime;
            inGameProgressBar.SetProgress(GetCurrProgress());
            if (timer > roundTime)
            {
                EndRound(true);
            }
            for (int i = 0; i < enemiesBeingTargeted.Count; i++)
            {
                enemiesBeingTargeted[i].SetHightlight(false);
            }
            CheckPlayerParcialAnswer(enemiesBeingTargeted);
            for (int i = 0; i < enemiesBeingTargeted.Count; i++)
            {
                enemiesBeingTargeted[i].SetHightlight(true);

            }
        }
    }

    private void StartGame()
    {
        startRoundTxt.gameObject.SetActive(false);
        introUiPanel.SetActive(false);
        gameplayUiPanel.SetActive(true);
        enemySpawner.OnGameStart();
        gameState = GameState.Playing;
    }

    List<EnemyBehaviour> enemiesBeingTargeted = new List<EnemyBehaviour>(30);


    public void CheckPlayerParcialAnswer(List<EnemyBehaviour> posibleEnemies)
    {
        posibleEnemies.Clear();
        var currDefensiveWord = inputs.userInput.text;
        if (string.IsNullOrEmpty(currDefensiveWord)) return;
        var currEnemies = enemySpawner.activeEnemies;
        for (int i = 0; i < currEnemies.Count; i++)
        {
            var curr = currEnemies[i];
            var targetting = curr.CheckPartialMatch(currDefensiveWord);
            if (targetting)
            {
                angelB.StartDefendingFrom(curr);
                posibleEnemies.Add(curr);
            }
        }
    }

    public void PlayerSendedDefenseWord(string defensiveWord)
    {
        var currEnemies = enemySpawner.activeEnemies;
        List<EnemyBehaviour> enemiesDamaged = new List<EnemyBehaviour>();
        for (int i = 0; i < currEnemies.Count; i++)
        {
            var currEnemy = currEnemies[i];
            var succed = currEnemy.CanReceiveDamage(defensiveWord);
            if (succed)
            {
                enemiesDamaged.Add(currEnemy);
            }
        }
        if (enemiesDamaged.Count > 0)
        {
            ScoreManager.Instance.AddScore(10 * enemiesDamaged.Count);
        }
        for (int i = 0; i < enemiesDamaged.Count; i++)
        {
            enemiesDamaged[i].Kill();
        }
    }

    public void OnPlayerDied()
    {
        EndRound(false);
    }

    public void EndRound(bool win)
    {
        gameState = GameState.RoundEnded;
        if (win)
        {
            Debug.Log("Round ended! you win");
        }
        else
        {
            Debug.Log("Round ended! you lose");
        }
        afterAction.GameOver(win);
    }

    public float GetCurrProgress()
    {
        return Mathf.InverseLerp(0, roundTime, timer);
    }

    public float GetCurrProgressMultiplier()
    {
        var levelProgress = Mathf.InverseLerp(0, roundTime, timer);
        var progressionMultiplier = progressionChangeRate.Evaluate(levelProgress);
        var offsetedProgression = Mathf.Lerp(0, initialProgression, progressionMultiplier);
        return offsetedProgression;
    }
}

public enum GameState
{
    StartingRound,
    Playing,
    Paused,
    RoundEnded,
}
