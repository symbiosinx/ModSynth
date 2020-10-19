using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MIDIEvents : MonoBehaviour {

    [SerializeField] int octave = 3;
    [SerializeField] float maxRelease = 1f;

    public static MIDIEvents instance;

    List<float> frequencies = new List<float>();
    List<float> velocities = new List<float>();
    List<float> durations = new List<float>();
    List<float> endTimes = new List<float>();

    void Awake() {
        instance = this;
    }

    void Update() {

        // Debug.Log(frequencies.Count);

        // Updating time and removing old notes 
        for (int i = frequencies.Count - 1; i >= 0; i--) {
            durations[i] += Time.deltaTime;

            if (durations[i] - endTimes[i] > maxRelease) {
                frequencies.RemoveAt(i);
                velocities.RemoveAt(i);
                durations.RemoveAt(i);
                endTimes.RemoveAt(i);
            }
        }

        // Adding pressed notes and ending depressed notes
        for (int i = 0; i < keyCodes.Length; i++) {
            
            float frequency = NoteToPitch(i, octave);

            // Add new Inputs onto the list
            if (Input.GetKeyDown(keyCodes[i])) {
                frequencies.Add(frequency);
                velocities.Add(1f);
                durations.Add(0f);
                endTimes.Add(float.MaxValue);
            }

            if (Input.GetKeyUp(keyCodes[i])) {

                for (int j = 0; j < frequencies.Count; j++) {
                    if (frequencies[j] == frequency) {
                        if (endTimes[j] == float.MaxValue)
                            endTimes[j] = durations[j];
                    }
                }
            }
        }
    }

    KeyCode[] keyCodes = new KeyCode[] {
        KeyCode.Z,
        KeyCode.S,
        KeyCode.X,
        KeyCode.D,
        KeyCode.C,
        KeyCode.V,
        KeyCode.G,
        KeyCode.B,
        KeyCode.H,
        KeyCode.N,
        KeyCode.J,
        KeyCode.M,
        KeyCode.Comma,
        KeyCode.L,
        KeyCode.Period,
        KeyCode.Semicolon,
        KeyCode.Slash
    };


    // public float[][] GetNotes() {

    //     List<float> frequencies = new List<float>();
    //     List<float> velocities = new List<float>();
    //     List<float> durations = new List<float>();
    //     List<float> timeSinceEnds = new List<float>();

    //     int octave = 3;

    //     for (int i = 0; i < keyCodes.Length; i++) {
    //         if (Input.GetKey(keyCodes[i])) {
    //             float frequency = NoteToPitch(i, octave);
    //             frequencies.Add(frequency);
    //             velocities.Add(1f);
    //             durations.Add(0f);
    //         }
    //     }

    //     float[][] output = new float[2][];
    //     output[0] = frequencies.ToArray();
    //     output[1] = velocities.ToArray();
    //     return output;
    // }

    public float[][] GetNotes() {
        float[][] output = new float[4][];
        output[0] = frequencies.ToArray();
        output[1] = velocities.ToArray();
        output[2] = durations.ToArray();
        output[3] = endTimes.ToArray();
        return output;
    }
    

    float NoteToPitch(int note, int octave) {
        return 440f * Mathf.Pow(Mathf.Pow(2f, 1f/12f), octave*12-8 + note - 49);
    }

}
