using Godot;
using System;

public class ObstacleBall : RigidBody
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_ObstacleBall_body_entered(Node other)
    {
        GetNode<GameManager>("/root/GameManager").DoCameraShake(2.5f, 0.15f, 2.5f);
        GetNode<GameManager>("/root/GameManager").PlayBallCollide();
    }
}
