using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_CastFX : MonoBehaviour
{
    [SerializeField] ParticleSystem castFX;
    public float castFXDelay;

    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    IEnumerator cast()
    {
        audioData.Play(0);
        yield return new WaitForSeconds(castFXDelay);
        castFX.Play();
        yield break;
    }


    public void CoroutineStarter() 
    {
        StartCoroutine(cast());
    }
}