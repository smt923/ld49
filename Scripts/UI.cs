using Godot;
using System;

public class UI : Control
{
    private Label scoreLabel;
    private Label heightLabel;
    private ProgressBar WinBar;
    private ProgressBar SprintBar;
    private Control popupLocation;
    private GameManager gameManager;
    private PackedScene popupScene;
    private ColorRect PauseMenu;
    private float UISmoother = 0.0f;

    public override void _Ready()
    {
        base._Ready();

        PauseMode = PauseModeEnum.Process;

        scoreLabel = GetNode<Label>("Score");
        heightLabel = GetNode<Label>("Height");
        WinBar = GetNode<ProgressBar>("WinBar");
        SprintBar = GetNode<ProgressBar>("SprintBar");
        popupLocation = GetNode<Control>("PopupLocation");
        popupScene = GD.Load<PackedScene>("res://Scenes/UI/PopupText.tscn");
        gameManager = GetNode<GameManager>("/root/World/GameManager");
        PauseMenu = GetNode<ColorRect>("RestartMenu");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Pause"))
        {
            PauseMenu.Visible = true;
            GetTree().Paused = true;
        }
        
        scoreLabel.Text = $"{gameManager.Score}";

        UISmoother += 1.0f * delta;
        if (UISmoother >= 0.085f)
        {
            heightLabel.Text = $"{gameManager.PlayerStackHeight:0.#}m";
            if (gameManager.winCounter > 0.0f)
                WinBar.Visible = true;
            else
                WinBar.Visible = false;
            UISmoother = 0.0f;
        }

        if (gameManager.winCounter > 0.0f)
        {
            WinBar.Value = gameManager.winCounter;
        }
        else
        {
            WinBar.Visible = false;
        }

        SprintBar.Value = gameManager.PlayerFloor.sprintTimer;
    }

    public void PopupText(String text)
    {
        PopupText popupNode = popupScene.Instance<PopupText>();
        popupLocation.AddChild(popupNode);
        popupNode.Setup(text);
    }

    public void _on_RestartButton_pressed()
    {
        GetTree().Paused = false;
        GetTree().ReloadCurrentScene();
    }

    public void _on_ResumeButton_pressed()
    {
        GetTree().Paused = false;
        PauseMenu.Visible = false;
    }
}
