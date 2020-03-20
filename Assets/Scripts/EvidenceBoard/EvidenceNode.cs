using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;

public class EvidenceNode : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    public static RectTransform DraggingObject;
    // 一个节点，应该知道跟他相连的是谁。以及它的边是谁。
    public List<EvidenceNode> NeighbourNodes = new List<EvidenceNode>();
    public List<EvidenceEdge> NeighbourEdges = new List<EvidenceEdge>();
    private Vector2 restorePosition;
    private RectTransform inBox;
    [SerializeField, Sirenix.OdinInspector.ReadOnly]
    private bool InInbox;
    // 是否是能把这些信息放在inbox?

    private void Awake()
    {
        // Inbox是线索收集箱,别改这个名字.
        inBox = GameObject.Find("Inbox").transform as RectTransform;
        InInbox = InOutDetection(transform.position, inBox);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        restorePosition = transform.position;
        DraggingObject = transform as RectTransform;
        if (InInbox)
        {
            inBox.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        if (NeighbourEdges.Count > 0)
        {
            // #Todo 当有临线的时候, 更新线的位置
            UpdateLine(this);
        }
    }

    private void UpdateLine(EvidenceNode movingNode)
    {
        for (int i = 0; i < NeighbourEdges.Count; i++)
        {
            var edge = NeighbourEdges[i];
            if (edge.NodeIndex.TryGetValue(movingNode, out var index))
            {
                var posOnBoard = inBox.parent.InverseTransformPoint(movingNode.transform.position);
                Debug.Log(posOnBoard);
                edge.LineRenderer.Points[index] = posOnBoard;
                edge.LineRenderer.SetAllDirty();
            }
            else
            {
                throw new System.IndexOutOfRangeException("Edge doesn't contains this node");
            }
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("endDrag");
        // 转换成为本地坐标系
        // 当不在里面的时候,
        //Debug.Log($"self: {transform.position}, Inbox: {transform.parent.position}");
        if (InOutDetection(transform.position, inBox))
        {
            Debug.Log("in");
            InInbox = true;
            transform.SetParent(inBox);
            // 没法知道layout group会把它扔到哪儿,因此没法tween,好在推理的话追求顺手,动画应该不必要
            //transform.DOMove(restorePosition, 0.5f).OnComplete(OnEndOfTween);
            
            // #Todo 当被扔进来的时候应该解开所有edge
        }
        else
        {
            InInbox = false;
            transform.SetParent(inBox.parent);
        }
        inBox.GetComponent<HorizontalLayoutGroup>().enabled = true;
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private bool InOutDetection(Vector3 detectPointInWorld, RectTransform detector)
    {
        var local = inBox.InverseTransformPoint(detectPointInWorld);
        return detector.rect.Contains(local);
    }


    // 当被一个节点砸到的时候

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log($"drop:{GetComponentInChildren<TMP_Text>().text}");
        Connect();
    }

    public void Connect()
    {
        var draggingNode = DraggingObject.GetComponent<EvidenceNode>();
        // #Todo 恢复原始位置
        var restorePos = draggingNode.restorePosition;
        DraggingObject.transform.position = restorePos;
        if (NeighbourNodes.Contains(draggingNode))
        {
            // Already linked together
            Debug.Log("重复添加无效");
            UpdateLine(draggingNode);
            return;
        }
        var board = inBox.parent;
        
        // 之后用prefab
        // 新建UILineRenderer
        var lineObject = new GameObject();
        var lineRenderer = lineObject.AddComponent<UILineRenderer>();
        var edge = new EvidenceEdge(lineRenderer);
        
        lineRenderer.GetComponent<Transform>().SetParent(board);
        Debug.Log(lineRenderer.canvas);

        var p0 = board.InverseTransformPoint(transform.position);
        var p1 = board.InverseTransformPoint(restorePos);
        lineRenderer.Points = new Vector2[]{p0, p1};
        
        // 初始化, 或许用Prefab以后就不用了. 也不用AddComponent了.
        var rectTransform = lineRenderer.rectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;


        // #Todo 当添加新的临线后, 应该将值注册到数组中
        edge.NodeIndex.Add(GetComponent<EvidenceNode>(), 0);
        edge.NodeIndex.Add(DraggingObject.GetComponent<EvidenceNode>(), 1);
        NeighbourEdges.Add(edge);
        NeighbourNodes.Add(draggingNode);
        draggingNode.NeighbourEdges.Add(edge);
        draggingNode.NeighbourNodes.Add(this);
        // #TEST 
        // #Todo 不应该能重复添加
        // #Todo 右键删除连线
    }

    private void DrawNewLine()
    {
        
    }
}