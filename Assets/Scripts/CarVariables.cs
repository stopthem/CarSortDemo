using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(menuName = "Variables/CarVariables")]
public class CarVariables : ScriptableObject
{
    public float moveDuration;
    public Ease moveEase = Ease.InOutSine;
}
