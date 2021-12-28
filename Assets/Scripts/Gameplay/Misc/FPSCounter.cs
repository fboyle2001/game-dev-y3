using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/**
* Used to show the FPS
**/
public class FPSCounter : MonoBehaviour {
    
    private TMP_Text fpsText;
    private int averageOverFrames = 300;
    private Queue<float> frameDeltaTimes = new Queue<float>();

    void Awake() {
        fpsText = GetComponent<TMP_Text>();
    }

    void Update() {
        // Show the FPS this frame as well as a running average over the past averageOverFrames frames
        int currentFps = (int)(1 / Time.unscaledDeltaTime);
        frameDeltaTimes.Enqueue(Time.unscaledDeltaTime); 

        if(frameDeltaTimes.Count > averageOverFrames) {
            frameDeltaTimes.Dequeue();
        }

        float avgUnscaledDeltaTime = frameDeltaTimes.Average();
        int averageFps = (int)(1 / avgUnscaledDeltaTime);

        fpsText.text = $"A: {averageFps}, C: {currentFps}";
    }
}
