using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("HUD是否显示当前天数")]
    public bool ShowCurrentDayOnUI;

    public TMP_Text Text_Day;  

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
        }
    }
    
    private void Update()
    {
        Text_Day.text = $"Day {SceneSwitcher.Instance.CurrentDay.ToString()}";
        if (ShowCurrentDayOnUI)
        {
            Text_Day.gameObject.SetActive(true);
        }
        else
        {
            Text_Day.gameObject.SetActive(false);
        }
    }
}