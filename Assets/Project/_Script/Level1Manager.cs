using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : LevelManager
{
    public int condition = 0;
    [SerializeField] GameObject Jumppad, Door;

    override public bool WinCondition()
    {
        return false;
    }

    //Drastic measures, should not be final
    protected override void Update()
    {
        base.Update();
        if (condition >= 2)
            Jumppad.gameObject.SetActive(true);
        if (condition >= 4)
            Door.gameObject.SetActive(true);
    }

    //Drastic measures, should not be final
    public void PlusCondition()
    {
        condition++;
    }
}
