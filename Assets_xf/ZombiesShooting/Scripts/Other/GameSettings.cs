using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;

/* Description 

This global setting behavior of the project. (Asset/Resources/Settings) 
variables of the script used in many scripts play. Singolton ideal for such an implementation.
Singolton ideal for such an implementation.

Description */
[CreateAssetMenu(menuName = "MySubMenu/Create GameSettings")]
public class GameSettings : ScriptableObject
{
	public static GameSettings instance;

	public static GameSettings In
	{
		get
		{
			if (instance == null)
				instance = Resources.Load("Settings") as GameSettings;
			return instance;
		}
	}

	public int toolbarSelectedIndex;
	// a control box is selected and displayed

	// Controller Type
	public ControllerType controllerType;

	public enum ControllerType
	{
		Hardware,
		Touch
	}
	// keyboard or mobile control
	// player settings
	public bool immortality;
	public float speed = 15;
	public float sliding_speed = 1.5f;
	// sliding velocity player walls
	[Range(0.2f, 3f)] 
	public float shot_cooldown = 0.8f;
	// bullets rate
	public int bullet_speed = 25;
	// Do not forget that the speed of bullets should be higher than the speed of movement!
	public int rush_speed_player = 1200;
	public float rush_cooldown = 2;
	// time rush action
	public float time_slowdown = 2;
	// the duration of the debuff slowing down after use rush
	public float slowdown_speed = 2.5f;
	// how many times the speed slows down from the debuff
	public float time_bonus = 10;

	public int field_size = 1;
	public int amount_bonuses = 1;
	// How many bonuses to spawn in the playing field?
	public float min_sek_respawn = 1;
	//respawn bonuses
	public float max_sek_respawn = 2;
	//respawn bonuses


	[Range(1f, 100f)]public int amount_sparks = 100;
	public bool sparks_OCS = true;
	//on off Photon.Instantiate sparks in OnCollisionStay2D
	public bool sparks_OCE = true;
	//on off Photon.Instantiate sparks in OnCollisionEnter2D

	public int amount_clones;
	public List <bool> atacking = new List<bool>(new bool[20]);
	// 20 = The maximum possible number of players in the room of the free PUN account
	public List <bool> moving = new List<bool>(new bool[20]);
	// 20 = The maximum possible number of players in the room of the free PUN account
	public List<bool> rushing = new List<bool>(new bool[20]);
	// 20 = The maximum possible number of players in the room of the free PUN account
	public bool rush_all;
	public bool atack_all;
	public bool move_all;
}