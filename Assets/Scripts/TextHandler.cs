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
                //case "help":

                //    break;

                case "go":
                    //GoToRoom(CurrentArgument);
                    GoToRoom(ParcedCommand[1]);
                    break;

                case "talk":
                    TalkToNPC();
                    break;

                case "download":
                    Download();
                    break;

                case "execute":
                    HandleExecute();
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

    private void HandleExecute()
    {
        
    }

    private void Download()
    {
        if (CurrentRoom.HasDownload())
        {
            Inventory.Add(CurrentRoom.Download);
            PrintMessage("you have downloaded " + CurrentRoom.Download);
        }
        else
        {
            PrintMessage("you can't download anything here");
        }
    }

    private void HandleTalk(string option)
    {
        int parsedOption;
        if(int.TryParse(option, out parsedOption))
        {
            NPC CurrentNPC = CurrentRoom.RoomNPC;

            if(parsedOption < 0 || parsedOption >= CurrentNPC.DialogueOptions.Count)
            {
                PrintMessage("there is no such option");
            }
            else
            {
                PrintMessage(CurrentNPC.NPCname + ": " + CurrentNPC.DialogueResponses[parsedOption]);
                if (CurrentNPC.HasKey() && parsedOption == CurrentNPC.KeyLocation)
                {
                    Inventory.Add(CurrentNPC.key);
                    PrintMessage(CurrentNPC.NPCname + " " + "got key");
                }
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
                    if (!CurrentRoom.UpRoom.IsLocked())
                    {
                        CurrentRoom = CurrentRoom.UpRoom;
                    }
                    else
                    {
                        PrintMessage(CurrentRoom.UpRoom.LockedDialogue);
                    }
                    break;

                case "down":
                    if (!CurrentRoom.DownRoom.IsLocked())
                    {
                        CurrentRoom = CurrentRoom.DownRoom;
                    }
                    else
                    {
                        PrintMessage(CurrentRoom.DownRoom.LockedDialogue);
                    }
                    break;

                case "left":
                    //    //if it's not locked
                    //    if (!CurrentRoom.LeftRoom.IsLocked())
                    //    {
                    //        CurrentRoom = CurrentRoom.LeftRoom;
                    //    }
                    //    //if the player has the key and the door is locked
                    //    else if(CurrentRoom.LeftRoom.IsLocked() && Inventory.Contains(CurrentRoom.LeftRoom.RequiredKeyName))
                    //    {
                    //        CurrentRoom = CurrentRoom.LeftRoom;
                    //    }
                    //    //if it's locked and the player doesn't have the key
                    //    else
                    //    {
                    //        PrintMessage(CurrentRoom.LeftRoom.LockedDialogue);
                    //    }
                    HandleRoomTransition(CurrentRoom.LeftRoom);
                    break;

                case "right":
                    if (!CurrentRoom.RightRoom.IsLocked())
                    {
                        CurrentRoom = CurrentRoom.RightRoom;
                    }
                    else
                    {
                        PrintMessage(CurrentRoom.RightRoom.LockedDialogue);
                    }
                    break;
            }

            PrintMessage(CurrentRoom.OpeningText);
        }
        else
        {
            PrintMessage("there is no such room");
        }
    }


    private void HandleRoomTransition(Room room)
    {
        if (!room.IsLocked())
        {
            CurrentRoom = room;
        }
        //if the player has the key and the door is locked
        else if (room.IsLocked() && Inventory.Contains(room.RequiredKeyName))
        {
            CurrentRoom = room;
        }
        //if it's locked and the player doesn't have the key
        else
        {
            PrintMessage(room.LockedDialogue);
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
