using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class DynamicButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI buttonText;
    
    public string Id;

    private List<Action<string>> listeners = new List<Action<string>>();

    public string Text
    {
        get => buttonText.text;
        set => buttonText.text = value;
    }

    private void Awake()
    {
        if (button == null) button = GetComponent<Button>();
        if (buttonText == null) buttonText = GetComponentInChildren<TextMeshProUGUI>();

        button.onClick.AddListener(OnClickInternal);
    }

    private void OnClickInternal()
    {
        foreach (var listener in listeners)
        {
            listener?.Invoke(Id);
        }
    }
    
    public void AddListener(Action<string> callback)
    {
        if (callback != null && !listeners.Contains(callback))
        {
            listeners.Add(callback);
        }
    }
    
    public void RemoveListener(Action<string> callback)
    {
        if (callback != null && listeners.Contains(callback))
        {
            listeners.Remove(callback);
        }
    }
    
    public void ClearListeners()
    {
        listeners.Clear();
    }
}