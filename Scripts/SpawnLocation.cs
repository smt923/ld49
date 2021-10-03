using Godot;
using System;

public class SpawnLocation : KinematicBody
{
    private float maxDistance = 1.5f;
    private float moveVec = 1.0f;
    private float moveSpeed = 0.8f;
    private float speedDamp = 5.0f;
    private Vector3 moveDirection;
    private Vector3 slideVelocity;
    private Vector3 finalMovement;
    private Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        moveDirection = new Vector3(moveVec, 0.0f, -moveVec);

        moveDirection = moveDirection.Normalized();
        slideVelocity = slideVelocity.LinearInterpolate(moveDirection * moveSpeed, speedDamp * delta);
        finalMovement.z = slideVelocity.z;
        finalMovement.x = slideVelocity.x;

        MoveAndSlide(finalMovement);

        if (Transform.origin.x >= maxDistance || Transform.origin.z >= maxDistance)
        {
            // every time we hit the max distance, make it further & faster
            moveVec = -moveVec;
            maxDistance += 0.5f;
            moveSpeed += 0.3f;

            maxDistance = Mathf.Clamp(maxDistance, 0.0f, 8.5f);
            moveSpeed = Mathf.Clamp(moveSpeed, 0.1f, 5.0f);
        }

    }
}
