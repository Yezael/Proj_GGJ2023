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

    [SerializeField] Renderer hightlightObj;

    public bool alreadyDead = false;

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
        hightlightObj.material.SetFloat("_IntensityDist", 0);
        alreadyDead = false;

    }

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        if (alreadyDead) return;
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
        if(DieRoutineRef != null)
        {
            StopCoroutine(DieRoutineRef);
        }
        alreadyDead = true;
        DieRoutineRef = DieRoutine();
        StartCoroutine(DieRoutineRef);
    }

    private IEnumerator DieRoutineRef;
    
    public IEnumerator DieRoutine()
    {
        var currdistortion = 0f;
        while (currdistortion < 2.3f)
        {
            currdistortion += 3 * Time.deltaTime;
            hightlightObj.material.SetFloat("_IntensityDist", currdistortion);
            yield return null;
        }
        ownerSpawner.RecycleEnemy(this);
    }
    

    public void SetHightlight(bool hightlight)
    {
        if (hightlight)
            hightlightObj.material.SetFloat("_Outline", 0.03f);
        else
            hightlightObj.material.SetFloat("_Outline", 0f);

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
