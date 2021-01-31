﻿using System.Collections;
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

    private List<string> KeyList;
    private List<string> Items;

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

        KeyList = new List<string>();
        Items = new List<string>();
        CurrentRoom = StartingRoom;

        Items.Add("item");
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
                ProcessLastCommand(LastCommand);
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
                    if (ParcedCommand.Length < 2)
                    {
                        PrintMessage("wrong use of command, \"go\" needs a direction");
                        return;
                    }

                    GoToRoom(ParcedCommand[1]);
                    break;

                case "talk":
                    TalkToNPC();
                    break;

                case "download":
                    Download();
                    break;

                case "execute":
                    if (ParcedCommand.Length < 2)
                    {
                        PrintMessage("wrong use of command, \"execute\" needs two parameters");
                        return;
                    }

                    HandleExecute(ParcedCommand[1], ParcedCommand[2]);
                    break;
            }
        }
        else
        {
            PrintMessage("there is no such command");
        }
    }

    private void HandleExecute(string param1, string param2)
    {
        if (KeyList.Contains(param1))
        {
            OpenDoor(param2, param1);
        }
        else if (Items.Contains(param1))
        {
            if(param2 != CurrentRoom.RoomNPC.NPCname)
            {
                PrintMessage("there is nobody named " + param2 + " here");
                return;
            }

            NPC CurrentNPC = CurrentRoom.RoomNPC;
            PrintMessage(CurrentNPC.OnExecuteDialogue);

            if (CurrentNPC.HasItem())
            {
                Items.Add(CurrentNPC.ItemName);
            }
            else if (CurrentNPC.HasKey())
            {
                KeyList.Add(CurrentNPC.KeyName);
            }
        }
        else
        {
            PrintMessage("there is no such thing as " + param1);
        }

    }

    private void OpenDoor(string room, string key)
    {
        //get the desired room
        //unlock it

        switch (room)
        {
            case "up":
                HandleExecuteKeyOnDoor(CurrentRoom.UpRoom, key);
                break;

            case "down":
                HandleExecuteKeyOnDoor(CurrentRoom.DownRoom, key);
                break;

            case "right":
                HandleExecuteKeyOnDoor(CurrentRoom.RightRoom, key);
                break;

            case "left":
                HandleExecuteKeyOnDoor(CurrentRoom.LeftRoom, key);
                break;
        }
    }

    private void HandleExecuteKeyOnDoor(Room room, string key)
    {
        //in case the player does not have the key
        if (!KeyList.Contains(key))
        {
            PrintMessage("you don't have that");
            return;
        }

        if (room.IsLocked() && key == room.RequiredKeyName)
        {
            room.isLocked = false;
            PrintMessage("used " + key);
        }
    }

    private void Download()
    {
        if (CurrentRoom.HasDownload())
        {
            KeyList.Add(CurrentRoom.Download);
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
                    KeyList.Add(CurrentNPC.KeyName);
                    PrintMessage(CurrentNPC.NPCname + " " + "got key");
                }

                if(CurrentNPC.HasItem() && parsedOption == CurrentNPC.ItemLocation)
                {
                    KeyList.Add(CurrentNPC.ItemName);
                    PrintMessage(CurrentNPC.NPCname + " " + "got item");
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
            if (CurrentRoom.RoomNPC.DialogueOptions.Count == 0)
            {
                PrintMessage("you can't talk to " + CurrentRoom.RoomNPC.NPCname);
                return;
            }

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
                    HandleRoomTransition(CurrentRoom.UpRoom);
                    break;

                case "down":
                    HandleRoomTransition(CurrentRoom.DownRoom);
                    break;

                case "left":
                    HandleRoomTransition(CurrentRoom.LeftRoom);
                    break;

                case "right":
                    HandleRoomTransition(CurrentRoom.RightRoom);
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
        //else if (room.IsLocked() && Inventory.Contains(room.RequiredKeyName))
        //{
        //    PrintMessage("used " + room.RequiredKeyName);
        //    CurrentRoom = room;
        //}
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
