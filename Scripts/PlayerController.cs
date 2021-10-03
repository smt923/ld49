using Godot;
using System;

public class PlayerController : KinematicBody
{
    private float speed = 5.5f;
    private float speedDamp = 2.5f;
    private Vector3 moveDirection;
    private Vector3 hVelocity;
    private Vector3 movement;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);

        if (Input.IsActionPressed("Left") && Transform.origin.z <= 10.0f)
            moveDirection = new Vector3(-1, 0, 1);
        else if (Input.IsActionPressed("Right") && Transform.origin.x <= 10.0f)
            moveDirection = new Vector3(1, 0, -1);
        else
            moveDirection = Vector3.Zero;

        moveDirection = moveDirection.Normalized();
        hVelocity = hVelocity.LinearInterpolate(moveDirection * speed, speedDamp * delta);
        movement.z = hVelocity.z;
        movement.x = hVelocity.x;

        MoveAndSlide(movement);
    }
}
