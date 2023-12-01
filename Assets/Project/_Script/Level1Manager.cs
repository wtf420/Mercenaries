using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelManager
{
    override public bool WinCondition()
    {
        return false;
        // switch (currentGameMode)
        // {
        //     default:
        //         {
        //             if (enemiesLeft == 0)
        //                 return true;
        //             else
        //                 return false;
        //         }
        // }
    }
}
