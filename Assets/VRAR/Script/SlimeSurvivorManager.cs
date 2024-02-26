using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlimeSurvivorManager : MonoBehaviour
{
    public GameObject[] monsters;
    // 0 - ������
    // 1 - ������
    // 2 - ����
    // 3 - ����
    // 4 - �ź���

    public GameObject[] dragons;
    // 0 - �ʷ�
    // 1 - ����
    // 2 - �˺��
    // 3 - �Ķ�

    public GameObject[] cubes;
    private int randIndex;

    public static int totalMonsterCount = 0;

    public Text playerHp;
    public Text clearTime;
    public Button attackButton;
    public Button exitButton;
    public Button yesExitButton;
    public Button noExitButton;

    public Image exitImg;
    public Image retryImg;
    public GameObject player;
    int mutStatus = 1;
    int addStatus = 1;

    float minutes;
    float seconds;

    int audioCount = 0;
    public AudioClip firstSound;
    public AudioClip secondSound;
    public AudioClip thirdSound;
    AudioSource audio;

    public Image blackScreen;
    float fadeSpeed = 1.0f;

    public Text timerText;
    private float currentTime = 0f;

    public static System.Action<int, Vector3> spawnMonster;
    public static System.Action<Transform, int> returnCube;
    public static System.Action IsDamaged;
    public static System.Action<float> endScreen;

    void Awake()
    {
        retryImg.gameObject.SetActive(false);
    }

    void Start()
    {
        Application.targetFrameRate = 60;

        audio = GetComponent<AudioSource>();
        audio.clip = firstSound;
        audio.Play();

        FadeToTransparent(fadeSpeed);

        spawnMonster = (index, vec) => SpawnMonster(index, vec);
        returnCube = (pos, index) => MakeCube(pos, index);
        IsDamaged = () => DamagedTiming();
        endScreen = (alpha) => SetTransparency(alpha);
    }

    void Update()
    {
        if(player != null)
            playerHp.text = "Player hp : " + player.GetComponent<Player>().hp.ToString();

        if(Player.isDead == false)
        {
            currentTime += Time.deltaTime;

            minutes = Mathf.FloorToInt(currentTime / 60);
            seconds = Mathf.FloorToInt(currentTime % 60);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            retryImg.gameObject.SetActive(true);
            clearTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }            

        if (!audio.isPlaying)
        {
            if (audioCount == 0)
            {
                // �� ��° ����������� ��ȯ
                PlayBackgroundMusic(secondSound);
                audioCount++;
            }
            else
            {
                PlayBackgroundMusic(thirdSound);
            }
        }
    }

    void PlayBackgroundMusic(AudioClip music)
    {
        audio.clip = music;
        audio.Play();
    }

    void FadeToTransparent(float interval)
    {
        // ������ 0���� ������ �����Ͽ� ������������ �մϴ�.
        blackScreen.CrossFadeAlpha(0, interval, false);
        Invoke("UnactiveBlackGround", interval);
    }

    void UnactiveBlackGround()
    {
        blackScreen.gameObject.SetActive(false);
    }

    public void SetTransparency(float alphaValue)
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = Color.gray;
        Color screenColor = blackScreen.color;
        screenColor.a = Mathf.Clamp01(alphaValue); // ���İ��� 0�� 1 ���̷� Ŭ�����մϴ�.
        blackScreen.color = screenColor;
    }

    public void DamagedTiming()
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.color = Color.red;
        FadeToTransparent(0.3f);
    }


    public void SpawnMonster(int i, Vector3 spawnVec)
    {
        GameObject monster = Instantiate(monsters[i]);
        monster.GetComponent<Monsters>().hp += mutStatus;
        monster.transform.position = spawnVec;
        totalMonsterCount += 1;
        mutStatus++;
        
        if(totalMonsterCount % 40 == 0)
        {
            SpawnDragon();
        }
    }

    private void SpawnDragon()
    {
        // �ݺ������� ȣ��Ǵ� �Լ��� ����
        int randIndex = (int)UnityEngine.Random.Range(0, dragons.Length);

        GameObject dragon = Instantiate(dragons[randIndex],
            player.transform.position + player.transform.forward * 30 + new Vector3(0, 5, 0), 
            Quaternion.identity);
        dragon.GetComponent<Dragons>().hp += 50 * addStatus;
        addStatus++;
    }

    public void MakeCube(Transform pos, int i)
    {        
        randIndex = (int)UnityEngine.Random.Range(0, 8);
        for(int j = 0; j < i; j++)
        {
            GameObject cube = Instantiate(this.cubes[randIndex]);
            cube.transform.position = pos.position
                + new Vector3((int)UnityEngine.Random.Range(0, 2)
                , 2f
                , (int)UnityEngine.Random.Range(0, 2));
        }        
    }

    public void UninteractableAttackButton(float coolTime)
    {
        attackButton.interactable = false; // ��ư ��Ȱ��ȭ
        Invoke("InteractableAttackButton", coolTime);
    }

    void InteractableAttackButton()
    {
        attackButton.interactable = true;
    }

    public void TogglePause()
    {
        exitImg.gameObject.SetActive(true);
        exitButton.GetComponent<AudioSource>().Play();
        Time.timeScale = 0.00001f; // ���� �ð��� ����ϴ�.
    }    

    public void TogglePlay()
    {
        noExitButton.GetComponent<AudioSource>().Play();
        exitImg.gameObject.SetActive(false);
        Time.timeScale = 1f; 
    }

    public void BackIntroScene()
    {
        Time.timeScale = 1f; 

        Invoke("LoadIntroScene", 1f);
    }


    void LoadIntroScene()
    {
        SceneManager.LoadScene("IntroScene");
    }
}
