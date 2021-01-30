using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Room UpRoom;
    public Room DownRoom;
    public Room RightRoom;
    public Room LeftRoom;

    public List<string> ObjectList;

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
}
