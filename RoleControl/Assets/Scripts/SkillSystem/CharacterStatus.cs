using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    /// <summary>生命 </summary>
    public int HP = 100;
    /// <summary>生命 </summary>
    public int MaxHP=100;
    /// <summary>当前魔法 </summary>
    public int SP = 100;
    /// <summary>最大魔法 </summary>
    public int MaxSP =100;
    /// <summary>伤害基数</summary>
    public float damage = 100;
    ///<summary>命中</summary>
    public float hitRate = 1;
    ///<summary>闪避</summary>
    public float dodgeRate = 1;
    /// <summary>防御</summary>  
    public float defence = 10f;
    /// <summary>主技能攻击距离 ,用于设置AI的攻击范围，与目标距离此范围内发起攻击</summary>
    public float attackDistance = 2;
    /// <summary>受击特效挂点 挂点名为HitFxPos </summary>
    [HideInInspector]
    public Transform HitFxPos;
    [HideInInspector]
    public Transform FirePos;
    
    public GameObject selected;
    
    [HideInInspector]
    public float normalAtt;
    /// <summary>Buff列表 </summary>
    protected float buffDamage;
    protected float durationTime;
    protected float interval;
    
    private GameObject damagePopup;
    private Transform hudPos;
    
    public virtual void Start()
    {
        //StartCoroutine(InitHUD());
        damagePopup = Resources.Load<GameObject>("HUD");
        
        HitFxPos = TransformHelper.FindChild(transform, "HitFxPos");
        FirePos = TransformHelper.FindChild(transform, "FirePos");
        hudPos = TransformHelper.FindChild(transform, "HUDPos");
    }
    
    /// <summary>受击 模板方法</summary>
    public virtual void OnDamage(int damage, GameObject killer)
    {
        //应用伤害
        var damageVal = ApplyDamage(damage, killer);
        //应用HUD 
        DamagePopup pop = Instantiate(damagePopup).GetComponent<DamagePopup>();
        pop.target = hudPos;
        pop.transform.rotation = Quaternion.identity;
        pop.Value = damageVal.ToString();
        //ApplyHUD(damageVal);
        //应用死亡
        if (HP <= 0)
        {
            HP = 0;
        }
        else
        {
            //buff创建
        }
        //Dead(killer);
    }
    
    /// <summary>应用伤害</summary>
    public virtual int ApplyDamage(int damage, GameObject killer)
    {
        HP -= damage;
        
        return damage;
    }
    
    /// <summary>
    /// 死亡
    /// </summary>
    /// <param name="killer">杀手</param>
    public virtual void Dead(GameObject killer)
    {

    }
}
