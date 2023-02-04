using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "GGJ/New Enemy Data")]
public class EnemyData : ScriptableObject
{
    public float initialDelayToAttack;

    public string attackingWord;

    public string[] acceptedDefenseWords;
}
