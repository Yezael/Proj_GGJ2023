using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemySpawner ownerSpawner;
    private float initialDelayToAttack;
    private string attackingWord;
    [NonSerialized]
    public string[] acceptedDefenseWords;
    private Vector3 originalPos;
    private float timer;
    private float currAttackingTime;

    public TMP_Text textCompAttackingWord;
    public AnimationCurve movementAnim;
    [SerializeField] Renderer renderer;

    public void Init(EnemyData data, EnemySpawner spawner)
    {
        timer = 0;
        initialDelayToAttack = data.initialDelayToAttack;
        attackingWord = data.attackingWord;
        textCompAttackingWord.SetText(attackingWord);
        acceptedDefenseWords = new string[data.acceptedDefenseWords.Length];
        for (int i = 0; i < acceptedDefenseWords.Length; i++)
        {
            acceptedDefenseWords[i] = data.acceptedDefenseWords[i];
        }
        ownerSpawner = spawner;
        currAttackingTime = GetNewAttackingTime();
        originalPos = transform.position;
        transform.right = (GameManager.Instance.player.transform.position - transform.position).normalized;
        textCompAttackingWord.transform.right = Vector3.right;
    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        timer += Time.deltaTime;
        //Tutorial
        if(timer >= (currAttackingTime / 3f) && (!GameManager.Instance.tutorialDone || !GameManager.Instance.tutorial2ndWord))
        {
            GameManager.Instance.StartTutorialPause(this);
        }
        //
        if(timer >= currAttackingTime)
        {
            GameManager.Instance.player.ReceiveDamage();
            Die();
        }
        var progress = Mathf.InverseLerp(0, currAttackingTime, timer);
        var anim_progress = movementAnim.Evaluate(progress);
        var end_pos = Vector3.zero;
        transform.position = Vector3.Lerp(originalPos, end_pos, anim_progress);
    }

    public float GetNewAttackingTime()
    {
        return initialDelayToAttack * GameManager.Instance.GetCurrProgressMultiplier();
    }

    public void Die()
    {
        ownerSpawner.RecycleEnemy(this);
    }

    public void SetHightlight(bool hightlight)
    {
        //hightlightObj.SetActive(hightlight);
        if(hightlight)
            renderer.material.SetFloat("_Outline", .03f);
        else
            renderer.material.SetFloat("_Outline", 0f);


    }

    public bool CheckPartialMatch(string defensiveWord)
    {
        for (int i = 0; i < acceptedDefenseWords.Length; i++)
        {
            var curr = acceptedDefenseWords[i];
            if (StringComparer.CompareParcialText(defensiveWord, curr))
            {
                return true;
            }
        }
        return false;
    }
    public bool CanReceiveDamage(string defensiveWord)
    {
        for (int i = 0; i < acceptedDefenseWords.Length; i++)
        {
            var curr = acceptedDefenseWords[i];
            if (StringComparer.CompareTexts(curr, defensiveWord))
            {
                Debug.Log("Correcly killed root");
                return true;
            }
        }
        return false;
    }

    public void Kill()
    {
        Die();
    }
}
