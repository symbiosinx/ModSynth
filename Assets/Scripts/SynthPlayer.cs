using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SynthPlayer : MonoBehaviour {
    [SerializeField] SynthGraph graph;
    int sample = 0;
    double sampleRate = 48000.0;
    float[][] midiEvents = new float[4][];

    void Update() {
        midiEvents = MIDIEvents.instance.GetNotes();
    }

    void OnAudioFilterRead(float[] data, int channels) {
        

        float[] frequencies = midiEvents[0];
        float[] velocities = midiEvents[1];
        float[] durations = midiEvents[2];
        float[] timeSinceEnds = midiEvents[3];

        for (int i = 0; i < data.Length; i+= channels) {

            float output = 0f;
            double time = sample / sampleRate;

            for (int f = 0; f < frequencies.Length; f++) {
                graph.SetMIDI(frequencies[f], velocities[f], durations[f], timeSinceEnds[f]);

                output += graph.GetOutput(time);

            }

            for (int j = 0; j < channels; j++) {
                data[i+j] = output;
            }
            sample++;
        }

    }
}
