using Godot;
using System;

public class PlayerController : KinematicBody
{
    private float speed = 6.6f;
    private float speedDamp = 1.4f;
    private Vector3 moveDirection;
    private Vector3 hVelocity;
    private Vector3 movement;
    private RayCast castC;
    private RayCast castL;
    private RayCast castR;
    private GameManager gameManager;

    public override void _Ready()
    {
        gameManager = GetNode<GameManager>("/root/GameManager");
        castC = GetNode<RayCast>("RayCastC");
        castL = GetNode<RayCast>("RayCastL");
        castR = GetNode<RayCast>("RayCastR");
        castC.Enabled = true;
        castL.Enabled = true;
        castR.Enabled = true;
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

    public override void _Process(float delta)
    {
        base._Process(delta);

        float castCheight = castC.GetCollisionPoint().y;
        float castLheight = castL.GetCollisionPoint().y;
        float castRheight = castR.GetCollisionPoint().y;

        if (castRheight > castLheight && castRheight > castCheight)
        {
            if (castR.GetCollider() is StackerShape shapeR && shapeR.HasLanded)
            {
                Vector3 hitVec = castR.GetCollisionPoint();
                hitVec.x = Transform.origin.x;
                hitVec.z = Transform.origin.z;
                gameManager.PlayerStackHeight = Transform.origin.DistanceTo(hitVec);
            }
        }
        else if (castLheight > castCheight && castLheight > castRheight)
        {
            if (castL.GetCollider() is StackerShape shapeC && shapeC.HasLanded)
            {
                Vector3 hitVec = castL.GetCollisionPoint();
                hitVec.x = Transform.origin.x;
                hitVec.z = Transform.origin.z;
                gameManager.PlayerStackHeight = Transform.origin.DistanceTo(hitVec);
            }
        }
        else
        {
            if (castC.GetCollider() is StackerShape shapeC && shapeC.HasLanded)
            {
                Vector3 hitVec = castC.GetCollisionPoint();
                hitVec.x = Transform.origin.x;
                hitVec.z = Transform.origin.z;
                gameManager.PlayerStackHeight = Transform.origin.DistanceTo(hitVec);
            }
        }
    }
}
