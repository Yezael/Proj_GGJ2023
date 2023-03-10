using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int hitsPerLife;
    private int currHitsUntilLifeChange;

    public int initialLifes = 5;
    private int currLifes = 5;
    public int CurrLifes { get => currLifes; 
        set
        {
            SetHealth(value);
        }
    }


    public Animator anims;

    public BrainState[] brainStates;
 

    public void Awake()
    {
        currLifes = 2;
        currHitsUntilLifeChange = hitsPerLife;
    }

    public void ReceiveDamage()
    {
        if (GameManager.Instance.gameState == GameState.RoundEnded) return;



        currHitsUntilLifeChange = hitsPerLife;

        CurrLifes -= 1;
        anims.SetTrigger("LevelDown");

        if (CurrLifes < 0)
        {
            anims.SetTrigger("Die");
            GameManager.Instance.OnPlayerDied();
        }
    }

    public void ReceiveHealth()
    {
        if (CurrLifes >= 4) return;
        currHitsUntilLifeChange = 0;
        CurrLifes += 1;
        anims.SetTrigger("LevelUp");
    }

    public void SetHealth(int newHealth)
    {
        previousLife = currLifes;
        currLifes = newHealth;
        anims.SetInteger("Level", CurrLifes);
    }

    int previousLife = -1;

    public void OnEndedLevelUpAnim()
    {
        
       
    }
}

[Serializable]
public class BrainState
{
    public int level;
    public GameObject[] objsToActive;
}
