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

    public void ActivationSkill()   // ��ų Ȱ��ȭ ��Ȱ��ȭ
    {
        thisImg = this.GetComponent<UnityEngine.UI.Image>();
        Color imgColor = thisImg.color;
        AudioClip onOffAudio = null;


        if (!isActivated && SkillManager.activatedCount < 3) 
        {
            // �̹��� ���� ���� (����: ������ ó��)
            // 0���� 1������ ������ ���� ���� (1: ���� ������, 0: ���� ����)
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
