using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DificultySetter : MonoBehaviour
{
    public GameProgresionData dataToSet;
    public Slider slider;

    public float minValue;
    public float maxValue;

    public void Awake()
    {
        slider.onValueChanged.AddListener( 
            (x) => {
                var value = Mathf.Lerp(maxValue, minValue, x);
                dataToSet.initialAttackTimeMultiplier = value;
        });
        var currValue = slider.value;
        currValue = Mathf.Lerp(maxValue, minValue, currValue);
        dataToSet.initialAttackTimeMultiplier = currValue;
    }

}
