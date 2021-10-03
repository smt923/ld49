using Godot;
using System;
using System.Collections.Generic;

public class GameManager : Node
{
    public int Score = 0;

    private float zoomOutRate = 0.2f;
    private float currentScaleFactor = 1.0f;
    private float currentColorFactor = 0.0f;
    private int points = 0;
    private int ballCounter = 0;
    public float winCounter = 0.0f;
    public bool GameWon = false;
    private Spatial SpawnLocation;
    public PlayerController PlayerFloor;
    public float PlayerStackHeight;
    private CameraShake CameraShake;
    private Timer GameClock;
    private UI UI;
    private PackedScene StackerShape;
    private PackedScene ObstacleBall;
    private Gradient ShapeColors;
    public List<Node> SpawnedShapes = new List<Node> { };
    private List<AudioStream> BlockCollideSounds = new List<AudioStream> { };
    private List<AudioStream> BallCollideSounds = new List<AudioStream> { };
    private AudioStreamPlayer blockCollidePlayer;
    private AudioStreamPlayer ballCollidePlayer;
    private AudioStreamPlayer ballSpawnPlayer;
    private AudioStreamPlayer blockSpawnPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        PauseMode = PauseModeEnum.Process;

        StackerShape = ResourceLoader.Load<PackedScene>("res://Scenes/StackerShape.tscn");
        ObstacleBall = ResourceLoader.Load<PackedScene>("res://Scenes/ObstacleBall.tscn");
        ShapeColors = ResourceLoader.Load<Gradient>("res://Materials/ShapeColors.tres");
        SpawnLocation = GetNode<Spatial>("/root/World/SpawnLocation");
        PlayerFloor = GetNode<PlayerController>("/root/World/FloorPlayer");
        CameraShake = GetNode<CameraShake>("/root/World/Camera");
        UI = GetNode<UI>("/root/World/UI");
        blockCollidePlayer = GetNode<AudioStreamPlayer>("/root/World/BlockCollidePlayer");
        ballCollidePlayer = GetNode<AudioStreamPlayer>("/root/World/BallCollidePlayer");
        ballSpawnPlayer = GetNode<AudioStreamPlayer>("/root/World/BallSpawnPlayer");
        blockSpawnPlayer = GetNode<AudioStreamPlayer>("/root/World/BlockSpawnPlayer");

        BlockCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Wood/impactWood_medium_000.ogg"));
        BlockCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Wood/impactWood_medium_001.ogg"));
        BlockCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Wood/impactWood_medium_002.ogg"));
        BlockCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Wood/impactWood_medium_003.ogg"));
        BlockCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Wood/impactWood_medium_004.ogg"));

        BallCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Metal/impactMetal_heavy_000.ogg"));
        BallCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Metal/impactMetal_heavy_001.ogg"));
        BallCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Metal/impactMetal_heavy_002.ogg"));
        BallCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Metal/impactMetal_heavy_003.ogg"));
        BallCollideSounds.Add(GD.Load<AudioStream>("res://Audio/Metal/impactMetal_heavy_004.ogg"));

        GameClock = new Timer();
        AddChild(GameClock);

        GameClock.Connect("timeout", this, nameof(onGameTick));
        GameClock.WaitTime = 6.0f;
        GameClock.OneShot = false;
        GameClock.Start();

