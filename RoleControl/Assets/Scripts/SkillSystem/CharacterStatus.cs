using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatus : MonoBehaviour
{
    /// <summary>生命 </summary>
    public float HP = 100;
    /// <summary>生命 </summary>
    public float MaxHP=100;
    /// <summary>当前魔法 </summary>
    public float SP = 100;
    /// <summary>最大魔法 </summary>
    public float MaxSP =100;
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

    private GameObject damagePopup;
    private Transform hudPos;

    public UIPortrait uiPortrait; 
    
    public virtual void Start()
    {
        if (CompareTag("Player"))
        {
            uiPortrait = GameObject.FindGameObjectWithTag("HeroHead").GetComponent<UIPortrait>();
        }
        else if (CompareTag("Enemy"))
        {
            Transform canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
            uiPortrait = Instantiate(Resources.Load<GameObject>("UIEnemyPortrait"), canvas).GetComponent<UIPortrait>();
            uiPortrait.gameObject.SetActive(false);
        }
        uiPortrait.cstatus = this;
        uiPortrait.RefreshHpMp();
        
        damagePopup = Resources.Load<GameObject>("HUD");
        
        selected = TransformHelper.FindChild(transform, "Selected").gameObject;
        HitFxPos = TransformHelper.FindChild(transform, "HitFxPos");
        FirePos = TransformHelper.FindChild(transform, "FirePos");
        hudPos = TransformHelper.FindChild(transform, "HUDPos");
    }
    
    /// <summary>受击 模板方法</summary>
    public virtual void OnDamage(float damage, GameObject killer,bool isBuff = false)
    {
        //应用伤害
        var damageVal = ApplyDamage(damage, killer);
        
        //应用PopDamage
        DamagePopup pop = Instantiate(damagePopup).GetComponent<DamagePopup>();
        pop.target = hudPos;
        pop.transform.rotation = Quaternion.identity;
        pop.Value = damageVal.ToString();
        
        //ApplyUI画像
        if (!isBuff)
        {
            uiPortrait.gameObject.SetActive(true);
            uiPortrait.transform.SetAsLastSibling();
            uiPortrait.RefreshHpMp();
        }
    }

    /// <summary>应用伤害</summary>
    public virtual float ApplyDamage(float damage, GameObject killer)
    {
        HP -= damage;
        //应用死亡
        if (HP <= 0)
        {
            HP = 0;
            Destroy(killer, 5f);
        }
        
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
