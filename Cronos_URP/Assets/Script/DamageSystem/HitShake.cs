using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitShake : MonoBehaviour
{
    [Header("Info")]
    private Vector3 _originalPos;
    private float _timer;
    private Vector3 _randomPos;
    private bool _isSake;

    [Header("Settings")]
    [Range(0f, 2f)]
    public float time = 0.2f;
    [Range(0f, 2f)]
    public float distance = 0.1f;
    [Range(0f, 0.1f)]
    public float delayBetweenShakes = 0f;

    private void LateUpdate()
    {
        if (_timer == 0f)
        {
            _originalPos = transform.position;
        }
    }

    public void Begin()
    {
        StopAllCoroutines();
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        _timer = 0f;
        _isSake = true;

        while (_timer < time)
        {
            _timer += Time.deltaTime;

            _randomPos = _originalPos + (Random.insideUnitSphere * distance);

            transform.position = _randomPos;

            if (delayBetweenShakes > 0f)
            {
                yield return new WaitForSeconds(delayBetweenShakes);
            }
            else
            {
                _isSake = false;
                yield return null;
            }
        }

        transform.position = _originalPos;
    }
}
