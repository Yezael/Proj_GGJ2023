using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBehaviour : MonoBehaviour
{
    private EnemySpawner ownerSpawner;
    private float initialDelayToAttack;
    private string attackingWord;
    private string[] acceptedDefenseWords;

    private float timer;
    private float currAttackingTime;

    public TMP_Text textCompAttackingWord;

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
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= currAttackingTime)
        {
            GameManager.Instance.player.ReceiveDamage();
            Die();
        }
    }

    public float GetNewAttackingTime()
    {
        return initialDelayToAttack * GameManager.Instance.GetCurrProgressMultiplier();
    }

    public void Die()
    {
        ownerSpawner.RecycleEnemy(this);
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
