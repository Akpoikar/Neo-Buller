using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    TextMeshProUGUI Text;
    Color def;
    private void Start()
    {
        Text = GetComponentInChildren<TextMeshProUGUI>();
        def = Text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Text.color = Color.red;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Text.color = def;
    }
}
