using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

public enum Waveform {
    Sine,
    Square,
    Triangle,
    Sawtooth
}

public class OscillatorNode : Node {
    
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double frequency;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double amplitude;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double phase;
    [Output] public double signal;

    [NodeEnum]
    public Waveform waveform;

    public override object GetAudioValue(NodePort port, double time) {
        frequency = GetInputAudioValue<double>("frequency", time, frequency);
        amplitude = GetInputAudioValue<double>("amplitude", time, amplitude);
        phase = GetInputAudioValue<double>("phase", time, phase);
        if (port.fieldName == "signal") {
            switch (waveform) {
                case Waveform.Sine:
                    return amplitude * GetSine(time);
                case Waveform.Square:
                    return amplitude * Math.Sign(GetSine(time));
                case Waveform.Triangle:
                    return amplitude * GetTriangle(time);
                case Waveform.Sawtooth:
                    return amplitude * GetSawtooth(time);
                default:
                    return 0.0;
            }
        }
        else return null;
    }

    double GetSine(double time) {
        return Math.Sin((time * frequency + phase) * Math.PI * 2.0);
    }

    double Frac(double value) {
        return value - Math.Truncate(value);
    }

    double GetTriangle(double time) {
        double frac = Frac(time * frequency + phase + 0.25);
        return 1.0 - 4.0 * Math.Abs(0.5 - frac);
    }

    double GetSawtooth(double time) {
        return Frac(time * frequency + phase + 0.5) * 2.0 - 1.0;
    }
}


[CustomNodeEditor(typeof(OscillatorNode))]
public class OscillatorNodeEditor : NodeEditor {

    public override void OnBodyGUI() {
        
        base.OnBodyGUI();

        OscillatorNode oscillatorNode = target as OscillatorNode;

        float timeFrame = 1f/20f;
        int frames = 100;
        Keyframe[] keyframes = new Keyframe[frames];

        for (int i = 0; i < frames; i++) {
            float time = i / timeFrame / 48000f;
            float value = System.Convert.ToSingle(oscillatorNode.GetAudioValue(oscillatorNode.GetPort("signal"), time));
            keyframes[i] = new Keyframe(i/(float)frames, value);
        }

        AnimationCurve curve = new AnimationCurve(keyframes);

        EditorGUILayout.CurveField(curve);
    }
}
