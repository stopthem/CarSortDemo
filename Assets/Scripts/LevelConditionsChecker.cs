using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class LevelConditionsChecker : MonoBehaviour
{
    public static LevelConditionsChecker Instance;

    private int _carGridCount;
    private int _placedGridCount;

    [Header("Car Win Tween")]
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private float scaleTweenDuration;
    [SerializeField] private float scaleTweenTimeBetweenCars;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _carGridCount = CarPathHelper.Instance._carGridHolders.SelectMany(x => x._carGrids).Count();
    }

    public void PlacedAtGrid()
    {
        _placedGridCount++;
        if (_placedGridCount == _carGridCount)
        {
            LerpManager.LoopWait<Transform>(CarPathHelper.Instance._carGridHolders
            .SelectMany(x => x._carGrids.Select(y => y.MyCar.transform))
            .OrderByDescending(x => x.transform.position.z)
            .ToArray(),
            scaleTweenTimeBetweenCars, x => x.DOScale(x.transform.localScale * scaleMultiplier, scaleTweenDuration).SetEase(LerpManager.PresetToAnimationCurve(PresetAnimationCurves.BOUNCE)));

            DOVirtual.DelayedCall(scaleTweenDuration + (scaleTweenTimeBetweenCars * _carGridCount), () => GameManager.Success());
        }
    }
}
