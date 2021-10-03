using Godot;
using System;

public class UI : Control
{
    private Label scoreLabel;
    private Label heightLabel;
    private ProgressBar WinBar;
    private Control popupLocation;
    private GameManager gameManager;
    private PackedScene popupScene;
    private float UISmoother = 0.0f;

    public override void _Ready()
    {
        base._Ready();

        scoreLabel = GetNode<Label>("Score");
        heightLabel = GetNode<Label>("Height");
        WinBar = GetNode<ProgressBar>("WinBar");
        popupLocation = GetNode<Control>("PopupLocation");
        popupScene = GD.Load<PackedScene>("res://Scenes/UI/PopupText.tscn");
        gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public override void _Process(float delta)
    {
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
    }

    public void PopupText(String text)
    {
        PopupText popupNode = popupScene.Instance<PopupText>();
        popupLocation.AddChild(popupNode);
        popupNode.Setup(text);
    }
}
