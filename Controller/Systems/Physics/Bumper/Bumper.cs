using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bumper : MonoBehaviour
{
    #region Enum
    public enum ImpactCombineType
    {
        // Average of Bumper and CharacterBump
        Average, 
        // Return the smallest of Bumper or other
        Minimum, 
        // Return the biggest of Bumper or other
        Maximum, 
        // Multiply both
        Multiply,
        // Only take in consideration Bumper impact velocity
        Bumper,
        // Only take in consideration the other impact velocity
        Player
    }
    #endregion

    #region Serialized Field
    [Required]
    [SerializeField] private Rigidbody _rigidbody;

    [Tooltip("How much the bump affect the impact")]
    [SerializeField] private float bumpMultiplier = 1;
    
    [Tooltip("If the total calculated impactMultiplier is to low, use this multiplier instead")]
    [SerializeField] private float minimumImpactMultiplier;
    
    [Tooltip("How we take in consideration the player speed in consideration")]
    [SerializeField] private ImpactCombineType impactType;

    [Tooltip("Do we take in consideration the Bumper speed")]
    [SerializeField] private bool considerVelocity;
    
    [Tooltip("If the bumper is a moving object, how much to take in consideration it's speed")]
    [ShowIf("considerVelocity")]
    [SerializeField] private float velocityMultiplier = 1;
    #endregion

    #region Properties

    public float BumpMultiplier => bumpMultiplier;
    public float MinimumImpactMultiplier => minimumImpactMultiplier;
    public ImpactCombineType ImpactType => impactType;

    public float Velocity
    {
        get
        {
            // Get the highest velocity between normal and angular
            if (considerVelocity)
                return Mathf.Max(_rigidbody.velocity.magnitude,_rigidbody.angularVelocity.magnitude) * velocityMultiplier;
            
            return 0;
        }

        #endregion
    }
}
