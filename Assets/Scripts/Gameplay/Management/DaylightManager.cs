using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaylightManager : MonoBehaviour
{

    public GameObject lighting;
    
    private int currentTime = 0;
    // m = mins per in-game day then TIPS = 86400 / (m * 60)
    private int timeIncrementPerSecond = 18; // 1440 = 60s per day, 72 = 20 mins per in-game day, 18 = 8 mins per in-game day
    private bool timeFrozen = false;

    void FixedUpdate() {
        if(timeFrozen) return;

        this.currentTime += Mathf.RoundToInt(Time.fixedDeltaTime * timeIncrementPerSecond);
        this.currentTime %= 86400;

        UpdateIntensity();
    }

    void UpdateIntensity() {
        lighting.GetComponent<Light>().intensity = CalculateIntensity(this.currentTime);
    }

    float CalculateIntensity(int time) {
        float intensity = (float) 0.5 * (1 + Mathf.Cos((Mathf.PI * time / 43200) - Mathf.PI));
        return intensity;
    }

    public void SetTimeFrozen(bool frozen) {
        this.timeFrozen = frozen;
    }

    public void SetLightIntensity(float intensity) {
        intensity = Mathf.Clamp(intensity, 0, 1);
        int time = Mathf.RoundToInt((43200 * (Mathf.Acos(2 * intensity - 1) + Mathf.PI)) / Mathf.PI) % 86400;
        this.currentTime = time;
        UpdateIntensity();
    }

    public void SetTime(int time) {
        this.currentTime = time % 86400;
        UpdateIntensity();
    }
}
