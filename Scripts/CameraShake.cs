using Godot;
using System;

public class CameraShake : Camera
{
    private Timer timer;
    private Tween tween;

    private float shakeAmount = 0.0f;

    public override void _Ready()
    {
        SetProcess(false);

        PrintTree();
        timer = GetNode<Timer>("Timer");
        tween = GetNode<Tween>("Tween");

        timer.Connect("timeout", this, nameof(onTimerTimeout));
    }

    public override void _Process(float delta)
    {
        HOffset = (float)GD.RandRange(-shakeAmount, shakeAmount) * delta;
        VOffset = (float)GD.RandRange(shakeAmount, -shakeAmount) * delta;
    }

    public void Shake(float newShake, float shakeTime = 0.4f, float shakeLimit = 100)
    {
        shakeAmount += newShake;
        if (shakeAmount > shakeLimit)
            shakeAmount = shakeLimit;

        timer.WaitTime = shakeTime;

        tween.StopAll();
        SetProcess(true);
        timer.Start();
    }

    private void onTimerTimeout()
    {
        shakeAmount = 0.0f;
        SetProcess(false);

        tween.InterpolateProperty(this, "h_offset", HOffset, 0.0f,
            0.1f, Tween.TransitionType.Quad, Tween.EaseType.InOut);
        tween.InterpolateProperty(this, "v_offset", VOffset, 0.0f,
            0.1f, Tween.TransitionType.Quad, Tween.EaseType.InOut);
    }
}
