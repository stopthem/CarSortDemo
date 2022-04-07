using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public static class TweenExtensions
{
    public static Sequence DoMoveRotate(this Transform t, Transform to, float duration, bool local = false, Ease ease = Ease.InOutQuad)
    {
        Vector3 toPos = local ? to.localPosition : to.position;
        Vector3 toRotation = local ? to.localRotation.FixReturnEuler() : to.rotation.FixReturnEuler();

        Sequence seq = DOTween.Sequence();
        seq.SetTarget(t);
        return seq
        .Append(local ? t.DOLocalMove(toPos, duration).SetEase(ease) : t.DOMove(toPos, duration).SetEase(ease))
        .Join(local ? t.DOLocalRotate(toRotation, duration).SetEase(ease) : t.DORotate(toRotation, duration).SetEase(ease));
    }
}
