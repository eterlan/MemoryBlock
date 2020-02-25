using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;

    [Tooltip("所有需要切换的scene")]
    public List<SceneReference> Scenes;

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
        Debug.Log(Scenes[CurrentSceneNumber].ScenePath);
        // 会在加载新场景的时候自动卸载当前的吗？
        SceneManager.LoadScene(Scenes[CurrentSceneNumber].ScenePath);
    }
}
