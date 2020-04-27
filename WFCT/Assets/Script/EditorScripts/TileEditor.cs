using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


struct tileReference
{
    public GameObject tilePiece;
    public string prefabReference;
}

public class TileEditor : EditorWindow
{
    bool initTile = false;

    tileObject tileObj;

    List<tileReference> tilePieces = new List<tileReference>();
   
    string tileName;

    //bounding box size
    float boundingX=1;
    float boundingY=1;
    float boundingZ=1;
    //piece selection
    bool togglePieceSelection;
    private List<tileReference> palette = new List<tileReference>();
    private string path = "Assets/Resources/Pieces";
    private int selectionIndex;


    private string basePiece = "Assets/Resources/tileBase.prefab";
    //index of the current piece on the tile to be modified
    int pieceIndex;

    [MenuItem("Window/TileEditor")]
    static void Init()
    {
        TileEditor tEditor = (TileEditor)EditorWindow.GetWindow(typeof(TileEditor));
        tEditor.Show();
        tEditor.refreshSelection();
    }

    void OnGUI()
    {
        if(initTile == false)
        {
            GUILayout.Label("Place tile to be edited", EditorStyles.boldLabel);
            tileObj = EditorGUILayout.ObjectField("Tile", tileObj, typeof(tileObject), true) as tileObject;
            if (GUILayout.Button("Load Tile"))
            {
                initTile = true;
                for (int i = 0; i < tileObj.tilePieceList.Count; i++)
                {
                    TileChild infoData = tileObj.tilePieceList[i];
                    GameObject prefab = AssetDatabase.LoadAssetAtPath(infoData.objectName, typeof(GameObject)) as GameObject;
                    Debug.Log(infoData.objectName);
                    GameObject piece = Instantiate(prefab, infoData.position, infoData.rotation);
                    piece.transform.localScale = infoData.scale;

                    tileReference tileRef = new tileReference();
                    tileRef.tilePiece = piece;
                    tileRef.prefabReference = infoData.objectName;

                    tilePieces.Add(tileRef);
                }


            }

            if(GUILayout.Button("New Tile"))
            {
                //make a base tile
                GameObject baseprefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Base.prefab", typeof(GameObject));
                GameObject basepiece = Instantiate(baseprefab, new Vector3(0, 0 - baseprefab.transform.localScale.y / 2, 0), Quaternion.identity);
                tileReference baserefPiece = new tileReference();
                baserefPiece.prefabReference = "Assets/Resources/Base.prefab";
                baserefPiece.tilePiece = basepiece;
                tilePieces.Add(baserefPiece);

                //make a base tile
                GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/FloorTile.prefab", typeof(GameObject));
                GameObject piece = Instantiate(prefab, new Vector3(0,0 - prefab.transform.localScale.y/2,0), Quaternion.identity);
                tileReference refPiece = new tileReference();
                refPiece.prefabReference = "Assets/Resources/FloorTile.prefab";
                refPiece.tilePiece = piece;
                tilePieces.Add(refPiece);

                initTile = true;
            }
        }
        else
        {
            TileEdit();
        }
    }

    
    private void OnSceneGUI(SceneView sceneView) 
    {
        drawBoundingBox();
    }

