using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMFX_ProjectilesFX : MonoBehaviour
{

    public float Speed = 15f;
    public float lifetime = 3f;
    public GameObject impact;

    public ParticleSystem detachFX;

	void FixedUpdate()
	{
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * Speed;
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            detachFX.transform.parent = null;
            Instantiate(impact, transform.position, transform.rotation);
        }         
    }
}