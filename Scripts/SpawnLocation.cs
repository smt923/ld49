using Godot;
using System;

public class SpawnLocation : KinematicBody
{
    private float maxDistance = 1.5f;
    private bool hasFlipped = false;
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
        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(_on_Timeout));
        timer.WaitTime = 2.0f;
        timer.OneShot = false;
        timer.Start();
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

        Transform t = Transform;
        t.origin.x = Mathf.Clamp(t.origin.x, -maxDistance, maxDistance);
        t.origin.z = Mathf.Clamp(t.origin.z, -maxDistance, maxDistance);
        Transform = t;
    }

    private void _on_Timeout()
    {
        // GD.Print("--- timeout!!! ---");
        // GD.Print("x: " + Transform.origin.x + " z: " + Transform.origin.z + " dist: " + maxDistance);
        // GD.Print("vec: " + moveVec);

        moveVec = -moveVec;

        maxDistance += 0.4f;
        moveSpeed += 0.4f;

        maxDistance = Mathf.Clamp(maxDistance, 0.5f, 14.0f);
        moveSpeed = Mathf.Clamp(moveSpeed, 0.1f, 6.0f);

        timer.WaitTime += 0.25f;
        timer.WaitTime = Mathf.Clamp(timer.WaitTime, 2.0f, 7.5f);
    }
}
