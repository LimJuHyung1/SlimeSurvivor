using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill2 : MonoBehaviour
{
    public Image skillImg;

    public void ClickSkillButton()
    {
        if(skillImg.GetComponent<Image>().sprite != null)
        {
            // 사진 이름으로 스킬 분별 + 스킬 인덱스와 SetActive(false) 될 시간 전달
            switch (skillImg.GetComponent<Image>().sprite.name)
            {
                case "01_Fireball_nobg":
                    SkillManager.skillStart(0, 4);
                    break;
                case "16_life_drain_nobg":
                    SkillManager.skillStart(1, 2);
                    break;
                case "05_Ice_shards_nobg":
                    SkillManager.skillStart(2, 4);
                    break;
                case "07_Lightning_Strike_nobg":
                    SkillManager.skillStart(3, 1);
                    break;
                case "13_Healing_spell_2_nobg":
                    SkillManager.skillStart(4, 2);
                    break;
                case "09_Melee_slash_nobg":
                    SkillManager.skillStart(5, 1);
                    break;
                case "06_Lightning_Bolt_nobg":
                    SkillManager.skillStart(6, 1);
                    break;
            }
        }
    }
}
