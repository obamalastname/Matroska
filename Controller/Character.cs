using System.Collections;
using System.Collections.Generic;
using EasyCharacterMovement;
using UnityEngine;

namespace Matroska.Controller
{
	/// <summary>
    /// Allows me to add functionality to EasyCharacter while updating the package
    /// </summary>
    public class Character : EasyCharacterMovement.Character
    {
    	[SerializeField] private GameObject visual;
    
    #region New Methods
    	/// <summary>
    	/// Init Player input only related to movement
    	/// <remarks>Does the Init input related to Camera Movement</remarks>
    	/// </summary>
    	public void InitPlayerMovementInput()
    	{
    		base.InitPlayerInput();
    	}
    
    	/// <summary>
    	/// DeInit Player input only related to movement
    	/// <remarks>Does the DeInit input related to Camera Movement</remarks>
    	/// </summary>
    	public void DeInitPlayerMovementInput()
    	{
    		base.DeinitPlayerInput();
    	}
    	
    	public void DisableVisuals()
    	{
    		visual.SetActive(false);
    	}
    
    	public void EnableVisuals()
    	{
    		visual.SetActive(true);
    	}
    #endregion
    
    #region Custom Methods
	    private IEnumerator TeleportWithDelay(Vector3 position, Quaternion rotation, float delay)
    	{
    		DeInitPlayerMovementInput();
    		
    		TeleportPosition(position);
    		TeleportRotation(rotation);
    
    		yield return new WaitForSeconds(delay);
    		
    		InitPlayerMovementInput();
    	}
    
    	public void Teleport(Vector3 position, Quaternion rotation, float delay)
    	{
    		StartCoroutine(TeleportWithDelay(position, rotation, delay));
    	}
    
    #endregion
	}
}
