using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = -0.15f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.3f, 0.3f) * magnitude;
            float y = Random.Range(-0.3f, 0.3f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }
}