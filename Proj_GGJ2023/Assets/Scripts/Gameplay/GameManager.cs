using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameProgresionData LevelData;

    private static GameManager instance;
    public static GameManager Instance => instance;

    public PlayerController player;
    public EnemySpawner enemySpawner;

    public GameState gameState = GameState.StartingRound;

    public UIAfterAction afterAction;
    public UIProgressBar inGameProgressBar;

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
    }

    private void Start()
    {
        enemySpawner.Init(LevelData.enemiesSpawnerData);
        ScoreManager.Instance.Init();
        afterAction.Init();
        gameState = GameState.Playing;
        inGameProgressBar.Init();
    }

    private void Update()
    {
        if (gameState == GameState.Playing)
        {
            timer += Time.deltaTime;
            inGameProgressBar.SetProgress(GetCurrProgress());
            if (timer > roundTime)
            {
                EndRound(true);
            }
        }
    }

    public void PlayerSendedDefenseWord(string defensiveWord)
    {
        var currEnemies = enemySpawner.activeEnemies;
        for (int i = 0; i < currEnemies.Count; i++)
        {
            var currEnemy = currEnemies[i];
            var succed = currEnemy.ReceiveDefenseWord(defensiveWord);
            if (succed)
            {
                ScoreManager.Instance.AddScore(10);
            }
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
