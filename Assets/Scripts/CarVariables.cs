using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Variables/CarVariables")]
public class CarVariables : ScriptableObject
{
    [Tooltip("To be multiplied with a normalized paths length.")] public float baseMoveDuration = 1.25f;
    public Ease moveEase = Ease.InOutSine;
}
