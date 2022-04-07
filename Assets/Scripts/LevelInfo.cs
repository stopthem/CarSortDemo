using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelInfo")]
public class LevelInfo : ScriptableObject
{
    public GameObject[] availableCars;
    public Color cameraSolidColor;
    public Color roadColor;
    public Team[] teams;
}

[System.Serializable]
public class Team
{
    public Color color;
    public int carCount;
}
