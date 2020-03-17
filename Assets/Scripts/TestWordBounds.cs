using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestWordBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //
        var temp_Text = GetComponentInChildren<TMP_Text>();
        var parent = this;

        Debug.Log($"panelTransform: {parent.transform.position}, panelRect: {parent.GetComponent<RectTransform>().anchoredPosition}");
        Debug.Log($"textTransform: {temp_Text.transform.position}, textRect: {temp_Text.rectTransform.anchoredPosition}, textBounds: {temp_Text.textBounds}");
        // 测试是否正常刷新
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
