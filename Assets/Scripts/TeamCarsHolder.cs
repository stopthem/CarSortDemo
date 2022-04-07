using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation;
using System;

public class TeamCarsHolder : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private GateButton gateButton;
    [SerializeField] private float coolDown;
    [SerializeField] private int teamIndex;

    [Header("Car Spawn and Reorder")]
    [SerializeField] private float waitBeforeMovingCar;
    [SerializeField] private Transform firstCarSpawnPoint;
    [SerializeField] private float zDisBetweenCars;
    [SerializeField] private float reOrderDuration;
    [SerializeField] private float reOrderTimeBetweenCars;
    [SerializeField] private Ease reOrderEase;

    private Color _teamColor;
    private int _carCount;
    private Stack<Car> _cars = new Stack<Car>();
    public List<CarGridHolder> _prioCarGridHolders = new List<CarGridHolder>();

    private void Start()
    {
        _teamColor = GameManager.currentlevelInfo.teams[teamIndex].color;
        _carCount = GameManager.currentlevelInfo.teams[teamIndex].carCount;
        _prioCarGridHolders = CarPathHelper.Instance.GetPriotarizedGridHolders(transform.position);

        gateButton.Init(teamIndex, coolDown, _teamColor, this);
        gate.SetColors(_teamColor);

        SpawnCars();
    }

    private void SpawnCars()
    {
        Vector3 spawnPoint = firstCarSpawnPoint.localPosition;
        for (int i = 0; i < _carCount; i++)
        {
            var car = Instantiate(GameManager.currentlevelInfo.availableCars[UnityEngine.Random.Range(0, GameManager.currentlevelInfo.availableCars.Length)], Vector3.zero, Quaternion.identity, transform);
            car.transform.localPosition = spawnPoint;
            spawnPoint.z -= zDisBetweenCars;
            Car carSc = car.GetComponent<Car>();
            carSc.Init(_teamColor, teamIndex, this);
            _cars.Push(carSc);
        }
        _cars = new Stack<Car>(_cars);
    }

    public void Open()
    {
        gate.Open(coolDown);
        DOVirtual.DelayedCall(waitBeforeMovingCar, () =>
        {
            Car car = _cars.Pop();
            car.FollowPath(GetNextPath(car.transform.position, -1));
        });
    }

    public Tuple<VertexPath, CarGrid> GetNextPath(Vector3 position, int i)
    {
        Tuple<VertexPath, CarGrid> tuple = CarPathHelper.Instance.GetPath(position, _prioCarGridHolders[i + 1]);
        if (i + 1 == _prioCarGridHolders.Count - 1) return null;
        return tuple;
    }
}
