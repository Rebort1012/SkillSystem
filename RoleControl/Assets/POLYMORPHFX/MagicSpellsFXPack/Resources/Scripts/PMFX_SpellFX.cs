using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_SpellFX : MonoBehaviour
{
    public GameObject spellFX;
    public float spellFXDelay;
    //public float spellFXCooldown;

    AudioSource audioData;

    void Start()
    {
        audioData = GetComponent<AudioSource>();
    }

    IEnumerator spellAttack()
    {
        audioData.Play(0);
        yield return new WaitForSeconds(spellFXDelay);
        Instantiate(spellFX, transform.position, transform.rotation);
        //Invoke("CooledDown", spellFXCooldown);
        yield break;
    }

    /* 
    void CooledDown()
    {
        audioData.Stop();
    }*/

    public void CoroutineStarter() 
    {
        StartCoroutine(spellAttack());
    }
}