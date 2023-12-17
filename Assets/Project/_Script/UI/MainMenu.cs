using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IUserInterface
{
    #region Fields and Properties
    [SerializeField] TMP_Dropdown _characterSelecting;
    [SerializeField] Button _startGame;
    [SerializeField] Button _option;

    public UI Type { get; set; }
    #endregion
    public static MainMenu Create(Transform parent = null)
	{
        MainMenu menu = Instantiate(Resources.Load<MainMenu>("_Prefabs/UI/MainMenu"), parent);
        menu.Type = UI.MAIN_MENU;

        UIManager.Instance.UserInterfaces.Add(menu);
        return menu;
    }

	private void OnEnable()
    {
        _characterSelecting.ClearOptions();
        _characterSelecting.AddOptions(new List<TMP_Dropdown.OptionData>()
        {
            new TMP_Dropdown.OptionData(typeof(Character).Name),
            new TMP_Dropdown.OptionData(typeof(Character1).Name),
            new TMP_Dropdown.OptionData(typeof(Character2).Name),
            new TMP_Dropdown.OptionData(typeof(Character3).Name),
            new TMP_Dropdown.OptionData(typeof(Character4).Name),
        });
        _characterSelecting.onValueChanged.AddListener(SetCharacter);
        
        _startGame.onClick.AddListener(Begin);
        _option.onClick.AddListener(OpenOption);

        UIManager.Instance.FindMainMenuAudio();
	}

	private void SetCharacter(int indexSelecting)
    {
        GameManager.Instance.SelectedCharacter = (GameConfig.CHARACTER)indexSelecting;
    }

    private void Begin()
    {
        GameManager.Instance.BeginLevel("AIWithSpawn");
    }

    private void OpenOption()
    {
        var option = Option.Create();
        option.PreviousUI = Type;

        Destroy(gameObject);
    }

	private void OnDisable()
	{
        _characterSelecting.onValueChanged.RemoveListener(SetCharacter);
        _startGame.onClick.RemoveListener(Begin);
        _option.onClick.RemoveListener(OpenOption);
        UIManager.Instance.UserInterfaces.Remove(this);
	}
}
