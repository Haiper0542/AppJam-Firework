using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIButtonCtrl : MonoBehaviour {

    public UnityEvent Press;
    public UnityEvent PressDown;

    public Color NormalColor;
    public Color HighlightColor;
    public Color PressedColor;

    public Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Highlight()
    {
        _renderer.material.color = HighlightColor;
    }

    public void UnHighlight()
    {
        _renderer.material.color = NormalColor;
    }

    public void Pressed()
    {
        _renderer.material.color = PressedColor;
        Press.Invoke();
    }
}
