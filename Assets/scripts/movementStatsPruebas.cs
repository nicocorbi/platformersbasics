using UnityEngine;


[CreateAssetMenu]
public class MovementStats : ScriptableObject
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float jumpForce = 12f;
    public float sustainedJumpForce = 8f;
    public float maxJumpDuration = 0.2f;
    public int jumpLimit = 1;
    public float groundCheckDistance = 0.6f;
    public float gradientValue = 1f;
    
    public float groundFriction = 2f;
    public float airFriction = 2f;
    public float groundAcceleration = 10f;
    public float airAcceleration = 5f;
    public float peakGravity = 0f;
    public float fallingGravity = 0f;
    public float defaultGravity = 0f;
}

