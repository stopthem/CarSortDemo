using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathCreation;
using System;
using System.Linq;
using CanTemplate.Extensions;

public class TeamCarsHolder : MonoBehaviour
{
    [SerializeField] private Gate gate;
    [SerializeField] private GateButton gateButton;
    [SerializeField] private float coolDown;
    [SerializeField] private int teamIndex;

    [Header("Car Spawn and Reorder")] [SerializeField]
    private float waitBeforeMovingCar;

    [SerializeField] private Transform firstCarSpawnPoint;
    [SerializeField] private float zDisBetweenCars;
    [SerializeField] private float reOrderTimeBetweenCars;
    [SerializeField] private Ease reOrderEase;

    private Color _teamColor;
    private int _carCount;
    private Stack<Car> _cars = new Stack<Car>();
    public List<CarGridHolder> _prioCarGridHolders = new List<CarGridHolder>();

    private void Start()
    {
        _teamColor = GameManager.currentLevelInfo.teams[teamIndex].color;
        _carCount = GameManager.currentLevelInfo.teams[teamIndex].carCount;

        gateButton.Init(teamIndex, coolDown, _teamColor, this);
        gate.SetColors(_teamColor);

        SpawnCars();
    }

    private void SpawnCars()
    {
        Vector3 spawnPoint = firstCarSpawnPoint.localPosition;
        for (int i = 0; i < _carCount; i++)
        {
            var car = Instantiate(GameManager.currentLevelInfo.availableCars[UnityEngine.Random.Range(0, GameManager.currentLevelInfo.availableCars.Length)], Vector3.zero, Quaternion.identity, transform);
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
        if (_cars.Count == 0) return;

        gate.Open(coolDown);

        DOVirtual.DelayedCall(waitBeforeMovingCar, () =>
        {
            Tuple<VertexPath, CarGrid> tuple = GetNextPath(_cars.Peek().transform.position, -1);
            if (tuple.Item1 == null) return;
            Car car = _cars.Pop();
            car.FollowPath(tuple);

            ReOrderCars();
        });
    }

    public Tuple<VertexPath, CarGrid> GetNextPath(Vector3 position, int i)
    {
        Tuple<VertexPath, CarGrid> tuple = CarPathHelper.Instance.GetPath(position, _prioCarGridHolders.ToArray());
        if (i + 1 == _prioCarGridHolders.Count - 1) return null;
        if (tuple.Item1 == null)
        {
            _prioCarGridHolders = CarPathHelper.Instance.GetMovableGridHoldersOrdered(transform.position);
            tuple = CarPathHelper.Instance.GetPath(position, _prioCarGridHolders.ToArray());
        }

        return tuple;
    }

    private void ReOrderCars()
    {
        LerpManager.LoopWait<Transform>(_cars.Select(x => x.transform).ToArray(), reOrderTimeBetweenCars,
            (Transform x, int i) => x.transform.DOMove(x.transform.position.WithZ(x.transform.position.z + zDisBetweenCars), coolDown).SetEase(reOrderEase));
    }
}