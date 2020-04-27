using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using JetBrains.Annotations;

public class SampleMapEditor : EditorWindow
{
    string sampleMapName;

    int sizeX = 0;
    int sizeY = 0;

    bool editTiles = false;
    bool eraser = false;
    sampleMap sMap;

    tileObject selectedTile = null;

    List<string> tileObjectList;
    List<GameObject> tileList;
    GameObject[,] gridtiles;

    [MenuItem("Window/Sample Map Editor")]
    static void Init()
    {
        SampleMapEditor tEditor = (SampleMapEditor)EditorWindow.GetWindow(typeof(SampleMapEditor));
        tEditor.tileList = new List<GameObject>();
        tEditor.gridtiles = new GameObject[0, 0];
        tEditor.tileObjectList = new List<string>();
    }

    void OnGUI()
    {
        sampleMapName = EditorGUILayout.TextField("Sample Map Name", sampleMapName);

        sizeX = EditorGUILayout.IntField("Grid X Length", sizeX);
        sizeY = EditorGUILayout.IntField("Grid Y Length", sizeY);

        //check if the size have been changed
        if (gridtiles.GetLength(0) != sizeX || gridtiles.GetLength(1) != sizeY)
        {
            gridtiles = new GameObject[sizeX, sizeY];

            for(int i = 0; i < tileList.Count; i++)
            {
                int indexX = Mathf.FloorToInt(tileList[i].transform.position.x);
                int indexY = Mathf.FloorToInt(tileList[i].transform.position.z);

                gridtiles[indexX, indexY] = tileList[i];
            }
        }

        editTiles = EditorGUILayout.Toggle("Edit Tiles", editTiles);

        if(editTiles)
        {
            tileEditing();
        }

        //finish
        if(GUILayout.Button("Finish Map"))
        {
            sampleMap newMap = new sampleMap();

            List<string> uniqueTilePaths = new List<string>();
            foreach(string tilePath in tileObjectList)
            {
                if(!uniqueTilePaths.Contains(tilePath))
                {
                    uniqueTilePaths.Add(tilePath);
                }
            }
            newMap.tileListPath = uniqueTilePaths;
            newMap.sampleTilesMap = new sampleRow[sizeY];
            for(int j = 0; j < sizeY; j++)
            {
                newMap.sampleTilesMap[j] = new sampleRow();
                newMap.sampleTilesMap[j].sampleTileData = new int[sizeX];
                for(int i = 0; i < sizeX; i++)
                {
                    //given the object in grid, find the index in game object list
                    int objectIndex = tileList.IndexOf(gridtiles[i, j]);
            
                    //use given object index and find the string path and then use that string path to find the proper tile index in the unique list
                    int index = uniqueTilePaths.IndexOf(tileObjectList[objectIndex]);

                    newMap.sampleTilesMap[j].sampleTileData[i] = index;
                }
            }

            AssetDatabase.CreateAsset(newMap, "Assets/Resources/SampleMap/" + sampleMapName + ".asset");

            foreach(GameObject obj in tileList)
            {
                DestroyImmediate(obj);
            }

            this.Close();
        }
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        drawBoundingBox();
        if(editTiles)
        {
            onSceneTileEditing();
        }
    }

    void OnFocus()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI; // Just in case
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    private void drawBoundingBox()
    {
        for(int YInd = 0; YInd < sizeY; YInd++)
        {
            for(int XInd = 0; XInd < sizeX; XInd++)
            {
                Handles.DrawWireCube(new Vector3(XInd + .5f, 0, YInd + .5f),new Vector3(1,1,1));
            }
        }
    }

    private void onSceneTileEditing()
    {
        // Get the mouse position in world space such as z = 0
        Ray guiRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float magnitude = (-guiRay.origin.y / guiRay.direction.y);
        Vector3 atPosition = guiRay.origin + guiRay.direction * magnitude;
        Handles.DrawSphere(0,atPosition, Quaternion.identity, 1);
        //   Vector3 mousePosition = guiRay.origin - guiRay.direction * (guiRay.origin.z / guiRay.direction.z);

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && selectedTile !=  null)
        {

            int indexX = Mathf.FloorToInt(atPosition.x);
            int indexY = Mathf.FloorToInt(atPosition.z);

            if(indexX < gridtiles.GetLength(0) && indexY < gridtiles.GetLength(1))
            {

                if (gridtiles[indexX, indexY] != null)
                {
                    GameObject oldTlile = gridtiles[indexX, indexY];
                    tileObjectList.RemoveAt(tileList.IndexOf(oldTlile));
                    tileList.Remove(oldTlile);

                    DestroyImmediate(oldTlile);
                }

                //if eraser than does not spawn anything
                if(!eraser)
                {
                    GameObject tile = loadtile(selectedTile);
                    gridtiles[indexX, indexY] = tile;
                    tileList.Add(tile);
                    tileObjectList.Add(selectedTile.assetPath);

                    tile.transform.position = new Vector3(indexX + .5f, 0, indexY + .5f);
                }

            }
        }
    }

    private void tileEditing()
    {
        //tile selection
        selectedTile = EditorGUILayout.ObjectField("Select Tile", selectedTile, typeof(tileObject), true) as tileObject;

        eraser = EditorGUILayout.Toggle("Eraser", eraser);
    }

    private GameObject loadtile(tileObject tileObj)
    {
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

        return basepiece;
    }
}
