using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

public class DisplayNode : Node {

    [Input(ShowBackingValue.Never, ConnectionType.Override)] public Anything input;

    public override object GetAudioValue(NodePort port, double time) {
        return GetInputAudioValue<object>("input", time, input);
    }

    [System.Serializable]
    public class Anything {}
}

[CustomNodeEditor(typeof(DisplayNode))]
public class DisplayNodeEditor : NodeEditor {

    public override void OnBodyGUI() {
        
        base.OnBodyGUI();

        DisplayNode displayNode = target as DisplayNode;

        // Get the value from the node, and display it
        object obj = displayNode.GetAudioValue(null, 0f);
        if (obj != null) EditorGUILayout.LabelField(obj.ToString());
    }
}
