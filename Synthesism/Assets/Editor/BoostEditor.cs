

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BoostController))]
public class BoostEditor : Editor {

    private void OnSceneGUI()
    {
        BoostController bPad = target as BoostController;
        Handles.color = Color.cyan;
        Handles.ArrowHandleCap(0, bPad.transform.position, Quaternion.LookRotation(bPad.forceDir), 1.0f,EventType.Repaint);

    }

}
