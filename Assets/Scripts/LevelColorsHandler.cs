using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelColorsHandler : MonoBehaviour
{
    public static LevelColorsHandler Instance;

    [SerializeField] private Material roadMat;

    private void Awake() => Instance = this;

    private void Start()
    {
        Camera.main.backgroundColor = GameManager.currentlevelInfo.cameraSolidColor;
        roadMat.color = GameManager.currentlevelInfo.roadColor;
    }

    public Color GetTeamColor(int teamIndex) => GameManager.currentlevelInfo.teams[teamIndex].color;
}
