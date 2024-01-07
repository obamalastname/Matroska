using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyCharacterMovement;
using Sirenix.OdinInspector;
using UnityEngine;
using Controller = Matroska.Controller;

[RequireComponent(typeof(Matroska.Controller.Character))]
public class CharacterCheckpointHandler<T> : MonoBehaviour where T : Matroska.Controller.Character
{
    [InlineEditor]
    [SerializeField] private CharacterCheckpointHandlerSettings settings;
    [SerializeField] private bool autoSetFirstCheckpoint;
    [SerializeField] private List<Checkpoint> checkpoints = new();
    
    protected T character;

    private void Awake() 
    {
        character = GetComponent<T>();
        
        if (autoSetFirstCheckpoint)
        {
            checkpoints.Add(new Checkpoint(0f, transform.position, transform.rotation));
        }
    }

    // When collect new Checkpoint
    public void OnCheckpointTrigger(CheckpointTrigger checkpointTrigger)
    {
        if (checkpoints.Contains(checkpointTrigger.Checkpoint))
            return;
        
        checkpoints.Add(checkpointTrigger.Checkpoint);
    }
    
    // When probably dies and go to a checkpoint
    public void OnCheckpointTeleporter(GoToCheckpointTrigger goToCheckpointTrigger)
    {
        Checkpoint checkpoint = GetHighestWeightCheckpoint();
        TeleportToCheckpoint(checkpoint);
    }

#region Public Methods

    public void TeleportToCheckpoint(Checkpoint checkpoint)
    {
        Teleport(checkpoint.Position, checkpoint.Rotation);
    }

    public virtual void Teleport(Vector3 position, Quaternion rotation)
    {
        character.Teleport(position, rotation, settings.delayUpdate);
    }
    
#endregion

#region Private Methods

    private Checkpoint GetHighestWeightCheckpoint()
    {
        if (checkpoints == null || checkpoints.Count == 0) 
        {
            return null;
        }
        
        Checkpoint highestWeightCheckpoint = checkpoints.OrderByDescending(c => c.Weight).FirstOrDefault();
        return highestWeightCheckpoint;
    } 

#endregion
}

// Base implementation
public class CharacterCheckpointHandler : CharacterCheckpointHandler<Controller.Character>
{
    
}

// Third person CM implementation
[RequireComponent(typeof(Controller.CharacterLook))]
public class ThirdPersonCharacterCheckpointHandler : CharacterCheckpointHandler<Matroska.CMThirdPersonCharacter>
{
    private Controller.CharacterLook _characterLook;

    protected Controller.CharacterLook CharacterLook
    {
        get
        {
            if (_characterLook == null)
                _characterLook = GetComponent<Controller.CharacterLook>();
            
            return _characterLook;
        }
    }
    
    
    private Coroutine _cameraSlerpRoutine;

    private IEnumerator SlerpCameraRoutine()
    {
        yield return null;
    }
}