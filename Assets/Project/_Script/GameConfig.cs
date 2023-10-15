using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameConfig
{
	public enum COLLIDABLE_OBJECT
	{
		ENEMY = 0,
		OBSTACLE
	}

	public enum SO_TYPE
	{
		CHARACTER = 0,
		WEAPON,
		ENEMY,
		PET
	}

	public enum STAT_TYPE
	{
		HP,
		ATTACK_SPEED,
		DAMAGE,
		MOVE_SPEED,
		ATTACK_RANGE,
		EFFECT_SCOPE,
		BULLET_SPEED,
		DETECT_RANGE
	}

	public enum WEAPON
	{
		RIFLE = 0,
		GERNADETHROWER = 1,
		BOOM,
	}

	public enum CHARACTER
	{
		CHARACTER_DEFAULT = 0,
		CHARACTER_1,
		CHARACTER_2,
		CHARACTER_3
	}

	public enum ENEMY
	{
		ENEMY_DEFAULT = 0,

	}

	public enum PET
	{ 
		DRONE = 0,
		TURRET
	}

}

