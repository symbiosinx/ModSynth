using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(150)]
public class ValueNode : Node {
    [Output(ShowBackingValue.Always)] public double value;

    public override object GetAudioValue(NodePort port, double time) {
        if (port.fieldName == "value") {
            return value; 
        }
        else return null;
    }
}
