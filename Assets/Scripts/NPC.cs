using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string NPCname;
    public List<string> DialogueOptions;
    public List<string> DialogueResponses;

    //public bool CanExecute;
    public string ExecutableName;
    public string OnExecuteDialogue;
    public string ItemName;
    public string KeyName;

    //public string key;
    [Header("put here what dialog option will give the key")]
    public int KeyLocation;
    public int ItemLocation;

    public bool HasKey()
    {
        return KeyName != "";
    }

    public bool HasItem()
    {
        return ItemName != "";
    }

    //execute [item] [npc]
    //search item list
    //print reaction
    //add key or item to the corresponding list
}
