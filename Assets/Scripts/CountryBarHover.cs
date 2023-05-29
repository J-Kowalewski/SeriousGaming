using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CountryBarHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    [Range(0,1)]
    private float factor =.5f;

    private Color color;
    private GameObject bar;
    void Start()
    {
        /*bar = transform.Find("Bar").gameObject;
        color = bar.GetComponent<Image>().color;*/
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        /*float r = color.r * factor;
        float g = color.g * factor;
        float b = color.b * factor;

        bar.GetComponent<Image>().color = new Color(r,g,b);*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /*bar.GetComponent<Image>().color = color;*/
    }
}
