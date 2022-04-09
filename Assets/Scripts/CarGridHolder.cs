using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class CarGridHolder : MonoBehaviour
{
    public CarGrid[] _carGrids { get; private set; }
    public bool deadEnd;
    public bool canBlockRoad;

    private void Awake()
    {
        _carGrids = GetComponentsInChildren<CarGrid>();
    }

    public Tuple<CarGrid, bool> GetAvailableGrid()
    {
        CarGrid carToReturn = null;
        foreach (var carGrid in _carGrids)
        {
            if (!carGrid.IsEmpty)
                return Tuple.Create(carToReturn, false);
            else carToReturn = carGrid;
        }
        return Tuple.Create(carToReturn, true);
    }
}
