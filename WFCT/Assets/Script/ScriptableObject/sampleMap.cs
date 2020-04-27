using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sample", menuName = "ScriptableObjects/SampleMap", order = 1)]
[System.Serializable]
public class sampleMap : ScriptableObject
{
    public List<string> tileListPath;
    public sampleRow[] sampleTilesMap;
}

[System.Serializable]
public class sampleRow
{
    public int[] sampleTileData;
}