using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Crosshair))]
public class CrosshairEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Crosshair crosshair = (Crosshair)target;
        crosshair.UpdateCrosshair();
    }
}
