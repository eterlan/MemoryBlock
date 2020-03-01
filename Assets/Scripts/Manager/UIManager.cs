using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BrunoMikoski.TextJuicer;

public class UIManager : MonoBehaviour
{
    // 检视器上的开关
    [Tooltip("HUD是否显示当前天数")]
    public bool ShowCurrentDayOnUI;

    // 需要设置的值
    public TMP_Text Text_Day;  
    
    // 控制逐字动画
    //public TMP_TextJuicer TEST_Juicer;

    [HideInInspector]
    public static UIManager Instance;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ChangeDayOnSceneLoaded();
    }

    private void OnValidate()
    {
        ControllShowDayOnValidate();
    }
    
    private void Update() 
    {
        // 控制逐字动画
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     AnimateWord();
        // };
    }

    // 控制关于出现【日期】
    private void ChangeDayOnSceneLoaded()
    {
        Text_Day.text = $"Day {SceneSwitcher.Instance.CurrentDay.ToString()}";
        Debug.Log("hi");
    }
    private void ControllShowDayOnValidate()
    {
        if (ShowCurrentDayOnUI)
        {
            Text_Day.gameObject.SetActive(true);
        }
        else
        {
            Text_Day.gameObject.SetActive(false);
        }
    }

    // 控制逐字动画
    // private void AnimateWord()
    // {
    //     TEST_Juicer.Play();
    // }

}