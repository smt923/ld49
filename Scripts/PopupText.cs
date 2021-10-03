using Godot;
using System;

public class PopupText : Label
{
    private AnimationPlayer anim;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        anim = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void Setup(String text)
    {
        Text = text;
        anim.Play("PopupText");
    }

    public void _on_AnimationPlayer_animation_finished(String animName)
    {
        QueueFree();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
