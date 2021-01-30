using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TextHandler : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI HistoryText;
    [SerializeField]
    private TMP_InputField InputText;
    [SerializeField]
    private Scrollbar Scrollbar;
    [SerializeField]
    private EventSystem EventSystem;
    [SerializeField]
    private List<string> CommandList;

    public float offset = -1;

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
            string LastCommand = InputText.text;
            PrintMessage(LastCommand);
            InputText.text = "";
            Scrollbar.value = offset;

            if(LastCommand != "")
                ProcessLastCommand(LastCommand.ToLower());
        }

        //constantly selects the input field
        EventSystem.current.SetSelectedGameObject(InputText.gameObject, null);
        InputText.OnPointerClick(new PointerEventData(EventSystem.current));
    }

    private void ProcessLastCommand(string lastCommand)
    {
        //looks bad I know
        char[] charrarray = { ' ' };
        string[] ParcedCommand = lastCommand.Split(charrarray);

        if (ParcedCommand.Length != 0 && ParcedCommand[0] == "help")
        {
            DisplayHelpMessage();
            return;
        }

        if (ParcedCommand.Length != 2)
        {
            PrintMessage("incorrect use of command");
            return;
        }

        string CurrentCommand = ParcedCommand[0];
        string CurrentArgument = ParcedCommand[1];

        if (CommandList.Contains(ParcedCommand[0]))
        {
            switch (CurrentCommand)
            {
                case "help":

                    break;

                case "go":

                    break;

                case "talk":

                    break;

                case "download":

                    break;

                case "execute":

                    break;

                case "objects":

                    break;
            }
        }
        else
        {
            PrintMessage("there is no such command");
        }
    }

    private void DisplayHelpMessage()
    {
        PrintMessage("Fuck you");
    }

    private void PrintMessage(string message)
    {
        HistoryText.text += "C:\\Users\\Bonzo>" + message + "\n";
    }
}