        onGameTick(); // spawn our first shape instantly so the player knows what's going on

    }

    public override void _Process(float delta)
    {
        base._Process(delta);

        CameraShake.Size += zoomOutRate * delta;
        CameraShake.Size = Mathf.Clamp(CameraShake.Size, 10.0f, 48.0f);

        Transform camerat = CameraShake.Transform;
        camerat.origin.y += zoomOutRate / 2 * delta;
        camerat.origin.y = Mathf.Clamp(camerat.origin.y, 12.0f, 30.0f);
        CameraShake.Transform = camerat;

        Transform spawnt = SpawnLocation.Transform;
        spawnt.origin.y += zoomOutRate * delta;
        spawnt.origin.y = Mathf.Clamp(spawnt.origin.y, 14.0f, 42.0f);
        SpawnLocation.Transform = spawnt;

        if (PlayerStackHeight > 16.0f)
        {
            if (winCounter >= 5.0f)
            {
                Camera cine = GetNode<Camera>("/root/World/CinematicCamera");
                if (GameWon == false)
                {
                    GameWon = true;
                    GD.Print("hit 16m!");
                    DoPopup("you hit 16m! you win!");
                    UI.Visible = false;
                    GetNode<Control>("/root/World/WinUI").Visible = true;
                    SpawnLocation.Visible = false;
                    foreach (var shape in SpawnedShapes)
                    {
                        if (shape is StackerShape s && s.HasLanded == false)
                            shape.QueueFree();
                    }
                    cine.Current = true;
                    GetTree().Paused = true;
                    winCounter = 5.0f;
                }
                cine.Size += 1.0f * delta;
                cine.Size = Mathf.Clamp(cine.Size, 15.0f, 40.0f);
            }
            winCounter += 1.0f * delta;
        }
        else
        {
            winCounter = 0.0f;
        }
    }

    void onGameTick()
    {
        if (ballCounter == 5)
        {
            RigidBody newBall = ObstacleBall.Instance<RigidBody>();
            AddChild(newBall);
            Transform tr = newBall.Transform;
            tr.origin = SpawnLocation.Transform.origin;
            tr.origin.y += 12.0f;
            newBall.Transform = tr;
            ballSpawnPlayer.Play();
            SpawnLocation.GetNode<AnimationPlayer>("AnimationPlayer").Play("SpawnBad");

            ballCounter = 0;
            return;
        }

        StackerShape newShape = StackerShape.Instance<StackerShape>();
        newShape.SetSizeScale(currentScaleFactor, currentScaleFactor);
        newShape.SetColor(ShapeColors.Interpolate(currentColorFactor));
        AddChild(newShape);
        SpawnedShapes.Add(newShape);
        Transform t = newShape.Transform;
        t.origin = SpawnLocation.Transform.origin;
        t.origin.y += 10.0f;
        newShape.Transform = t;
        blockSpawnPlayer.Play();
        SpawnLocation.GetNode<AnimationPlayer>("AnimationPlayer").Play("SpawnGood");


        currentScaleFactor -= 0.016f; // smaller spawns each time for difficulty 
        currentColorFactor += 0.025f; // increase color gradient value
        GameClock.WaitTime -= 0.032f; // increase spawn frequency
        ballCounter++;

        currentScaleFactor = Mathf.Clamp(currentScaleFactor, 0.2f, 1.0f);
        currentColorFactor = Mathf.Clamp(currentColorFactor, 0.0f, 1.0f);
        GameClock.WaitTime = Mathf.Clamp(GameClock.WaitTime, 3.0f, 5.5f);
    }

    public void DoCameraShake(float newShake, float shakeTime = 0.4f, float shakeLimit = 100)
    {
        CameraShake.Shake(newShake, shakeTime, shakeLimit);
    }

    public void DoPopup(string Text)
    {
        UI.PopupText(Text);
    }

    public void PlayBlockCollide()
    {
        blockCollidePlayer.Stop();
        GD.Randomize();
        int i = (int)GD.RandRange(0, BlockCollideSounds.Count);
        blockCollidePlayer.Stream = BlockCollideSounds[i];
        //GD.Print(BlockCollideSounds[i]);
        blockCollidePlayer.PitchScale = (float)GD.RandRange(0.95f, 1.05f);
        blockCollidePlayer.Play();
    }

    public void PlayBallCollide()
    {
        blockCollidePlayer.Stop();
        ballSpawnPlayer.Stop();
        ballCollidePlayer.Stop();
        GD.Randomize();
        int i = (int)GD.RandRange(0, BallCollideSounds.Count);
        ballCollidePlayer.Stream = BallCollideSounds[i];
        //GD.Print(BallCollideSounds[i]);
        ballCollidePlayer.PitchScale = (float)GD.RandRange(0.95f, 1.05f);
        ballCollidePlayer.Play();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
