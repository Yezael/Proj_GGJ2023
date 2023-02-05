using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseAngelBehaviour : MonoBehaviour
{

    private EnemyBehaviour currTarget;
    private Vector3 targetPos;
    private Vector3 initialPosition;

    private Vector3 velocity;

    public float moveTime = 1;
    public float attackingTriggerZone = 5;

    private float attackingTriggerZoneSqr;

    public AngelStates currState;


    public void Start()
    {
        targetPos = transform.position;
    }

    public void StartDefendingFrom(EnemyBehaviour enemy)
    {
        targetPos = enemy.transform.position;
        initialPosition = transform.position;
        attackingTriggerZoneSqr = attackingTriggerZone * attackingTriggerZone;
        currTarget = enemy;
    }

    public void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing) return;
        if (currTarget == null || !currTarget.gameObject.activeInHierarchy)
        {
            currTarget = null;
            SetState(AngelStates.Idle);
            return;
        }
        transform.position = Vector3.SmoothDamp(initialPosition, targetPos, ref velocity, moveTime);
        var distVector = targetPos - initialPosition;
        var sqrDist = Vector3.SqrMagnitude(distVector);
        if(sqrDist <= attackingTriggerZoneSqr)
        {
            SetState(AngelStates.Attacking);
        }
        else 
        {
            SetState(AngelStates.Walking);
        }
    }

    public void SetState(AngelStates newState)
    {
        if (newState == currState) return;
        currState = newState;
        //TODO anims here
    }

}

public enum AngelStates
{
    Idle,
    Walking,
    Attacking
}