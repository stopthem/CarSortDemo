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

    public CarGridHolder[] _carGridHolders { get; private set; }

    private void Awake()
    {
        Instance = this;
        _carGridHolders = GetComponentsInChildren<CarGridHolder>();
        _pathCreator = GetComponent<PathCreator>();
    }

    public List<CarGridHolder> GetMovableGridHoldersOrdered(Vector3 from)
     => _carGridHolders.Where(x => x.GetAvailableGrid().Item1)
        .OrderBy(y => Mathf.Abs(y.transform.position.x - from.x))
        .ThenBy(x => Vector3.Distance(from, x.transform.position)).Reverse().ToList();

    public Tuple<VertexPath, CarGrid> GetPath(Vector3 from, CarGridHolder[] carGridHolders)
    {
        var path = new NavMeshPath();
        Tuple<CarGrid, BezierPath> toGridTuple = GetBestCarGridNPath(from, carGridHolders);
        if (toGridTuple.Item1)
        {
            toGridTuple.Item1.Targeted(true);
            _pathCreator.bezierPath.GlobalNormalsAngle = 90;
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Free;
            _pathCreator.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
            return Tuple.Create(_pathCreator.path, toGridTuple.Item1);
        }
        else return new Tuple<VertexPath, CarGrid>(null, null);
    }

    private Tuple<CarGrid, BezierPath> GetBestCarGridNPath(Vector3 from, CarGridHolder[] carGridHolders)
    {
        CarGrid grid = null;
        Vector3 prevGridPos = from;
        foreach (var carGridHolder in carGridHolders)
        {
            if (grid) prevGridPos = grid.transform.position;
            Tuple<CarGrid, bool> tuple = carGridHolder.GetAvailableGrid();
            if (carGridHolder.deadEnd && tuple.Item1)
            {
                grid = tuple.Item1;
                AddSegmentToPath(from, grid, prevGridPos);
                break;
            }
            else if ((!tuple.Item2 && !carGridHolder.deadEnd) || carGridHolder.canBlockRoad)
            {
                grid = tuple.Item1 ? tuple.Item1 : grid;
                if (tuple.Item1) AddSegmentToPath(from, grid, prevGridPos);
                break;
            }
            else if (tuple.Item1)
            {
                grid = tuple.Item1;
                AddSegmentToPath(from, grid, prevGridPos);
            }

        }
        return Tuple.Create(grid, _pathCreator.bezierPath);
    }

    private void AddSegmentToPath(Vector3 from, CarGrid grid, Vector3 prevGridPos)
    {
        if (prevGridPos == from && grid)
        {
            _pathCreator.bezierPath = new BezierPath(GetNavMeshPath(from, grid.transform.position));
        }
        else if (prevGridPos != Vector3.zero && grid)
        {
            Vector3[] pathCorners = GetNavMeshPath(prevGridPos, grid.transform.position);
            for (int i = 0; i < pathCorners.Length; i++)
            {
                _pathCreator.bezierPath.AddSegmentToEnd(pathCorners[i]);
            }
        }
    }

    private Vector3[] GetNavMeshPath(Vector3 from, Vector3 to)
    {
        var path = new NavMeshPath();
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
        for (int i = 0; i < path.corners.Length; i++)
        {
            path.corners[i] = path.corners[i].WithY(path.corners[i].y + pathYOffset);
        }
        return path.corners;
    }
}
