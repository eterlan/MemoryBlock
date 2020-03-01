using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using PixelCrushers;
using UnityStandardAssets.Characters.FirstPerson;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [Tooltip("所有需要切换的scene")]
    public List<SceneReference> Scenes;

    public FirstPersonController FPScontroller;

    // 如果非空，传送到下一个场景中叫这个名称的物体的位置
    [Tooltip("如果非空，将人物传送到下一个场景中叫这个名称的物体的位置")]
    public string SpawnPointInNextScene;

    // Note: 如果场景按正确的顺序放进list，那么玩家第一天应该是第七个场景。
    public int CurrentDay
    {
        get => CurrentSceneNumber - 7;
    }
    
    [SerializeField,ReadOnly]
    private int CurrentSceneNumber = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); 
            
            SaveSystem.sceneLoaded += OnSceneLoaded;
        }
    }

    // 按c键切换
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeScene();
        }
    }

    // 切换逻辑
    public void ChangeScene()
    {
        CurrentSceneNumber = CurrentSceneNumber < Scenes.Count - 1 
            ? CurrentSceneNumber + 1 
            : 0;

        var sceneName = Scenes[CurrentSceneNumber].ScenePath;
        // 为什么getSceneByPath失败了？因为 GetSceneByPath 只能获得已经load的scene的reference
        Debug.Log(sceneName);

        // 为了解决转场景时坠空的问题
        FPScontroller.enabled =  false;

        SaveSystem.LoadScene(string.IsNullOrEmpty(SpawnPointInNextScene) 
            ? sceneName 
            : sceneName + "@" + SpawnPointInNextScene);

        //SceneManager.GetSceneByPath("d").name
        // 会在加载新场景的时候自动卸载当前的吗？
    }

    private void OnSceneLoaded(string sceneName, int sceneIndex)
    {
        StartCoroutine(AfterSceneLoaded(0.1f));
        Debug.Log("sceneLoaded!");
    }

    private IEnumerator AfterSceneLoaded(float second)
    {
        yield return new WaitForSeconds(0.1f);
        FPScontroller.enabled = true;
        Debug.Log("AfterSceneLoaded!");
    }
}
