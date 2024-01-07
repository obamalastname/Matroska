using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoToCheckpointTrigger : MonoBehaviour
{
	[Tooltip("Use a custom transform to teleport to.")]
	[SerializeField] private bool useCustomEndPoint;
	
	[ShowIf("useCustomEndPoint")]
	[SerializeField] private Transform customCheckpointTransform;
	
	private void OnTriggerEnter(Collider other)
	{
		CharacterCheckpointHandler characterCheckpointHandler = other.GetComponent<CharacterCheckpointHandler>();

		if (!characterCheckpointHandler)
			return;

		if (useCustomEndPoint)
		{
			if (customCheckpointTransform == null)
			{
				Debug.LogError("Custom Checkpoint Transform is null in " + transform.name);
				return;
			}
				
			characterCheckpointHandler.Teleport(customCheckpointTransform.position, customCheckpointTransform.rotation);
		}
		else
		{
			// Will use the highest weighted checkpoint in CheckpointHandler
			characterCheckpointHandler.OnCheckpointTeleporter(this);
		}
	}
}