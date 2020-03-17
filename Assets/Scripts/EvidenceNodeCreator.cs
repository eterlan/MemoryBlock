﻿using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameState
{
    // Singleton
    private GameState(){}
    public static readonly GameState Instance = new GameState();
    public int Day;
    public Location Location;
    public Speaker LastSpeaker;
    public int RemainTagAtInbox;
}

[CreateAssetMenu(fileName = "EvidenceNodeCreatorSetting", menuName = "Setting/EvidenceNodeCreator")]
public class EvidenceNodeCreator : SerializedScriptableObject
{
    public GameObject NodePrefab;
    // Should be Inbox
    public RectTransform Parent;
    public List<EvidenceNodeGroup> NodeGroupAuthoring;

    // 当切换人物的时候发起事件
    private void Awake()
    {
        // 将CheckNodeCondition注册到事件上
        
    }
    
    // void OnSpeakerChanged()
    //      {
    //          CheckNodeCondition();
    //      }
    //      void OnSceneLoaded()
    //      {
    //          CheckNodeCondition();
    //      }

    // 人讲完话或切换场景的时候，检测是否有符合条件的NodeGroup
    void CheckNodeCondition()
    {
        EvidenceNodeGroup nodeGroup = null;
        for (var i = 0; i < NodeGroupAuthoring.Count; i++)
        {
            if (NodeGroupAuthoring[i].Verify(GameState.Instance.LastSpeaker, GameState.Instance.Day, GameState.Instance.Location))
            {
                nodeGroup = NodeGroupAuthoring[i];
                NodeGroupAuthoring.RemoveAt(i);
                break;
            }
        }

        if (nodeGroup != null)
        {
            // Instantiate Node 改成协程，1s一个。那是什么时候展示呢？第一次打开UI的时候吗？对。
            var instance = Instantiate(NodePrefab,Vector3.zero,Quaternion.identity,Parent.transform);
            // Set Node
            nodeGroup.SetNodeInLayoutGroup(instance, Parent);
        }
        else
        {
            Debug.Log("Doesn't find node");
        }
    }
}


public class EvidenceNodeGroup : SerializedScriptableObject
{
    public class EvidenceNodeCondition
    {
        // 条件：前一个讲话的人，第几天，场景
        public Speaker Speaker;
        public int Day;
        public Location Location;
    }

    public bool Verify(Speaker speaker, int day, Location location)
    {
        return speaker  == Condition.Speaker 
            && day      == Condition.Day 
            && location == Condition.Location;
    }
    public EvidenceNodeCondition Condition;

    public string[] Texts;

    public Image[] Images;

    public void SetNodeInLayoutGroup(GameObject instance, RectTransform parent)
    {
        for (var i = 0; i < Texts.Length; i++)
        {
            var tmp_Text = instance.GetComponentInChildren<TMP_Text>();
            tmp_Text.text = Texts[i];
            tmp_Text.ForceMeshUpdate();
            instance.GetComponent<RectTransform>().SetParent(parent);
        }
    }
    
                                                 
                      
}
public class EvidenceEdge : MonoBehaviour
{
    // 方便渲染特效？
    
}
public class EvidenceNode : MonoBehaviour
{
    // 一个节点，应该知道跟他相连的是谁。以及它的边是谁。
    public EvidenceNode[] NeighbourNodes;
    public EvidenceEdge[] NeighbourEdges;

    public void Connect()
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// 节点生产的地方
/// </summary>
public class EvidenceBoardInbox : MonoBehaviour
{

}

public enum Location
{
    None,
    别墅,
    医院
}

public enum Speaker
{
    None,
    李某,
}