using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGrid : MonoBehaviour
{
    [SerializeField] private int teamIndex;
    [SerializeField] private SpriteRenderer spriteRenderer;
    public CarGridHolder MyGridHolder { get; private set; }

    public bool IsEmpty { get; private set; } = true;

    private void Start()
    {
        spriteRenderer.color = GameManager.currentlevelInfo.teams[teamIndex].color;
        MyGridHolder = GetComponentInParent<CarGridHolder>();
    }

    public void Placed(int teamIndex)
    {
        IsEmpty = false;
        if (teamIndex != this.teamIndex) GameManager.Fail();
    }
}
