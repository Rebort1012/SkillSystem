using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillBox : MonoBehaviour
{
    public Image[] skillImgs;
    public Text[] skillTexts;
    public Button[] skillBtn;
    private CharacterSkillManager csm;
    void Start()
    {
        csm = FindObjectOfType<CharacterSkillManager>();
        CharacterSkillSystem css = FindObjectOfType<CharacterSkillSystem>();
        for (int i = 0; i < skillBtn.Length; ++i)
        {
            int j = i;
            skillBtn[i].onClick.AddListener(() => { css.AttackUseSkill(j + 1); });
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < skillImgs.Length; ++i)
        {
            //if(skillImgs[i].fillAmount < 1)
            //    skillBtn[i].enabled = false;
            //else
            //    skillBtn[i].enabled = true;

            float cd = csm.skills[i].coolRemain;
            skillImgs[i].fillAmount = 1 - cd / csm.skills[i].skill.coolTime;
            skillTexts[i].text = cd.ToString("F0");
            if (skillTexts[i].text == "0")
                skillTexts[i].text = "";
        }

        //Debug.Log( RayTool.RaycastUI());
    }
}
