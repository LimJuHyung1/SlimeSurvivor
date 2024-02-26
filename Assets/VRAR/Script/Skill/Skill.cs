using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Skill : MonoBehaviour
{
    bool isActivated = false;

    AudioSource audio;
    public AudioClip onAudio;
    public AudioClip offAudio;

    UnityEngine.UI.Image thisImg;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    public void ActivationSkill()   // 스킬 활성화 비활성화
    {
        thisImg = this.GetComponent<UnityEngine.UI.Image>();
        Color imgColor = thisImg.color;
        AudioClip onOffAudio = null;


        if (!isActivated && SkillManager.activatedCount < 3) 
        {
            // 이미지 투명도 조절 (예시: 반투명 처리)
            // 0부터 1까지의 값으로 투명도 조절 (1: 완전 불투명, 0: 완전 투명)
            imgColor.a = 1f;
            onOffAudio = onAudio;
            thisImg.color = imgColor;

            SkillManager.SetImg(thisImg.sprite);
        }

        if(isActivated && SkillManager.activatedCount > 0)
        {
            imgColor.a = 0.2f;
            onOffAudio = offAudio;
            thisImg.color = imgColor;

            SkillManager.RemoveImg();
        }

        audio.clip = onOffAudio;
        audio.Play();

        isActivated = !isActivated;
    }
}
