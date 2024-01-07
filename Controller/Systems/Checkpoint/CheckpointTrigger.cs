using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CheckpointTrigger : MonoBehaviour
{
    // Fields
    [Required]
    [SerializeField] private Checkpoint checkpoint;
    [SerializeField] private bool useOwnRotation;
    [SerializeField] private bool disableOnTrigger;

    // Properties
    public Checkpoint Checkpoint => checkpoint;
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterCheckpointHandler characterCheckpointHandler = other.GetComponent<CharacterCheckpointHandler>();

        if (characterCheckpointHandler)
        {
            // Set the correct checkpoint rotation
            checkpoint.Rotation = useOwnRotation ? transform.rotation : characterCheckpointHandler.transform.rotation;
            
            characterCheckpointHandler.OnCheckpointTrigger(this);

            if (disableOnTrigger)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
