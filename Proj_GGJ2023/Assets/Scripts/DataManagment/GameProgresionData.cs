using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameProgressionData", menuName = "GGJ/New Game Progression Data")]
public class GameProgresionData : ScriptableObject
{
    /// <summary>
    /// We multiply this value for each enemy attack time value so we can control how fast all enemies goes to the center
    /// </summary>
    public float initialAttackTimeMultiplier = 1;
    public float roundTotalTime;
    /// <summary>
    /// How will the inital time multiplier change over the game time, this is a value between 0 and 1 where values closer to 0 make the game harder (enemies attack faster)
    /// </summary>
    public AnimationCurve progressionChangeRate;

    public EnemiesSpawnerData enemiesSpawnerData;
}
