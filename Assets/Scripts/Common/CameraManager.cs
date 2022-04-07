using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class CameraManager : Singleton<CameraManager>
{
    public enum Positions
    {
        Start,
        Character
    }
    private const float speed = 2.25f;

    [SerializeField] private Camera otherCamera;
    [SerializeField] private Transform positionsHolder;
    [HideInInspector] public Positions _currentPos;
    private Transform[] positionsInOrder;

    private void Awake()
    {
        positionsInOrder = positionsHolder.GetComponentsInChildren<Transform>().Where(x => x.transform != positionsHolder).ToArray();
    }

    private void Start()
    {
        if (otherCamera == null)
        {
            otherCamera = Camera.main;
        }
    }

    public static Sequence Move(Transform to, float duration, Ease ease = Ease.InOutQuad, bool local = false) => Instance.transform.DoMoveRotate(to, duration, local, ease);
    public static Sequence Move(Positions position, float duration, Ease ease = Ease.InOutQuad, bool local = false) => Instance.transform.DoMoveRotate(Instance.positionsInOrder[(int)position], duration, local, ease);
}
