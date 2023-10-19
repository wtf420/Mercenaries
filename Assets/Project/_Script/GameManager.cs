using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DicSO
{
	[SerializeField] public GameConfig.SO_TYPE Type;
	[SerializeField] public List<ScriptableObject> Stats;
}

public class GameManager: MonoBehaviour
{
	#region Fields & Properties
	private static GameManager instance;
	public static GameManager Instance
	{
		get => instance;
		private set => instance = value;
	}

	public List<DicSO> SO_Stats;

	[SerializeField] Character character;

	[SerializeField] List<Enemy> enemies;

	[SerializeField] CameraController myCamera;
	#endregion

	#region Methods
	private void Awake()
	{
		if (instance == null)
			instance = this;
		else
			Destroy(gameObject);

		character.Initialize();
		myCamera.Initialize(character.gameObject);

		foreach(var enemy in enemies)
		{
			enemy.Initialize();
		}	
	}

	public ScriptableObject GetStats(GameConfig.SO_TYPE type, int index = 0)
	{
		//Debug.Log($"Type: {type}, Index {index}");
		return SO_Stats.Find(element => element.Type == type).Stats[index];
	}
	#endregion
}
