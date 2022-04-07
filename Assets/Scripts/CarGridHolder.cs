using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class CarGridHolder : MonoBehaviour
{
    public CarGrid[] _carGrids { get; private set; }
    public bool deadEnd;

    private void Awake()
    {
        _carGrids = GetComponentsInChildren<CarGrid>();
    }

    public CarGrid GetAvailableGrid()
    {
        CarGrid carToReturn = null;
        foreach (var carGrid in _carGrids)
        {
            if (!carGrid.IsEmpty)
                return carToReturn;
            else carToReturn = carGrid;
        }
        return carToReturn;
    }
}
