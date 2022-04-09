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

    [SerializeField] private float scaleTweenTimeBetweenCars;

    private void Awake() => Instance = this;
    private void Start() => _carGridCount = CarPathHelper.Instance.carGridHolders.SelectMany(x => x._carGrids).Count();

    public void PlacedAtGrid()
    {
        _placedGridCount++;
        if (_placedGridCount == _carGridCount)
        {
            float tweensDuration = 0;
            LerpManager.LoopWait<Car>(CarPathHelper.Instance.carGridHolders
            .SelectMany(x => x._carGrids.Select(y => y.MyCar))
            .OrderByDescending(x => x.transform.position.z)
            .ToArray(),
            scaleTweenTimeBetweenCars, x => x.PlayScaleTween(out tweensDuration));
            DOVirtual.DelayedCall(tweensDuration + (scaleTweenTimeBetweenCars * _carGridCount), () => GameManager.Success());
        }
    }
}
