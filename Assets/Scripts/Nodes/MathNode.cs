using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(185)]
public class MathNode : Node {

    public enum MathOperation {
        Add,
        Subtract,
        Multiply,
        Divide,
    }
    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double input1;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double input2;
    [Output] public double output;
    public MathOperation operation;



    public override object GetAudioValue(NodePort port, double time) {
        input1 = GetInputAudioValue<double>("input1", time, this.input1);
        input2 = GetInputAudioValue<double>("input2", time, this.input2);
        if (port.fieldName == "output") {
            switch (operation) {
                case MathOperation.Add:
                    return input1 + input2;
                case MathOperation.Subtract:
                    return input1 - input2;
                case MathOperation.Multiply:
                    return input1 * input2;
                case MathOperation.Divide:
                    return input1 / input2;
                default:
                    return input1 + input2;
            }
        }
        else return null;
    }
}
