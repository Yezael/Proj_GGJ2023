using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int initialLifes = 5;
    public int currLifes = 5;

    public void Awake()
    {
        currLifes = initialLifes;
    }

    public void ReceiveDamage()
    {
        currLifes -= 1;
        Debug.Log("Player received damage, currLifes: " + currLifes);
        if(currLifes <= 0)
        {
            Debug.Log("Player died");
            GameManager.Instance.OnPlayerDied();
        }
    }

    public void ReceiveHealth()
    {
        currLifes += 1;
    }
}
