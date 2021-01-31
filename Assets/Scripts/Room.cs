using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public Room UpRoom;
    public Room DownRoom;
    public Room RightRoom;
    public Room LeftRoom;
    public bool isLocked;
    public string KeyName;
    public string RequiredKeyName;
    public string LockedDialogue;

    public string OpeningText;

    public string Download;
    public NPC RoomNPC;

    public bool CanExecute;
    public string ExecuteName;

    private bool HasBeenDowloaded;

    public bool HasRoomInDirection(string direction)
    {
        if (UpRoom == null && DownRoom == null && LeftRoom == null && RightRoom == null)
            return false;

        if(direction == "up" && UpRoom != null)
        {
            return true;
        }

        switch (direction)
        {
            case "up":

                if (UpRoom != null)
                    return true;

                break;

            case "down":

                if (DownRoom != null)
                    return true;

                break;

            case "right":

                if (RightRoom != null)
                    return true;

                break;

            case "left":

                if (LeftRoom != null)
                    return true;

                break;
        }

        return false;
    }

    public bool HasDownload()
    {
        return Download != "";
    }

    public bool HasNPC()
    {
        return RoomNPC != null;
    }

    public bool IsLocked()
    {
        return isLocked;
    }
}
