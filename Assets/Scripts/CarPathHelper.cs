using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using PathCreation;

public class CarPathHelper : MonoBehaviour
{
    [SerializeField] private float pathYOffset;
    public static CarPathHelper Instance;

    private PathCreator _pathCreator;

    private CarGridHolder[] _carGridHolders;

    private void Awake()
    {
        Instance = this;
        _carGridHolders = GetComponentsInChildren<CarGridHolder>();
        _pathCreator = GetComponent<PathCreator>();
    }

    public List<CarGridHolder> GetPriotarizedGridHolders(Vector3 pos)
     => _carGridHolders
     .OrderBy(y => Mathf.Abs(pos.x - y._carGrids.Average(k => k.transform.position.x)))
     .ThenBy(y => Mathf.Abs(pos.z - y._carGrids.Average(k => k.transform.position.z)))
     .ToList();

    public Tuple<VertexPath, CarGrid> GetPath(Vector3 from, CarGridHolder to)
    {
        var path = new NavMeshPath();
        CarGrid car = to.GetAvailableGrid();
        if (car)
        {
            if (!NavMesh.CalculatePath(from, car.transform.position, NavMesh.AllAreas, path)) return new Tuple<VertexPath, CarGrid>(null, null);

            for (int i = 0; i < path.corners.Length; i++)
                path.corners[i].y += pathYOffset;

            _pathCreator.bezierPath = new BezierPath(path.corners);
            _pathCreator.bezierPath.GlobalNormalsAngle = 90;

            // bulamadığım bir nedenden dolayı path düz yolu çizerken sorun yaşıyor, böyle bir yöntem ile düzeltiyorum
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Aligned;
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;

            return Tuple.Create(_pathCreator.path, car);
        }
        else return new Tuple<VertexPath, CarGrid>(null, null);
    }
}
