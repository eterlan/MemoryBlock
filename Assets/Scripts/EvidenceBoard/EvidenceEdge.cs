using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class EvidenceEdge
{
    // 方便渲染特效？
    // 我需要储存node，以及知道每个node在UILineRenderer的Point数组里面对应的Index
    public readonly Dictionary<EvidenceNode, int> NodeIndex = new Dictionary<EvidenceNode, int>();
    public UILineRenderer LineRenderer;

    public EvidenceEdge(UILineRenderer lineRenderer)
    {
        LineRenderer = lineRenderer;
    }
}