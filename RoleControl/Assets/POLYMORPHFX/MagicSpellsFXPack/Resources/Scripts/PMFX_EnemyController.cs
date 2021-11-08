using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_EnemyController : MonoBehaviour
{
    public PMFX_CameraShake cameraShake;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            animator.SetTrigger("Hit");
            StartCoroutine(cameraShake.Shake(.08f, .08f));
        }
    }
}