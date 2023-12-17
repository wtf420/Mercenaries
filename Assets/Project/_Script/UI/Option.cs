using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Option: MonoBehaviour, IUserInterface
{
    #region Fields and Properties
    [SerializeField] Scrollbar _volume;
    [SerializeField] Button _keyboard;
    [SerializeField] Button _back;

    public UI Type { get; set; }

    public UI PreviousUI { get; set; }
    #endregion

    public static Option Create(Transform parent = null)
    {
        Option option = Instantiate(Resources.Load<Option>("_Prefabs/UI/Option"), parent);
        option.Type = UI.OPTION;

        UIManager.Instance.UserInterfaces.Add(option);
        return option;
    }

    private void OnEnable()
    {
        _volume.value = UIManager.Instance.Volume;
        _volume.onValueChanged.AddListener(VolumeChange);

        _keyboard.onClick.AddListener(KeyboardSetting);
        _back.onClick.AddListener(Back);

        if (GameManager.Instance.IsInPlayScene())
        {
            _keyboard.enabled = false;
            Destroy(_keyboard.gameObject);
        }
    }

    private void VolumeChange(float volume)
    {
        UIManager.Instance.Volume = volume;
    }

    private void KeyboardSetting()
	{
        KeyboardMenu.Create();
        Destroy(gameObject);
	}

    private void Back()
    {
        switch(PreviousUI)
		{
            case UI.MAIN_MENU:
                MainMenu.Create();
                break;

            case UI.PAUSE:
                PauseMenu.Create();
                break;
		}
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _volume.onValueChanged.RemoveListener(VolumeChange);
        _keyboard.onClick.RemoveListener(KeyboardSetting);
        _back.onClick.RemoveListener(Back);

        UIManager.Instance.UserInterfaces.Remove(this);
    }
}
