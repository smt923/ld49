using Godot;
using System;

public class CinematicCamera : Camera
{
    private bool started = false;
    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Process;
    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        if (started)
        {
            this.Size += 1.0f * delta;
            this.Size = Mathf.Clamp(this.Size, 15.0f, 40.0f);
        }
    }

    public void Start()
    {
        this.Current = true;
        started = true;
    }
}
