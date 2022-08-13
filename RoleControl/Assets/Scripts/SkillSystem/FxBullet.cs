using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FxBullet : MonoBehaviour
{
    private float speed = 10;
    private Rigidbody rig;
    private float persistTime;
    public float curTime;
    
    void Start()
    {
        persistTime = GetComponent<SkillDeployer>().skillData.skill.durationTime;
        rig = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        curTime = 0;
     }
    
    void Update()
    {
        rig.velocity = transform.forward.normalized * speed;
        curTime += Time.deltaTime;
        if (curTime > persistTime)
        {
            if (gameObject.activeSelf)
            {
                GameObjectPool.I.Destory(gameObject);
            }

            curTime = 0;
        }
    }

    private void OnDisable()
    {
        if (rig != null)
        {
            rig.velocity = Vector3.zero;
        }
    }
}
