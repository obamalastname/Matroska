using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Matroska.Controller
{
	public class FirstPersonCharacter : EasyCharacterMovement.FirstPersonCharacter
	{
#region Serialized Field

		[Header("Cinemachine")]
		public CinemachineVirtualCamera cmWalkingCamera;
		public CinemachineVirtualCamera cmCrouchedCamera;

#endregion

#region Methods

		protected override void AnimateEye()
		{
			// Removes programatically crouch / un crouch animation as this will be handled by Cinemachine cameras
		}

		protected override void OnCrouched()
		{
			base.OnCrouched();

			cmWalkingCamera.gameObject.SetActive(false);
			cmCrouchedCamera.gameObject.SetActive(true);
		}

		protected override void OnUnCrouched()
		{
			base.OnUnCrouched();
			
			cmCrouchedCamera.gameObject.SetActive(false);
			cmWalkingCamera.gameObject.SetActive(true);
		}

#endregion
	}
}