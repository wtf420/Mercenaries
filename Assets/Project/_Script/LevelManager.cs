using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// [Serializable]
// public class DicSO
// {
//     [SerializeField] public GameConfig.SO_TYPE Type;
//     [SerializeField] public List<ScriptableObject> Stats;
// }

public enum GameMode
{
    Sweep,
    Survival,
    SearchAndDestroy
}

public class LevelManager : MonoBehaviour
{
    #region Fields & Properties
    public GameMode currentGameMode;
    public GameMode[] availableGameMode = { GameMode.Sweep, GameMode.Survival };
    public GameObject text;
    public GameObject characterSpawner;

    //[SerializeField]
    //public DataBank DataBank;

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    //public CharacterSO characterInfo;
    [SerializeField] Character character;

    [SerializeField] List<Enemy> enemies;

    [SerializeField] CameraController myCamera;
    #endregion

    #region Methods
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //DataBank = GetComponent<DataBank>();
    }

    void Start()
    {
        switch(GameManager.Instance.SelectedCharacter)
		{
            case GameConfig.CHARACTER.CHARACTER_1:
                character = Character1.Create(null, characterSpawner.transform.position);
                break;

            case GameConfig.CHARACTER.CHARACTER_2:
                character = Character2.Create(null, characterSpawner.transform.position);
                break;

            case GameConfig.CHARACTER.CHARACTER_3:
                character = Character3.Create(null, characterSpawner.transform.position);
                break;

            case GameConfig.CHARACTER.CHARACTER_4:
                character = Character4.Create(null, characterSpawner.transform.position);
                break;
        }
        //CharacterSO c = GameManager.Instance.selectedCharacter;
        //GameObject charactergameobject = Instantiate(c.characterPrefab, characterSpawner.transform.position, characterSpawner.transform.rotation);
        //character = charactergameobject.GetComponent<Character>();

        enemies.AddRange(GameObject.FindObjectsOfType<Enemy>());
        myCamera = Camera.main.gameObject.GetComponent<CameraController>();

        character.Initialize();
        myCamera.Initialize(character.gameObject);
        foreach (var enemy in enemies)
        {
            enemy.Initialize();
        }

        text.SetActive(false);
    }

    private void FixedUpdate()
    {
        RemoveDeathEnemy();

        if (!character.IsDeath)
            character.UpdateCharacter(enemies);
        myCamera.UpdateCamera();
        foreach (var enemy in enemies)
        {
            if (!enemy.IsDead)
                enemy.UpdateEnemy(character);
        }
        if (WinCondition())
        {
            text.SetActive(true);
            text.GetComponent<Text>().text = "WIN";
        } else if (LoseCondition())
        {
            text.SetActive(true);
            text.GetComponent<Text>().text = "LOSE";
        }
    }

    private void LateUpdate()
    {

    }

    //public SO_EnemyDefault GetStats(Enemy aEnemy)
    //{
    //    //Debug.Log($"Type: {type}, Index {index}");
    //    return DataBank.EnemyStats.Find(element => element.enemy.GetType() == aEnemy.GetType()).Stats;
    //}

    private void RemoveDeathEnemy()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].IsDead)
            {
                if (enemies[i].deleteUponDeath)
                    Destroy(enemies[i].gameObject);
                enemies.RemoveAt(i);
            }
        }
    }

    virtual public bool WinCondition()
    {
        switch (currentGameMode)
        {
            default:
                {
                    if (enemies.Count == 0)
                        return true; else
                        return false;
                }
        }
    }

    virtual public bool LoseCondition()
    {
        switch (currentGameMode)
        {
            default:
                {
                    return character.IsDeath;
                }
        }
    }
    #endregion
}
