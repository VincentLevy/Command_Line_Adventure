using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string NPCname;
    public List<string> DialogueOptions;
    public List<string> DialogueResponses;

    public string key;
    [Header("put here what dialog option will give the key")]
    public int KeyLocation;

    public bool HasKey()
    {
        return key != null;
    }
}
