using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
// using XNodeEditor;

[DisallowMultipleNodes]
[NodeWidth(150)]
[NodeTint(.45f, .45f, .65f)]
public class OutputNode : Node {
    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public double signal;
    public override object GetAudioValue(NodePort port, double time) {
        return GetInputAudioValue<object>("signal", time);
    }
}



// [CustomNodeEditor(typeof(OutputNode))]
// public class OutputNodeEditor : NodeEditor {

//     public override void OnBodyGUI() {    

//         base.OnBodyGUI();

//         OutputNode outputNode = target as OutputNode;

//         object obj = outputNode.GetValue();
//         if (obj != null) EditorGUILayout.LabelField(obj.ToString());
//     }
// }