    void TileEdit()
    {

        //name of this tile
        tileName = EditorGUILayout.TextField("Name", tileName);

        //toggle show tile piece selection, clicking on it will spawn the object and set the index to it
        togglePieceSelection = EditorGUILayout.Toggle("Selection", togglePieceSelection);

        if (togglePieceSelection)
        {

            GUILayout.Label("Select a piece to spawn", EditorStyles.boldLabel);
            //display all pieces if one is selected, togglepieceselection becomes false
            List<GUIContent> pieceIcons = new List<GUIContent>();
            foreach (tileReference prefab in palette)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(prefab.tilePiece);
                pieceIcons.Add(new GUIContent(texture));
            }
            selectionIndex = GUILayout.SelectionGrid(selectionIndex, pieceIcons.ToArray(), 6);

            //generate piece and reupdate current piece index
            if (GUILayout.Button("Create Piece"))
            {
                GameObject newPiece = Instantiate(palette[selectionIndex].tilePiece, Vector3.zero, Quaternion.identity);
                tileReference newRef;
                newRef.tilePiece = newPiece;
                newRef.prefabReference = palette[selectionIndex].prefabReference;
                tilePieces.Add(newRef);

                pieceIndex = tilePieces.Count - 1;
                togglePieceSelection = false;

            }
        }
        else if (tilePieces.Count > 0)
        {
            GUILayout.Label("Select a piece to edit", EditorStyles.boldLabel);
            //list of all current piece objects
            //an index to look through each piece
            List<GUIContent> pieceIcons = new List<GUIContent>();
            for (int i = 0; i < tilePieces.Count; i++)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(tilePieces[i].tilePiece);
                pieceIcons.Add(new GUIContent(texture));
            }
            pieceIndex = GUILayout.SelectionGrid(pieceIndex, pieceIcons.ToArray(), 5);

            //based on the index, the gameobject will highlight itself

            //editable transforms of piece at index
            GUILayout.Space(100);
            GUILayout.Label("Select a piece to edit", EditorStyles.miniLabel);
            tilePieces[pieceIndex].tilePiece.transform.position = EditorGUILayout.Vector3Field("Object Position", tilePieces[pieceIndex].tilePiece.transform.position);
            tilePieces[pieceIndex].tilePiece.transform.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Object Rotation", tilePieces[pieceIndex].tilePiece.transform.rotation.eulerAngles));
            tilePieces[pieceIndex].tilePiece.transform.localScale = EditorGUILayout.Vector3Field("Object Position", tilePieces[pieceIndex].tilePiece.transform.localScale);

            //ability to remove said piece
            if (GUILayout.Button("Remove Piece"))
            {
                DestroyImmediate(tilePieces[pieceIndex].tilePiece);
                tilePieces.RemoveAt(pieceIndex);
                pieceIndex = 0;
            }

        }

        GUILayout.Space(100);

        //ability to save the information as a scriptable, will ovveride existing scriptable if it exists
        if (GUILayout.Button("Save tile to name"))
        {
            tileObject tObject = new tileObject();
            tObject.tilePieceList = new List<TileChild>();

            for (int i = 0; i < tilePieces.Count; i++)
            {
                TileChild newPiece = new TileChild();
                newPiece.objectName = tilePieces[i].prefabReference;

                Transform transformData = tilePieces[i].tilePiece.transform;
                newPiece.position = transformData.position;
                newPiece.rotation = transformData.rotation;
                newPiece.scale = transformData.localScale;

                tObject.tilePieceList.Add(newPiece);

                DestroyImmediate(tilePieces[i].tilePiece);
            }
            tObject.assetPath = "Assets/Resources/Tiles/" + tileName + ".asset";
            AssetDatabase.CreateAsset(tObject, "Assets/Resources/Tiles/" + tileName + ".asset");

            initTile = false;
            tileObj = null;

            tilePieces = new List<tileReference>();

            tileName = "";

            pieceIndex = 0;

            refreshSelection();
        }

    }

    private void drawBoundingBox()
    {
        Handles.DrawWireCube(Vector3.zero - new Vector3(0,boundingY/2,0), new Vector3(boundingX, boundingY, boundingZ));
    }
    private void refreshSelection()
    {
        palette.Clear();
        string[] prefabFiles = System.IO.Directory.GetFiles(path, "*.prefab");
        foreach (string prefabFile in prefabFiles)
        {
            tileReference newRef;
            newRef.tilePiece = AssetDatabase.LoadAssetAtPath(prefabFile, typeof(GameObject)) as GameObject;
            newRef.prefabReference = prefabFile;
            palette.Add(newRef);
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

}
