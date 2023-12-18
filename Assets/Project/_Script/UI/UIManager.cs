using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<Action<float>> VolumeChanges;

    public float Volume
	{
        get => _volume;

        set
		{
            _volume = value;
            foreach (var volumeChange in VolumeChanges)
			{
                volumeChange?.Invoke(_volume);
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

        VolumeChanges = new List<Action<float>>();
        UserInterfaces = new List<IUserInterface>();
        MainMenu.Create();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (GameManager.Instance.IsInPlayScene())
			{
				if (UserInterfaces.Count > 1)
				{
					RemoveAllUIInPlayGame();
					Time.timeScale = 1f;
					return;
				}
			}
			else
			{
				return;
			}

			var pause = UserInterfaces.Find(element => element.Type == UI.PAUSE);
            if (pause == null)
            {
                PauseMenu.Create();
                Time.timeScale = 0;
            }
		}
    }

    public void FindMainMenuAudio()
	{
        _mainMenuAudio = GameObject.Find("Audio")?.GetComponent<AudioSource>();
        if (_mainMenuAudio) 
		{
            _mainMenuAudio.volume = Volume;
		}
    }

    public void SubscribeVolumeEvent(Action<float> action)
	{
        VolumeChanges.Add(action);
	}        

    public void UnSubscribeVolumeEvent(Action<float> action)
	{
        VolumeChanges.Remove(action);
	}

    public IUserInterface GetUI(UI type) => UserInterfaces.Find(element => element.Type == type);

    private void RemoveAllUIInPlayGame()
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
