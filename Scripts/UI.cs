using Godot;
using System;

public class UI : Node
{
    private Label scoreLabel;
    private Label heightLabel;
    private Control popupLocation;
    private GameManager gameManager;
    private PackedScene popupScene;
    private float heightSmoother = 0.0f;

    public override void _Ready()
    {
        base._Ready();

        scoreLabel = GetNode<Label>("Score");
        heightLabel = GetNode<Label>("Height");
        popupLocation = GetNode<Control>("PopupLocation");
        popupScene = GD.Load<PackedScene>("res://Scenes/UI/PopupText.tscn");
        gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public override void _Process(float delta)
    {
        scoreLabel.Text = $"{gameManager.Score}";

        heightSmoother += 1.0f * delta;
        if (heightSmoother >= 0.1f)
        {
            heightLabel.Text = $"{gameManager.PlayerStackHeight:0.#}m";
            heightSmoother = 0.0f;
        }
    }

    public void PopupText(String text)
    {
        PopupText popupNode = popupScene.Instance<PopupText>();
        popupLocation.AddChild(popupNode);
        popupNode.Setup(text);
    }
}
