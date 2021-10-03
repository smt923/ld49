using Godot;
using System;

public class ObstacleBall : RigidBody
{
    public bool HitObjects = false;
    private Timer timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(_on_Timeout));
        timer.WaitTime = 10.0f;
        timer.OneShot = true;
        timer.Start();
    }

    public void _on_ObstacleBall_body_entered(Node other)
    {
        HitObjects = true;
        GetNode<GameManager>("/root/World/GameManager").DoCameraShake(2.5f, 0.15f, 2.5f);
        GetNode<GameManager>("/root/World/GameManager").PlayBallCollide();
    }

    private void _on_Timeout()
    {
        QueueFree();
    }
}
