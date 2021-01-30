using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    //[SerializeField]
    //private TextMeshProUGUI InputText;

    [SerializeField]
    private TextMeshProUGUI HistoryText;

    [SerializeField]
    private TMP_InputField InputText;

    // Start is called before the first frame update
    void Start()
    {
        InputText.text = "hello world";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HistoryText.text += "C:\\Users\\Bonzo>" + InputText.text + "\n";
            InputText.text = "";
        }
    }
}
