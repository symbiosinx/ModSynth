using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[DisallowMultipleNodes]
[NodeWidth(150)]
[NodeTint(.5f, .4f, .5f)]
public class MIDINode : Node {

    
    // [Rename("Frequency (hz)")]
    [Output] public double frequency;
    [Output] public double velocity;
    [Output] public double duration;
    [Output] public double endTime;

    public override object GetAudioValue(NodePort port, double time) {

        if (port.fieldName == "frequency") {
            return frequency;
        }
        else if (port.fieldName == "velocity") {
            return velocity;
        }
        else if (port.fieldName == "duration") {
            return duration;
        }
        else if (port.fieldName == "endTime") {
            return endTime;
        }
        else return null;

    }

}
