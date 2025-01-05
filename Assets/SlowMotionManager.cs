using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionManager : MonoBehaviour
{
    public void SlowForDuration(float strangth , float duration)
    {
        Time.timeScale = strangth;
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, duration).SetEase(Ease.InCubic);
    }
}
