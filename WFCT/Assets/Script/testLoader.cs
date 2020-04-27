using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class testLoader : MonoBehaviour
{
	public tileObject loadObj;
	public sampleMap sample;
	public Vector3 startPos;
	private string path = "Assets/Resources/Pieces/";

	private void Start()
	{
		Debug.Log(sample.tileListPath.Count);
		/*
 
		for(int i = 0; i < loadObj.tilePieceList.Count; i++)
		{
			TileChild infoData = loadObj.tilePieceList[i];
			GameObject prefab = AssetDatabase.LoadAssetAtPath(infoData.objectName, typeof(GameObject)) as GameObject;

			GameObject piece = Instantiate(prefab, infoData.position, infoData.rotation,this.transform);
			piece.transform.localScale = infoData.scale;
		}

		*/

	}
}
