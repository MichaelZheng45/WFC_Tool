using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileObject", order = 2)]
[System.Serializable]
public class tileObject : ScriptableObject
{
	//contains all the pieces the tile contains
	public List<TileChild> tilePieceList;

	public string assetPath;
}
