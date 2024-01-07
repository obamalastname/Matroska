using System;
using EasyCharacterMovement;
using UnityEngine;

/// <summary>
/// The character will bump with other <see cref="Bumper"/>
/// </summary>
[RequireComponent(typeof(CharacterMovement))]
public class CharacterBump : MonoBehaviour
{
    #region Serialized Fields
    [Tooltip("How much we take in consideration the player velocity")]
    [SerializeField] private float velocityMultiplier = 1;
    [Tooltip("Add a Y velocity after everything is calculated if the impact velocity is lower then the vertical")]
    [SerializeField] private float minimumVerticalLaunchSpeed;
    [Tooltip("The minimum force we have to launch the CharacterBump. 0 is ignored")]
    [SerializeField] private float minimumLaunchSpeed = 0;
    [Tooltip("The maximum we can launch the CharacterBump. 0 is ignored")]
    [SerializeField] private float maximumLaunchSpeed = 0;
    #endregion
    
    #region Private Fields
    private CharacterMovement _characterMovement;
    #endregion

    #region Monobehavior
    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void OnEnable()
    {
        _characterMovement.Collided += OnCollided;
    }

    private void OnDisable()
    {
        _characterMovement.Collided -= OnCollided;
    }
    #endregion

#region Methods
    private void OnCollided(ref CollisionResult collisionResult)
    {
        Bumper bumper = collisionResult.collider.GetComponent<Bumper>();
        
        if (!bumper)
            return;

        // Use maxLaunchSpeed ?
        float maxLaunchSpeed = maximumLaunchSpeed;
        
        if (maxLaunchSpeed == 0)
            maxLaunchSpeed = float.PositiveInfinity;

        // Interaction between Bumper and Player
        float impactMultiplier = bumper.BumpMultiplier * CalculateImpactMultiplier(bumper);
        
        // Clamp the impactMultpliyer
        impactMultiplier = Mathf.Clamp(impactMultiplier, bumper.MinimumImpactMultiplier, maxLaunchSpeed);
        
        // Impact with direction
        Vector3 impact = collisionResult.normal * impactMultiplier;

        // If the impact is too low
        if (impact.magnitude < minimumLaunchSpeed)
        {
            impact = collisionResult.normal * minimumLaunchSpeed;
        }

        // Add the vertical boost if the vertical launch is too low and more then 0 to avoid launching in the air below bumpers
        if (impact.y < minimumVerticalLaunchSpeed && impact.y >= 0)
        {
            impact = new Vector3(impact.x, minimumVerticalLaunchSpeed, impact.z);
        }

        // Launch
        _characterMovement.PauseGroundConstraint();
        _characterMovement.LaunchCharacter(impact, false, false);
    }
    
    private float CalculateImpactMultiplier(Bumper bumper)
    {
        float playerVelocity = _characterMovement.velocity.magnitude * velocityMultiplier;
        
        // We had 1 to avoid multiply by 0 for idle bumpers
        float bumperVelocity = 1 + bumper.Velocity;
        
        switch (bumper.ImpactType)
        {
            case Bumper.ImpactCombineType.Average:
                return (bumperVelocity + playerVelocity) / 2f;
            case Bumper.ImpactCombineType.Minimum:
                return Mathf.Min(bumperVelocity, playerVelocity);
            case Bumper.ImpactCombineType.Maximum:
                return Mathf.Max(bumperVelocity, playerVelocity);
            case Bumper.ImpactCombineType.Bumper:
                return bumperVelocity;
            case Bumper.ImpactCombineType.Player:
                return playerVelocity;
            case Bumper.ImpactCombineType.Multiply:
            default:
                return bumperVelocity * playerVelocity;
        }
    }
#endregion
}
