using System;
using UnityEngine;

public class CombatTurnController : SceneSingleton<CombatTurnController>
{

	public const float TURN_DURATION = 6f;
	
	private void Start()
	{
		PlayerCombatController.Instance.StartTurn();
	}


	public void PlayOutTurnActions()
	{
		
	}
	

}
