using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PMFX_SceneManager : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    PMFX_PlayerController pmfx_PlayerController;
    PMFX_ObjectSwitch pmfx_ObjectSwitch;
    PMFX_SpellFX pmfx_SpellFX;
    PMFX_CastFX pmfx_CastFX;

    [SerializeField] private float shootDelay;
    bool canShoot = true;

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitScene()
    {
        Application.Quit();
    }

    public void CameraOne()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
    }

    public void CameraTwo()
    {
        cam1.SetActive(false);
        cam2.SetActive(true);
    }

    public void PreviousEffect()
    {
        pmfx_ObjectSwitch = FindObjectOfType<PMFX_ObjectSwitch>();
        pmfx_ObjectSwitch.PreviousEffect();

    }

    public void NextEffect()
    {
        pmfx_ObjectSwitch = FindObjectOfType<PMFX_ObjectSwitch>();
        pmfx_ObjectSwitch.NextEffect();
    }

    public void PlayEffect()
    {
        if (canShoot)
        {
            canShoot = false;
            pmfx_PlayerController = FindObjectOfType<PMFX_PlayerController>();
            pmfx_SpellFX = FindObjectOfType<PMFX_SpellFX>();
            pmfx_CastFX = FindObjectOfType<PMFX_CastFX>();

            pmfx_CastFX.CoroutineStarter();
            pmfx_SpellFX.CoroutineStarter();
            pmfx_PlayerController.PlayerAnimation();
            Invoke("EnableCanShoot", shootDelay);
        }
        else
        {
            return;
        }
    }

    void EnableCanShoot()
    {
        if (!canShoot)
            canShoot = true;
    }
}