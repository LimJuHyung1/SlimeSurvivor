using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    Dictionary<string, int> skillDict = new Dictionary<string, int>();          // ��ų ����
    Dictionary<string, bool> skillActivation = new Dictionary<string, bool>();  // ��ų Ȱ��ȭ

    public Button skillTreeButton;  // on/off ���
    public Image skillTree;
    public GameObject[] skillBox;
    public Image[] skillImgs;
    
    public static int activatedCount = 0;

    AudioSource audio;

    public AudioClip skillTreeAudio;    // ��ų Ʈ�� ���� �ݴ� �Ҹ�
    public AudioClip[] skillAudio;      // ��ų Ŭ���� ������ �Ҹ�

    Vector2 transPos = new Vector2(800, 0);
    bool isOpened = false;

    public GameObject player;
    public GameObject[] skillEffect;
    public GameObject levelEffect;

    private Vector3[] skillSize;
    int index;  // skillEffect�� �����ϱ� ���� ���
    bool isAbleToSkill = true;
    int buffMode = 0;

    public static Action<Sprite> SetImg;      // ��ų �̹��� ����
    public static Action RemoveImg;      // ��ų �̹��� ����
    public static Action<int, int> skillStart;

    void Awake()
    {
        SetImg = (s) => SetImage(s);
        RemoveImg = () => RemoveImage();
        skillStart = (index, time) => OnSkill(index, time);

        skillSize = new Vector3[skillEffect.Length];
        for(int i = 0; i < skillEffect.Length; i++)
        {
            skillSize[i] = skillEffect[i].gameObject.transform.localScale;
        }

        audio = GetComponent<AudioSource>();        
    }

    void SetSkillBox(string resourceName)   // ��ų ȹ��
    {
        switch (resourceName)
        {
            case "Hell Cube(Clone)":    // castle
                skillBox[0].SetActive(true);
                break;
            case "Hell Cube2(Clone)":   // Spike
                skillBox[1].SetActive(true);
                break;
            case "Snowman Cube(Clone)": // Snowman
                skillBox[2].SetActive(true);
                break;
            case "Candy Cube(Clone)":   // Candy
                skillBox[3].SetActive(true);
                break;
            case "Candy2 Cube(Clone)":  // Cake
                skillBox[4].SetActive(true);
                break;
            case "Desert Cube(Clone)":  // Cactus
                skillBox[5].SetActive(true);
                break;
            case "Desert2 Cube(Clone)": // Hill
                skillBox[6].SetActive(true);
                break;

            default:
                Debug.Log("SetSkillBox Error!");
                break;
        }
    }
    public void LevelUp(string resourceName)    // �ڿ� ��� ���� ������ ��ų ȹ�� �� ������
    {
        levelEffect.SetActive(true);
        levelEffect.transform.position = player.transform.position;
        levelEffect.GetComponent<AudioSource>().Play();

        if(resourceName == "Tree Cube(Clone)" || 
            resourceName == "Stone Cube(Clone)" || 
            resourceName == "Flower Cube(Clone)")
        {
            switch (resourceName)
            {
                // �ڿ� ���� �ӵ� ����
                case "Tree Cube(Clone)":
                    player.GetComponent<Player>().atkDelay -= 0.3f;
                    Debug.Log("�ڿ� ���� �ӵ� ����");
                    break;
                // Hp ����
                case "Stone Cube(Clone)":
                    player.GetComponent<Player>().maxHp += 10f;
                    player.GetComponent<Player>().hp += 10f;
                    Debug.Log("Hp ����");
                    break;
                // �̵��ӵ� ����
                case "Flower Cube(Clone)":
                    player.GetComponent<Player>().walkSpeed += 2f;
                    Debug.Log("�̵��ӵ� ����");
                    break;
            }
        }
        else
        {
            if (!skillDict.ContainsKey(resourceName))
            {
                skillDict.Add(resourceName, 1);
                SetSkillBox(resourceName);
                SelectSkillLevelUp(resourceName, skillDict[resourceName]);
                Debug.Log(resourceName + ": ���� 1 �޼�!");
            }
            else
            {
                skillDict[resourceName] += 1;
                SelectSkillLevelUp(resourceName, skillDict[resourceName]);                
            }
        }

        Invoke("OffLevelUp", 2.5f);
    }

    void SelectSkillLevelUp(string n, int level)       // ������ �� ��ų ��ġ ����
    {
        switch (n)
        {
            case "Hell Cube(Clone)":    // castle - ���׿�
                skillEffect[0].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Hell Cube2(Clone)":   // Spike - ������ �巹��
                skillEffect[1].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Snowman Cube(Clone)": // Snowman - ���̽� ���Ǿ�
                skillEffect[2].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Candy Cube(Clone)":   // Candy - ����Ʈ�� ��Ʈ����ũ
                skillEffect[3].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Candy2 Cube(Clone)":  // Cake - ��
                skillEffect[4].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Desert Cube(Clone)":  // Cactus - ���Ž�
                skillEffect[5].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Desert2 Cube(Clone)": // Hill - �ظ� ũ����
                skillEffect[6].GetComponent<SkillCollider>().skillLevel = level;
                break;

            default:
                Debug.Log("SetSkillBox Error!");
                break;
        }

    }

    public void SetImage(Sprite i)  // ��ų ĭ �̹��� ����
    {
        Color tmpColor = skillImgs[activatedCount].GetComponent<Image>().color;

        tmpColor.a = 1f;
        skillImgs[activatedCount].color = tmpColor;
        skillImgs[activatedCount].GetComponent<Image>().sprite = i;
        activatedCount += 1;
    }

    public void RemoveImage()
    {
        activatedCount -= 1;
        Color tmpColor = skillImgs[activatedCount].GetComponent<Image>().color;
        tmpColor.a = 0f;
        skillImgs[activatedCount].color = tmpColor;
    }

    public void SkillTreeButton()   // ��ųâ ���� �ݱ�
    {
        RectTransform btn = skillTreeButton.GetComponent<RectTransform>();
        RectTransform img = skillTree.GetComponent<RectTransform>();

        Vector2 btnPos = btn.anchoredPosition; // X, Y ��ǥ
        Vector2 imgPos = img.anchoredPosition; // X, Y ��ǥ

        if (!isOpened)
        {
            btn.anchoredPosition = btnPos - transPos;
            img.anchoredPosition = imgPos - transPos;            
        }
        else
        {
            btn.anchoredPosition = btnPos + transPos;
            img.anchoredPosition = imgPos + transPos;
        }

        audio.clip = skillTreeAudio;
        audio.Play();
        isOpened = !isOpened;
    }

    public void OnSkill(int i, int t)   // ��ų �ε���, ��ƼŬ active-false �ð�
    {
        if (isAbleToSkill)  // ��ų ��Ÿ�� ���� �ٸ� ��ų ��� �Ұ�
        {
            isAbleToSkill = false;
            index = i;
            skillEffect[i].gameObject.SetActive(true);

            if(i == 1)  // hp ���
            {
                Player.faceMode = 2;
                skillEffect[i].transform.position = player.transform.position;
                SkillCollider.isDrain = true;
                audio.clip = skillAudio[1];
                audio.Play();
                Invoke("OffDrain", 10f);
            }
            else if(i == 4) // hp ȸ��
            {
                Player.faceMode = 2;
                skillEffect[i].transform.position = player.transform.position;
                audio.clip = skillAudio[4];
                audio.Play();
                player.GetComponent<Player>().hp += 3 * skillEffect[i].GetComponent<SkillCollider>().skillLevel;
            }
            else
            {
                // �÷��̾ �ٶ󺸴� ���� ���ϱ�
                Vector3 playerForward = player.transform.forward;

                // �ٶ󺸴� ���⿡�� ���� ������ ��ǥ ���
                float distance = 5.0f; // ���ϴ� �Ÿ� ����
                Vector3 offsetPosition = player.transform.position + (playerForward * distance);

                skillEffect[i].transform.position = offsetPosition;

                audio.clip = skillAudio[i];
                audio.Play();
            }

            // ��ų ������ ���� �ݶ󿡴� ������ ����
            BoxCollider box = skillEffect[i].GetComponent<BoxCollider>();
            int lev = skillEffect[i].GetComponent<SkillCollider>().skillLevel;

            skillEffect[i].gameObject.transform.localScale 
                = skillSize[i] + new Vector3(lev/2, lev/2, lev/2);

            // ��ƼŬ�� �����ϱ� ���� Ȱ��ȭ
            var particleSystem = skillEffect[buffMode].GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
                Invoke("OffSkill", t);
            }
        }
    }

    void OffSkill()
    {
        Player.faceMode = 0;
        isAbleToSkill = true;
        skillEffect[index].gameObject.SetActive(false);
    }

    void OffLevelUp()
    {
        levelEffect.SetActive(false);
    }

    void OffDrain()
    {
        SkillCollider.isDrain = false;
    }
}
