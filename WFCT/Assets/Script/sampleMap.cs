using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sample", menuName = "ScriptableObjects/SampleMap", order = 1)]
public class sampleMap : ScriptableObject
{
    List<GameObject> tileList;
    int[,] sampleTilesMap;
}
