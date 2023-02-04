using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private Pool<EnemyBehaviour> ownerPool;
    private float initialDelayToAttack;
    private string attackingWord;
    private string[] acceptedDefenseWords;

    private float timer;
    private float currAttackingTime;

    public void Init(EnemyData data, Pool<EnemyBehaviour> pool)
    {
        timer = 0;
        initialDelayToAttack = data.initialDelayToAttack;
        attackingWord = data.attackingWord;
        acceptedDefenseWords = new string[data.acceptedDefenseWords.Length];
        for (int i = 0; i < acceptedDefenseWords.Length; i++)
        {
            acceptedDefenseWords[i] = data.acceptedDefenseWords[i];
        }
        ownerPool = pool;
        currAttackingTime = GetNewAttackingTime();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= currAttackingTime)
        {
            GameManager.Instance.player.ReceiveDamage();
            ownerPool.RecycleItem(this);
        }
    }

    public float GetNewAttackingTime()
    {
        return initialDelayToAttack * GameManager.Instance.GetCurrProgressMultiplier();
    }
}
