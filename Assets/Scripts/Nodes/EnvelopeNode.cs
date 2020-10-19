using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;

public class EnvelopeNode : Node {
    [Output] public double envelope;

    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public double duration;
    [Input(ShowBackingValue.Never, ConnectionType.Override)]
    public double endTime;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double attack;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double decay;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double sustain;
    [Input(ShowBackingValue.Unconnected, ConnectionType.Override)]
    public double release;


    public override object GetAudioValue(NodePort port, double time) {
        duration = GetInputAudioValue<double>("duration", time, duration);
        endTime  = GetInputAudioValue<double>("endTime",  time, endTime );
        attack   = GetInputAudioValue<double>("attack",   time, attack  );
        decay    = GetInputAudioValue<double>("decay",    time, decay   );
        sustain  = GetInputAudioValue<double>("sustain",  time, sustain );
        release  = GetInputAudioValue<double>("release",  time, release );
        return GetEnvelope(duration, endTime);
    }

    double Clamp(double v, double min, double max) {
        return Math.Min(Math.Max(v, min), max);
    }
    double Lerp(double x, double y, double t) {
        return x * (1.0 - t) + y * t;
    }
    double Smoothstep(double x, double y, double v) {
        // return (v - x) / (y - x);
        double inv = Clamp((v - x) / (y - x), 0.0, 1.0);
        return inv * inv * (3.0 - 2.0 * inv);
    }


    public double GetEnvelope(double time, double endTime) {

        double sustainLength = endTime - (attack + decay);

        double s1 = Clamp(Smoothstep(0.0, attack, time), 0.0, 1.0);
        double s2 = Clamp(Lerp(1.0, sustain,
            Smoothstep(attack, attack+decay, time)),sustain, 1.0);
        double s3 = Clamp(Lerp(1.0, 0.0, Smoothstep(
            attack + decay + sustainLength, attack + decay + sustainLength + release, time)), 0.0, 1.0);
        
        return s1 * s2 * s3;
    }
}

[CustomNodeEditor(typeof(EnvelopeNode))]
public class EnvelopeNodeEditor : NodeEditor {

    public override void OnBodyGUI() {
        
        base.OnBodyGUI();

        EnvelopeNode envelopeNode = target as EnvelopeNode;

        envelopeNode.attack  = envelopeNode.GetInputAudioValue<double>("attack", 0.0, envelopeNode.attack );
        envelopeNode.decay   = envelopeNode.GetInputAudioValue<double>("decay",  0.0, envelopeNode.decay  );
        envelopeNode.sustain = envelopeNode.GetInputAudioValue<double>("sustain",0.0, envelopeNode.sustain);
        envelopeNode.release = envelopeNode.GetInputAudioValue<double>("release",0.0, envelopeNode.release);

        double sustainLength = .5;
        float timeFrame = System.Convert.ToSingle(envelopeNode.attack +
            envelopeNode.decay + sustainLength + envelopeNode.release);
        int frames = 50;
        Keyframe[] keyframes = new Keyframe[frames];

        for (int i = 0; i < frames; i++) {
            float time = i / (float)frames * timeFrame;
            double endTime = sustainLength + envelopeNode.attack + envelopeNode.decay;
            float value = System.Convert.ToSingle(envelopeNode.GetEnvelope(time, endTime));
            keyframes[i] = new Keyframe(i/(float)frames, value);
        }

        AnimationCurve curve = new AnimationCurve(keyframes);

        EditorGUILayout.CurveField(curve);
    }
}