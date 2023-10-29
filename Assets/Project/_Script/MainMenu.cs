using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetCharacter();
    }

    public void SetCharacter(int index = 0)
    {
        switch (index)
        {
            case 0:
                {
                    GameManager.Instance.SelectedCharacter = GameConfig.CHARACTER.CHARACTER_1;
                    break;
                }
            case 1:
                {
                    GameManager.Instance.SelectedCharacter = GameConfig.CHARACTER.CHARACTER_2;
                    break;
                }
            default:
                {
                    GameManager.Instance.SelectedCharacter = GameConfig.CHARACTER.CHARACTER_3;
                    break;
                }
        }

    }

    public void Begin(string levelname)
    {
        GameManager.Instance.BeginLevel(levelname);
    }
}
