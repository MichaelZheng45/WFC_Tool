using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SamepleWindowEditor : EditorWindow
{
    int data;
    GameObject fg;
   // CollisionManager3D colManager3D;
    string name;
    [MenuItem("Window/SampleWindowEditor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(SamepleWindowEditor));
    }

    private void OnGUI()
    {
        data = EditorGUILayout.IntField("Number ", data);
      //  colManager3D = EditorGUILayout.ObjectField("GameObject", colManager3D, typeof(CollisionManager3D), true) as CollisionManager3D;
        name = EditorGUILayout.TextField("Name", name);
        if (GUILayout.Button("somethingh"))
        {
            name = "michael";
        }

        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.blue;
        GUILayout.Button("Label", style);


        //currentparticle gameobject filler
        //







        //if it does not exist then you can create new particle
        //
    }
    private void Update()
    {
     //   if (colManager3D == null)
     //   {
     //       colManager3D = GameObject.FindGameObjectWithTag("CollisionManager").GetComponent<CollisionManager3D>();
     //   }
    }

}