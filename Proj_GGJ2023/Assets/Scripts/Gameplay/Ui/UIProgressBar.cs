using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar : MonoBehaviour
{
    public RectTransform progressImg;
    public RectTransform progressBg;

    private float totalSpace;

    public float fillSpeed = 3;

    public IEnumerator progressAnimRoutine;

    public void Init()
    {
        totalSpace = progressBg.rect.width;

    }

    //Progress must be a value between 0 and 1
    public void StartProgressAnim(float progress)
    {

        if(progressAnimRoutine != null)
        {
            StopCoroutine(progressAnimRoutine);
        }
        progressAnimRoutine = ProgressAnim(progress);

        StartCoroutine(progressAnimRoutine);
    }

    public void SetProgress(float progress)
    {
        progressImg.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, progress * totalSpace);
    }

    public IEnumerator ProgressAnim(float desiredProgress)
    {
        var size = progressImg.sizeDelta;
        size.x = 0;
        progressImg.sizeDelta = size;

        while(progressImg.sizeDelta.x < totalSpace)
        {
            size.x = Mathf.Lerp(size.x, desiredProgress * totalSpace, fillSpeed * Time.deltaTime);
            progressImg.sizeDelta = size;
            yield return null;
        }

        size.x = desiredProgress * totalSpace;
    }

}
