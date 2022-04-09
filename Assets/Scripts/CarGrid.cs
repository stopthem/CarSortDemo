using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarGrid : MonoBehaviour
{
    [SerializeField] private Transform canvas;

    [SerializeField] private int teamIndex;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Car MyCar { get; private set; }
    public CarGridHolder MyGridHolder { get; private set; }

    public bool IsEmpty { get; private set; } = true;

    private void Start()
    {
        Vector3 lookAt = transform.InverseTransformPoint(Camera.main.transform.position.WithX(canvas.position.x));
        canvas.localRotation = Quaternion.LookRotation(canvas.localPosition.GetDirection(lookAt));
        canvas.localScale = Vector3.zero;

        spriteRenderer.color = GameManager.currentlevelInfo.teams[teamIndex].color;
        MyGridHolder = GetComponentInParent<CarGridHolder>();
    }

    public void Placed(Car car, int teamIndex)
    {
        MyCar = car;
        IsEmpty = false;
        if (teamIndex != this.teamIndex) GameManager.Fail();
        else
        {
            canvas.DOScale(Vector3.one, .25f).SetEase(Ease.OutBack);
            LevelConditionsChecker.Instance.PlacedAtGrid();
        }
    }

    public void Targeted(bool status) => IsEmpty = !status;
}
