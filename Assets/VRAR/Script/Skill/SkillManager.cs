using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    Dictionary<string, int> skillDict = new Dictionary<string, int>();          // 스킬 레벨
    Dictionary<string, bool> skillActivation = new Dictionary<string, bool>();  // 스킬 활성화

    public Button skillTreeButton;  // on/off 기능
    public Image skillTree;
    public GameObject[] skillBox;
    public Image[] skillImgs;
    
    public static int activatedCount = 0;

    AudioSource audio;

    public AudioClip skillTreeAudio;    // 스킬 트리 열고 닫는 소리
    public AudioClip[] skillAudio;      // 스킬 클릭시 나오는 소리

    Vector2 transPos = new Vector2(800, 0);
    bool isOpened = false;

    public GameObject player;
    public GameObject[] skillEffect;
    public GameObject levelEffect;

    private Vector3[] skillSize;
    int index;  // skillEffect에 접근하기 위해 사용
    bool isAbleToSkill = true;
    int buffMode = 0;

    public static Action<Sprite> SetImg;      // 스킬 이미지 세팅
    public static Action RemoveImg;      // 스킬 이미지 삭제
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

    void SetSkillBox(string resourceName)   // 스킬 획득
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
    public void LevelUp(string resourceName)    // 자원 흡수 조건 만족시 스킬 획득 및 레벨업
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
                // 자원 공격 속도 감소
                case "Tree Cube(Clone)":
                    player.GetComponent<Player>().atkDelay -= 0.3f;
                    Debug.Log("자원 공격 속도 감소");
                    break;
                // Hp 증가
                case "Stone Cube(Clone)":
                    player.GetComponent<Player>().maxHp += 10f;
                    player.GetComponent<Player>().hp += 10f;
                    Debug.Log("Hp 증가");
                    break;
                // 이동속도 증가
                case "Flower Cube(Clone)":
                    player.GetComponent<Player>().walkSpeed += 2f;
                    Debug.Log("이동속도 증가");
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
                Debug.Log(resourceName + ": 레벨 1 달성!");
            }
            else
            {
                skillDict[resourceName] += 1;
                SelectSkillLevelUp(resourceName, skillDict[resourceName]);                
            }
        }

        Invoke("OffLevelUp", 2.5f);
    }

    void SelectSkillLevelUp(string n, int level)       // 레벨업 한 스킬 수치 전송
    {
        switch (n)
        {
            case "Hell Cube(Clone)":    // castle - 메테오
                skillEffect[0].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Hell Cube2(Clone)":   // Spike - 에너지 드레인
                skillEffect[1].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Snowman Cube(Clone)": // Snowman - 아이스 스피어
                skillEffect[2].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Candy Cube(Clone)":   // Candy - 라이트닝 스트라이크
                skillEffect[3].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Candy2 Cube(Clone)":  // Cake - 힐
                skillEffect[4].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Desert Cube(Clone)":  // Cactus - 스매쉬
                skillEffect[5].GetComponent<SkillCollider>().skillLevel = level;
                break;
            case "Desert2 Cube(Clone)": // Hill - 해머 크러쉬
                skillEffect[6].GetComponent<SkillCollider>().skillLevel = level;
                break;

            default:
                Debug.Log("SetSkillBox Error!");
                break;
        }

    }

    public void SetImage(Sprite i)  // 스킬 칸 이미지 변경
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

    public void SkillTreeButton()   // 스킬창 열고 닫기
    {
        RectTransform btn = skillTreeButton.GetComponent<RectTransform>();
        RectTransform img = skillTree.GetComponent<RectTransform>();

        Vector2 btnPos = btn.anchoredPosition; // X, Y 좌표
        Vector2 imgPos = img.anchoredPosition; // X, Y 좌표

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

    public void OnSkill(int i, int t)   // 스킬 인덱스, 파티클 active-false 시간
    {
        if (isAbleToSkill)  // 스킬 나타날 동안 다른 스킬 사용 불가
        {
            isAbleToSkill = false;
            index = i;
            skillEffect[i].gameObject.SetActive(true);

            if(i == 1)  // hp 흡수
            {
                Player.faceMode = 2;
                skillEffect[i].transform.position = player.transform.position;
                SkillCollider.isDrain = true;
                audio.clip = skillAudio[1];
                audio.Play();
                Invoke("OffDrain", 10f);
            }
            else if(i == 4) // hp 회복
            {
                Player.faceMode = 2;
                skillEffect[i].transform.position = player.transform.position;
                audio.clip = skillAudio[4];
                audio.Play();
                player.GetComponent<Player>().hp += 3 * skillEffect[i].GetComponent<SkillCollider>().skillLevel;
            }
            else
            {
                // 플레이어가 바라보는 방향 구하기
                Vector3 playerForward = player.transform.forward;

                // 바라보는 방향에서 조금 떨어진 좌표 계산
                float distance = 5.0f; // 원하는 거리 설정
                Vector3 offsetPosition = player.transform.position + (playerForward * distance);

                skillEffect[i].transform.position = offsetPosition;

                audio.clip = skillAudio[i];
                audio.Play();
            }

            // 스킬 레벨에 따라 콜라에더 스케일 설정
            BoxCollider box = skillEffect[i].GetComponent<BoxCollider>();
            int lev = skillEffect[i].GetComponent<SkillCollider>().skillLevel;

            skillEffect[i].gameObject.transform.localScale 
                = skillSize[i] + new Vector3(lev/2, lev/2, lev/2);

            // 파티클을 실행하기 위해 활성화
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
