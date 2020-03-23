using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileObject", order = 2)]
public class tileObject : ScriptableObject
{
	//contains all the pieces the tile contains
	List<TileChild> tilePieceList;

}
