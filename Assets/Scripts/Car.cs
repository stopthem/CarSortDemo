using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using DG.Tweening;
using System;

public class Car : MonoBehaviour
{
    [SerializeField] private CarVariables carVariables;
    [SerializeField] private MeshRenderer meshRenderer;

    private TeamCarsHolder _teamCarsHolder;
    public int teamIndex { get; private set; }

    private CarGrid _currentGrid;

    public void Init(Color color, int teamIndex, TeamCarsHolder teamCarsHolder)
    {
        meshRenderer.material.color = color;
        this.teamIndex = teamIndex;
        _teamCarsHolder = teamCarsHolder;
    }

    public void FollowPath(Tuple<VertexPath, CarGrid> tuple)
    {
        if (tuple.Item1 == null || tuple.Item2 == null) return;
        float moveDuration = carVariables.baseMoveDuration * (carVariables.maxPathLengthToCalculateSpeed / (carVariables.maxPathLengthToCalculateSpeed - tuple.Item1.length));
        DOTween.Sequence()
        .Append(DOTween.To(x =>
        {
            transform.position = tuple.Item1.GetPointAtTime(x);
            transform.rotation = tuple.Item1.GetRotation(x);
        }, 0, .99f, moveDuration).SetEase(carVariables.moveEase))
        .AppendCallback(() =>
        {
            _currentGrid = tuple.Item2;
            _currentGrid.Placed(this, teamIndex);
        });
    }

    public void PlayScaleTween(out float duration)
    {
        var tween = transform.DOScale(transform.localScale * carVariables.scaleMultiplier, carVariables.scaleTweenDuration / 2)
        .SetLoops(2, LoopType.Yoyo)
        .SetEase(carVariables.scaleEase);

        duration = tween.Duration();
    }
}
