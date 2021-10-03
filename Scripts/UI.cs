using Godot;
using System;

public class UI : Node
{
    private Label scoreLabel;
    private Control popupLocation;
    private GameManager gameManager;
    private PackedScene popupScene;

    public override void _Ready()
    {
        base._Ready();

        scoreLabel = GetNode<Label>("Score");
        popupLocation = GetNode<Control>("PopupLocation");
        popupScene = GD.Load<PackedScene>("res://Scenes/UI/PopupText.tscn");
        gameManager = GetNode<GameManager>("/root/GameManager");
    }

    public override void _Process(float delta)
    {
        scoreLabel.Text = $"{gameManager.Score}";
    }

    public void PopupText(String text)
    {
        PopupText popupNode = popupScene.Instance<PopupText>();
        popupLocation.AddChild(popupNode);
        popupNode.Setup(text);
    }
}
