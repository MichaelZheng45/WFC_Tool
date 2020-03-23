using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


struct tileReference
{
    GameObject tilePiece;
    string prefabReference;
}

public class TileEditor : EditorWindow
{
    List<tileReference> tilePieces;

    string tileName;

    bool togglePieceSelection;

    int pieceIndex;

    [MenuItem("Window/TileEditor")]
    static void Init()
    {

    }

    void OnGUI()
    {
        //name of this tile
        tileName = EditorGUILayout.TextField("Name", tileName);

        //toggle show tile piece selection, clicking on it will spawn the object and set the index to it
        togglePieceSelection = EditorGUILayout.Toggle("Selection", togglePieceSelection);

        if(togglePieceSelection)
        {
            //display all pieces if one is selected, togglepieceselection becomes false
        }
        else
        {

        }
        //list of all current piece objects

        //an index to look through each piece
        //based on the index, the gameobject will highlight itself
        //editable transforms of piece at index
        //ability to remove said piece


        //ability to save the information as a script
    }

    
}
