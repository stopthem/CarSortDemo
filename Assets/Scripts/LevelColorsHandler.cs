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
        Camera.main.backgroundColor = GameManager.currentLevelInfo.cameraSolidColor;
        roadMat.color = GameManager.currentLevelInfo.roadColor;
    }

    public Color GetTeamColor(int teamIndex) => GameManager.currentLevelInfo.teams[teamIndex].color;
}