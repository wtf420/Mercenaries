using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UI
{
    MAIN_MENU = 0,
    PAUSE,
    WIN,
    LOSE,
    IN_GAME,
    OPTION,
    KEY,
    AUDIO
}

public class UIManager : MonoBehaviour
{
    #region Fields & Properties
    public static UIManager Instance { get; private set; }

    public List<IUserInterface> UserInterfaces;

    private List<AudioSource> _volumeChanges;

    public float Volume
	{
        get => _volume;

        set
		{
            _volume = value;
            foreach (var volumeChange in _volumeChanges)
			{
                volumeChange.volume = _volume;
			}

            if(_mainMenuAudio)
            {
                _mainMenuAudio.volume = _volume;
            }
        }
        
	}
    private float _volume = 1f;

    private AudioSource _mainMenuAudio;
    #endregion
    private void Awake()
	{
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(Instance);

        DataPersistenceManager.Instance.LoadData();

        _volumeChanges = new List<AudioSource>();
        UserInterfaces = new List<IUserInterface>();
        if (SceneManager.GetActiveScene().name == "Main Menu")
            MainMenu.Create();
    }

    public void ResumeGame()
    {
        RemoveAllUIInPlayGame();
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        PauseMenu.Create();
        Time.timeScale = 0;
    }

    public void FindMainMenuAudio()
	{
        _mainMenuAudio = GameObject.Find("Audio")?.GetComponent<AudioSource>();
        if (_mainMenuAudio) 
		{
            _mainMenuAudio.volume = Volume;
		}
    }

    public void SubscribeVolumeEvent(AudioSource audio)
	{
        _volumeChanges.Add(audio);

        // set volume for all
        audio.volume = _volume;
	}        

    public void UnSubscribeVolumeEvent(AudioSource audio)
	{
        _volumeChanges.Remove(audio);
	}

    public IUserInterface GetUI(UI type) => UserInterfaces.Find(element => element.Type == type);

    public void RemoveAllUIInPlayGame()
	{
        for (int i = UserInterfaces.Count - 1; i >= 0; i--)
		{
            if (UserInterfaces[i].Type == UI.IN_GAME)
			{
                return;
			}

            var ui = (UserInterfaces[i] as MonoBehaviour).transform;
            if (ui == null)
			{
                UserInterfaces.RemoveAt(i);
                return;
			}

            Destroy(ui.gameObject);
        }
    }
}
