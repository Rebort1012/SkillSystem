using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_ObjectSwitch : MonoBehaviour
{
    
    public int selectedChar = 0;

    public int previousSelectedChar;

    void Start()
    {
        SelectExplosion ();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            NextEffect();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            PreviousEffect();
        }

        if (previousSelectedChar != selectedChar)
        {
            SelectExplosion();
        }
    }

    void SelectExplosion ()
    {
        int i = 0;
        foreach (Transform explosion in transform)
        {
            if (i == selectedChar)
                explosion.gameObject.SetActive(true);
            else
               explosion.gameObject.SetActive(false);
            i++;
        }
    }

    public void PreviousEffect() 
    {
        previousSelectedChar = selectedChar;

        if (selectedChar <= 0)
            selectedChar = transform.childCount - 1;
        else
            selectedChar--;
    }

    public void NextEffect() 
    {
        previousSelectedChar = selectedChar;

        if (selectedChar >= transform.childCount - 1)
            selectedChar = 0;
        else
            selectedChar++;
    }
}