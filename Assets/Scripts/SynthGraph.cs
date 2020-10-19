using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
[RequireNode(typeof(MIDINode))]
[RequireNode(typeof(OutputNode))]
public class SynthGraph : NodeGraph {
    OutputNode outputNode;
    MIDINode midiNode;

    OutputNode GetOutputNode() {
        if (outputNode != null) return outputNode;
        else {
            OutputNode output = null;
            foreach (var node in nodes) {
                output = node as OutputNode;
                if (output != null) {
                    break;
                }
            }
            return output;
        }
    }

    MIDINode GetMIDINode() {
        if (midiNode != null) return midiNode;
        else {
            MIDINode midi = null;
            foreach (var node in nodes) {
                midi = node as MIDINode;
                if (midi != null) {
                    break;
                }
            }
            return midi;
        }
    }

    public float GetOutput(double time) {
        outputNode = GetOutputNode();
        return Convert.ToSingle((outputNode.GetAudioValue(null, time)));
    }

    public void SetMIDI(double frequency, double velocity, double duration, double endTime) {
        midiNode = GetMIDINode();
        midiNode.frequency = frequency;
        midiNode.velocity = velocity;
        midiNode.duration = duration;
        midiNode.endTime = endTime;
    }

    public void SetEnvelope() {

    }
}
