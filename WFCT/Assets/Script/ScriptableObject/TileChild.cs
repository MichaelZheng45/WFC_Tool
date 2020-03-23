using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileChild
{
	Vector3 position;
	Quaternion rotation;
	Vector3 scale;

	//note: it should be under folder "TilePiece/" ex TilePiece/TreeTile
	string objectName = "";
}
