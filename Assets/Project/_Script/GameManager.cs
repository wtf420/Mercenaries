using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Fields & Properties

    public GameConfig.CHARACTER SelectedCharacter = GameConfig.CHARACTER.CHARACTER_DEFAULT;

    public static GameManager Instance { get; protected set; }
    #endregion

    #region Methods
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
        	Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);
    }

    public void BeginLevel(string levelname)
    {
        SceneManager.LoadScene(levelname);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    #endregion
}
