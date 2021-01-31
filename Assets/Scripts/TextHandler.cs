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
    private List<string> CommandList;

    [SerializeField]
    private Room StartingRoom;
    private Room CurrentRoom;

    private List<string> Inventory;

    private bool IsTalking;

    public float offset = -1;

    // Start is called before the first frame update
    void Start()
    {
        CommandList = new List<string>();
        CommandList.Add("help");
        CommandList.Add("go");
        CommandList.Add("talk");
        CommandList.Add("download");
        CommandList.Add("execute");
        CommandList.Add("objects");

        Inventory = new List<string>();
        CurrentRoom = StartingRoom;
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

        if(ParcedCommand[0] == "leave")
        {
            IsTalking = false;
            return;
        }

        if (IsTalking && ParcedCommand.Length != 0)
        {
            HandleTalk(ParcedCommand[0]);
            return;
        }

        //if (ParcedCommand.Length != 2)
        //{
        //    PrintMessage("incorrect use of command");
        //    return;
        //}

        string CurrentCommand = ParcedCommand[0];
        //string CurrentArgument = ParcedCommand[1];

        if (CommandList.Contains(ParcedCommand[0]))
        {
            switch (CurrentCommand)
            {
                case "help":

                    break;

                case "go":
                    //GoToRoom(CurrentArgument);
                    GoToRoom(ParcedCommand[1]);
                    break;

                case "talk":
                    TalkToNPC();
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

    private void HandleTalk(string option)
    {
        int parsedOption;
        if(int.TryParse(option, out parsedOption))
        {
            if(parsedOption < 0 || parsedOption >= CurrentRoom.RoomNPC.DialogueOptions.Count)
            {
                PrintMessage("there is no such option");
            }
            else
            {
                PrintMessage(CurrentRoom.RoomNPC.DialogueResponses[parsedOption]);
            }
        }
        else
        {
            PrintMessage("insert a number or say \"leave\" ");
        }

    }

    private void TalkToNPC()
    {
        if (CurrentRoom.HasNPC())
        {
            IsTalking = true;

            int i = 0;
            foreach (string s in CurrentRoom.RoomNPC.DialogueOptions)
            {
                ShowOptions("[" + i++ + "]" + " " + s);
            }
        }
        else
        {
            PrintMessage("there is nobody to talk here to");
        }
    }

    private void GoToRoom(string direction)
    {
        if (CurrentRoom.HasRoomInDirection(direction))
        {
            switch (direction)
            {
                case "up":
                    CurrentRoom = CurrentRoom.UpRoom;
                    break;

                case "down":
                    CurrentRoom = CurrentRoom.DownRoom;
                    break;

                case "left":
                    CurrentRoom = CurrentRoom.LeftRoom;
                    break;

                case "right":
                    CurrentRoom = CurrentRoom.RightRoom;
                    break;
            }

            PrintMessage(CurrentRoom.OpeningText);
        }
        else
        {
            PrintMessage("there is no such room");
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

    private void ShowOptions(string options)
    {
        HistoryText.text += options + "\n";
    }
}
