using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GL_3DNet))] 

public class CustomGUI : Editor {

    GL_3DNet net;
    
    SerializedProperty segment;
    SerializedProperty Position;
    SerializedProperty x, y, z, size;
    
    
    private void OnEnable()
    {
        segment = serializedObject.FindProperty("go_segment");
        Position = serializedObject.FindProperty("position");
        x = serializedObject.FindProperty("xx");
        y = serializedObject.FindProperty("yy");
        z = serializedObject.FindProperty("zz");
        size = serializedObject.FindProperty("size");
        net = (GL_3DNet)target;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Label("Great Script");

        EditorGUILayout.PropertyField(size);
        EditorGUILayout.PropertyField(x);
        EditorGUILayout.PropertyField(y);
        EditorGUILayout.PropertyField(z);        

        EditorGUILayout.PropertyField(segment, true);
        EditorGUILayout.PropertyField(Position, true);
        
        if (GUILayout.Button(new GUIContent("Create net", "Make net on button")))
        {            
            net.CreateNet();
        }
        if (GUILayout.Button(new GUIContent("Clear a list")))
        {
            net.ClearList();
        }
        if (GUILayout.Button(new GUIContent("Get position")))
        {
            net.SetPosition();
        }
        serializedObject.ApplyModifiedProperties();
    }
   

}
