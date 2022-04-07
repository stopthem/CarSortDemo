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
    private int _gridHolderCount = -1;

    public void Init(Color color, int teamIndex, TeamCarsHolder teamCarsHolder)
    {
        meshRenderer.material.color = color;
        this.teamIndex = teamIndex;
        _teamCarsHolder = teamCarsHolder;
    }

    public void FollowPath(Tuple<VertexPath, CarGrid> tuple)
    {
        if (tuple.Item1 == null || tuple.Item2 == null) return;
        DOTween.Sequence()
       .Append(DOTween.To(x =>
       {
           transform.position = tuple.Item1.GetPointAtTime(x);
           transform.rotation = tuple.Item1.GetRotation(x);
       }, 0, .99f, carVariables.moveDuration))
       .AppendCallback(() =>
       {
           _currentGrid = tuple.Item2;
           _gridHolderCount++;

           Tuple<VertexPath, CarGrid> toTuple = _teamCarsHolder.GetNextPath(transform.position, _gridHolderCount);

           if ((toTuple != null && toTuple.Item2 != null) && !_currentGrid.MyGridHolder.deadEnd) FollowPath(toTuple);
           else _currentGrid.Placed(teamIndex);
       });
    }
}
