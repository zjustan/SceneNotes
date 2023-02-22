using UnityEditor;
using UnityEngine;
public abstract class Note
{
    public abstract void Draw();
    public abstract bool DrawEditable();

    public abstract void DrawEditor();
}

public class TextNote : Note
{
    public Vector3 Position;
    public Color color;
    public string Text;

    public override void Draw()
    {
        Handles.color = color;
        Handles.Label(Position,Text);
    }

    public override bool DrawEditable()
    {
        Handles.color = Color.white;
        bool button = Handles.Button(Position, Quaternion.identity, .1f, .1f, Handles.DotHandleCap);
        Draw();
        return button;


    }

    public override void DrawEditor()
    {
        Draw();
        Position = Handles.PositionHandle(Position, Quaternion.identity);
    }
}