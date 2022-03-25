using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    public new void OnInspectorGUI()
    {
        DrawDefaultInspector();

        AudioManager aM = (AudioManager)target;
        if (GUILayout.Button("Adjust Volume"))
            aM.AdjustFromInspector();
    }
}
#endif
