using Godot;
using System;

public class FallArea : Area
{
    private GameManager gm;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gm = GetNode<GameManager>("/root/World/GameManager");
    }

    public void _on_FallArea_body_entered(Node body)
    {
        if (body is StackerShape)
        {
            GD.Print("Lost a shape!");
            gm.Score -= 200;
            gm.DoPopup($"-200  shape dropped");
            gm.SpawnedShapes.Remove(body);
            GetNode<AudioStreamPlayer>("/root/World/PopPlayer").Play();
            body.QueueFree();
        }

        if (body is ObstacleBall ob)
        {
            if (ob.HitObjects == false)
            {
                gm.Score += 200;
                gm.DoPopup($"+200  ball dodged");
            }
            body.QueueFree();
        }
    }
}
