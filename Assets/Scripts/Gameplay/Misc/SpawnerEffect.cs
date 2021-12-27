using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* This code is heavily based on the Effects Examples from Unity
* Specifically the SpawnEffect.cs script has been altered to create this class
**/
public class SpawnerEffect : MonoBehaviour
{

    public float spawnEffectTime = 3;
    public AnimationCurve fadeIn;

    ParticleSystem ps;
    float timer = 0;
    Renderer _renderer;
    int shaderProperty;

    void Start() {
        shaderProperty = Shader.PropertyToID("_cutoff");
        _renderer = GetComponent<Renderer>();
        ps = GetComponentInChildren <ParticleSystem>();

        var main = ps.main;
        main.duration = spawnEffectTime;
    }

    public void RestartEffect() {
        this.timer = 0;
        ps.Play();
    }

    void Update() {
        if(timer > spawnEffectTime) {
            return;
        }

        timer += Time.deltaTime;
        _renderer.material.SetFloat(shaderProperty, fadeIn.Evaluate(Mathf.InverseLerp(0, spawnEffectTime, timer)));
    }
}
