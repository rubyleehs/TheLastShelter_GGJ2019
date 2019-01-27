﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private Vector3 _originalPos;
    public static CameraShake _instance;

    void Awake()
    {
        _originalPos = transform.localPosition;

        _instance = this;
    }

    public static void Shake(float duration, float amount)
    {
        _instance.StopAllCoroutines();
        _instance.StartCoroutine(_instance.cShake(duration, amount));
    }

    public IEnumerator cShake(float duration, float amount)
    {
        float endTime = Time.unscaledTime + duration;
        int i = 0;
        while (Time.unscaledTime < endTime && i < 500)
        {
            i++;
            transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

            //duration -= Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = _originalPos;
    }
}
