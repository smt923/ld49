using Godot;
using System;

public class ObstacleBall : RigidBody
{
    public bool HitObjects = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_ObstacleBall_body_entered(Node other)
    {
        HitObjects = true;
        GetNode<GameManager>("/root/GameManager").DoCameraShake(2.5f, 0.15f, 2.5f);
        GetNode<GameManager>("/root/GameManager").PlayBallCollide();
    }
}
