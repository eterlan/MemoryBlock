using System;
using UnityEngine;
using UnityEngine.Events;

public class NodeCreator : MonoBehaviour
{
    public event Action OnButtonDown = delegate {  };

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnButtonDown.Invoke();
        }
    }
}
