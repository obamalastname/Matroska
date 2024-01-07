using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Checkpoint
{
    [SerializeField] private float weight;
    private Vector3 _position;
    private Quaternion _rotation;

    public float Weight => weight;
    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    public Quaternion Rotation {
        get => _rotation;
        set => _rotation = value;
    }

    #region Constructors
    public Checkpoint(float weight, Vector3 position, Quaternion rotation)
    {
        this.weight = weight;
        _position = position;
        _rotation = rotation;
    }

 
    #endregion

}
