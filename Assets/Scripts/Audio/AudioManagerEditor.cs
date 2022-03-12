using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioManager aM = (AudioManager)target;

        if (GUILayout.Button("Adjust Volume"))
            aM.AdjustFromInspector();
    }
}
