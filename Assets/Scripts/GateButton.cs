using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GateButton : MonoBehaviour
{
    private int _teamIndex;
    private float _coolDown;
    private MeshRenderer _pressableMesh;
    [SerializeField] private Ease pressedEase;
    [SerializeField] private Transform pressableButton;
    [SerializeField] private Transform buttonGoTo;
    private bool _canPress = true;
    private Vector3 _pressableStartLocal;
    private TeamCarsHolder _teamCarsHolder;

    private void Awake()
    {
        _pressableMesh = pressableButton.GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        _pressableStartLocal = pressableButton.localPosition;
    }

    public void Init(int teamIndex, float coolDown, Color teamColor, TeamCarsHolder teamCarsHolder)
    {
        _pressableMesh.material.color = teamColor;
        _teamIndex = teamIndex;
        _coolDown = coolDown;
        _teamCarsHolder = teamCarsHolder;
    }

    private void OnMouseDown()
    {
        if (!_canPress) return;
        _teamCarsHolder.Open();
        _canPress = false;
        DOTween.Sequence()
        .Append(pressableButton.DOLocalMove(buttonGoTo.localPosition, _coolDown / 2).SetEase(pressedEase))
        .Append(pressableButton.DOLocalMove(_pressableStartLocal, _coolDown / 2).SetEase(pressedEase))
        .AppendCallback(() => _canPress = true);
    }
}
