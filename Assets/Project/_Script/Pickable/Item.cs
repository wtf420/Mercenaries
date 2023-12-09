using UnityEngine;

public class Item : MonoBehaviour, IPickable
{
	#region Fields & Properties
	[SerializeField] private GameConfig.PICKABLE_TYPE _type;

	[Header("BUFF")]
	[SerializeField] private GameConfig.BUFF _buffType;
	[SerializeField] private float _statBuff;

	[Header("ITEM")]
	[SerializeField] private IWeapon _weapon;

	public GameConfig.PICKABLE_TYPE Type 
	{
		get => _type;
		set => _type = value;
	}

	#endregion

	#region Methods 

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("ENEMY"))
		{
			return;
		}

		Character character = other.gameObject.GetComponent<Character>();
		if (!character)
		{
			return;
		}

		switch(_type)
		{
			case GameConfig.PICKABLE_TYPE.BUFF:
				character.TakenBuff(_buffType, _statBuff);
				break;

			case GameConfig.PICKABLE_TYPE.ITEM:
				character.TakenWeapon(_weapon);
				break;
		}

		Destroy(gameObject);
	}
	#endregion
}

