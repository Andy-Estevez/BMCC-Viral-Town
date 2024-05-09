using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OccurrencesPreFab : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textDesc_title;
    [SerializeField]
    private TMP_Text textDesc_text;

    [SerializeField]
    private TMP_Text textSide_title;
    [SerializeField]
    private TMP_Text textSide_text;

    public void setDescTitle(string text)
    {
        textDesc_title.text = text;
    }
    void setDescText(string text)
    {
        textDesc_text.text = text;
    }

    void setSideTitle(string text)
    {
        textSide_title.text = text;
    }
    void setSideText(string text)
    {
        textSide_text.text = text;
    }

    public void onClick_closeButton()
    {
        Destroy(gameObject);
    }
}
