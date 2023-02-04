using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITxt_CorrectAnswer : MonoBehaviour
{
    private Pool<UITxt_CorrectAnswer> ownerPool;
    public TMP_Text text;
    public float lifeTime = 3;
    private float timer = 0;
    public void Init(int scoreAmount, Pool<UITxt_CorrectAnswer> pool)
    {
        timer = 0;
        text.SetText("+ " + scoreAmount);
        ownerPool = pool;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= lifeTime)
        {
            ownerPool.RecycleItem(this);
        }
    }

}
