using System;
using UnityEngine;

[Serializable]
public struct WeaponParameters
{
	public int rateOfFire;
	public int clipSize;
	public int damage;
}

[Serializable]
public struct GameParameters
{
	public int targetsToKill;
}

[Serializable]
public struct TargetParameters
{
	public int health;
}

[Serializable]
public class Parameters
{
	public WeaponParameters WeaponParameters;
	public GameParameters GameParameters;
	public TargetParameters TargetParameters;
}