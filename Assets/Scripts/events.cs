using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class events : MonoBehaviour
{
    [SerializeField]
    private TMP_Text textDesc_title;
    [SerializeField]
    private TMP_Text textDesc_text;

    [SerializeField]
    private TMP_Text textSide_title;
    [SerializeField]
    private TMP_Text textSide_text;

    private void Start()
    {
        // Set placeholders incase
        if (textDesc_title != null) textDesc_title.text = "PLACEHOLDER";
        if (textDesc_text != null) textDesc_text.text = "PLACEHOLDER";
        if (textSide_title != null) textSide_title.text = "PLACEHOLDER";
        if (textSide_text != null) textSide_text.text = "PLACEHOLDER";
    }

    void setDescTitle(string text)
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
