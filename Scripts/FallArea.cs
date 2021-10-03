using Godot;
using System;

public class FallArea : Area
{
    private GameManager gm;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        gm = GetNode<GameManager>("/root/GameManager");
    }

    public void _on_FallArea_body_entered(Node body)
    {
        if (body is StackerShape)
        {
            GD.Print("Lost a shape!");
            gm.Score -= 200;
            gm.DoPopup($"-200  shape dropped");
        }
    }
}
