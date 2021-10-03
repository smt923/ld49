using Godot;
using System;

public class StackerShape : RigidBody
{
    public bool HasLanded = false;
    public Spatial CenterPoint;
    public GameManager gm;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        SetSizeScale(1, 1);
        CenterPoint = GetNode<Spatial>("CollisionShape/Center");
        gm = GetNode<GameManager>("/root/GameManager");
    }

    public void SetSizeScale(float x, float z)
    {
        CollisionShape col = (CollisionShape)GetNode("CollisionShape");
        col.Transform = col.Transform.Scaled(new Vector3(x, 1f, z));
    }

    public void SetColor(Color c)
    {
        MeshInstance m = GetNode<MeshInstance>("CollisionShape/MeshInstance");
        m.GetActiveMaterial(0).Set("albedo_color", c);
    }

    private void _OnCollisionEnter(Node body)
    {
        gm.DoCameraShake(1.2f, 0.12f, 2.0f);
        //TODO: workout how far from center
        Vector3 locationMinusHeight = Transform.origin;
        locationMinusHeight.y = 0.0f;
        float distance = locationMinusHeight.DistanceTo(gm.PlayerFloor.Transform.origin);
        int score = CalcScore(distance);
        gm.Score += score;
        Disconnect("body_entered", this, nameof(_OnCollisionEnter));
        HasLanded = true;
        Mass = 120;
        GravityScale = 1.85f;
        gm.PlayBlockCollide();
    }

    private int CalcScore(float Distance)
    {
        if (Distance <= 0.3f)
        {
            int score = 200;
            gm.DoPopup($"+{score}  perfect");
            return score;
        }
        if (Distance <= 1.0f)
        {
            int score = 100;
            gm.DoPopup($"+{score}  great");
            return score;
        }
        if (Distance <= 2.5f)
        {
            int score = 50;
            gm.DoPopup($"+{score}  good");
            return score;
        }
        if (Distance <= 4.0f)
        {
            int score = 25;
            gm.DoPopup($"+{score}  sketchy");
            return score;
        }
        else
            return 0;
    } 

}
