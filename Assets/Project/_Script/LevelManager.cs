using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    [SerializeField] Character character;

    [SerializeField] List<Enemy> enemies;

    [SerializeField] CameraController myCamera;
    #endregion

    #region Methods
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        enemies.AddRange(GameObject.FindObjectsOfType<Enemy>());
        myCamera = Camera.main.gameObject.GetComponent<CameraController>();
    }

    private void FixedUpdate()
    {
        RemoveDeathEnemy();

        if (!character.IsDeath)
            character.UpdateCharacter(enemies);
        myCamera.UpdateCamera();
        foreach (var enemy in enemies)
        {
            enemy.UpdateEnemy(character);
        }
    }

    private void LateUpdate()
    {

    }

    private void RemoveDeathEnemy()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i].IsDead)
            {
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
                    return true;
                }
        }
    }

    virtual public bool LoseCondition()
    {
        switch (currentGameMode)
        {
            default:
                {
                    return false;
                }
        }
    }
    #endregion
}
