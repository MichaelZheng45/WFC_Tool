using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEditor;

public class Generator : MonoBehaviour
{
    public sampleMap mapData;

    public int outputX;
    public int outputY;
    public int seed;
    private void Start()
    {
        generate();
    }
    public void generate()
    {
        int sizeX = mapData.sampleTilesMap[0].sampleTileData.Length;
        int sizeY = mapData.sampleTilesMap.Length;
        byte[,] sampleData = new byte[sizeX,sizeY];
        for(int y= 0; y < sizeY; y++)
        {
            for(int x = 0; x < sizeX; x++)
            {
                sampleData[x, y] = (byte)mapData.sampleTilesMap[y].sampleTileData[x];
            }
        }

        //new class
        TwoDimWaveFunctionCollapse model = new TwoDimWaveFunctionCollapse(sampleData, mapData.tileListPath, 2, outputX, outputY, false, false, 8, 0);

        //class.run
        for(int i = 0; i < 10; i++)
        {
            if(model.Run(seed, 0));
            {
                break;
            }
        }

        int[,] finalMapData =  model.draw();
        for(int y = 0; y < outputY; y++)
        {
            for(int x = 0; x < outputX; x++)
            {
                createTile(x, y, mapData.tileListPath[finalMapData[x, y]]);
            }
        }   
    }


    private void createTile(int x, int y, string tile)
    {
        tileObject tileObj = AssetDatabase.LoadAssetAtPath(tile, typeof(tileObject)) as tileObject;

        TileChild baseinfoData = tileObj.tilePieceList[0];
        GameObject baseprefab = AssetDatabase.LoadAssetAtPath(baseinfoData.objectName, typeof(GameObject)) as GameObject;

        GameObject basepiece = Instantiate(baseprefab, Vector3.zero, Quaternion.identity);

        for (int i = 1; i < tileObj.tilePieceList.Count; i++)
        {

            TileChild infoData = tileObj.tilePieceList[i];
            GameObject prefab = AssetDatabase.LoadAssetAtPath(infoData.objectName, typeof(GameObject)) as GameObject;

            GameObject piece = Instantiate(prefab, infoData.position, infoData.rotation, basepiece.transform);
            piece.transform.localScale = infoData.scale;
        }
        basepiece.transform.position = new Vector3(x + .5f, 0, y + .5f);
    }

}
