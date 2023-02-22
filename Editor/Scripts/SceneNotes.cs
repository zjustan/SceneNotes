using Codice.CM.SEIDInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[UnityEditor.InitializeOnLoad()]
public class SceneNotes : Editor
{
    public static SceneNotes Instance
    {
        get
        {
            if (!_instance)
                _instance = CreateInstance<SceneNotes>();
            return _instance;
        }
    }

    private static SceneNotes _instance;


    public List<Note> notes;
    public bool IsEditing;
    private Note EditingNote;


    
    public NoteInspector NoteInspector
    {
        get
        {
            return _noteInspector;
        }
        set
        {
            _noteInspector = value;
            _noteInspector.CreateInspector(EditingNote);
        }
    }


    private NoteInspector _noteInspector;
    static SceneNotes()
    {

    }

    public void OnEnable()
    {
        if (_instance == null)
            _instance = this;

        if (_instance != this)
            return;

        Debug.Log("Test");
        notes = new List<Note>();
        notes.Add(new TextNote
        {
            Position = Vector3.up,
            color = Color.red,
            Text = "Forntite"
        });
        SceneView.duringSceneGui -= SceneView_duringSceneGui;
        SceneView.duringSceneGui += SceneView_duringSceneGui;
    }

    private void SceneView_duringSceneGui(SceneView obj)
    {
        foreach (var item in notes)
        {
            if (IsEditing)
            {
                if (item == EditingNote)
                    item.DrawEditor();
                else if (item.DrawEditable())
                    SetEditingItem( item);
            }
            else
                item.Draw();
        }
    }
    public void SetEditingItem(Note note)
    {
        EditingNote = note;
        if(NoteInspector != null)
        {
            NoteInspector.CreateInspector(EditingNote);
        }
    }
    internal void CreateNew(Note newNote)
    {
        IsEditing = true;
        SetEditingItem(newNote);
        notes.Add(newNote);
    }
}

[Overlay(typeof(SceneView), "SceneNote overlay", true)]
public class NoteInspector : Overlay
{

    VisualElement Parent;

    public override VisualElement CreatePanelContent()
    {
        Parent = new GroupBox(); 
        SceneNotes.Instance.NoteInspector = this;
        return Parent;
    }

    public void CreateInspector(Note note)
    {
        Parent.Clear();

        if (note == null)
            return;
            
        var fields = note.GetType().GetFields().Where(e => e.IsPublic);

        foreach (var item in fields)
        {

           if(item.FieldType == typeof(string))
                CreateField<string, TextField>(item, note);
            else if (item.FieldType == typeof(Vector3))
                CreateField<Vector3, Vector3Field>(item, note);
            else if (item.FieldType == typeof(Color))
                CreateField<Color, ColorField>(item, note);
            else if (item.FieldType == typeof(Vector2))
                CreateField<Vector2, Vector2Field>(item, note);
        }
    }

    public void CreateField<Ttype, Field>(FieldInfo info, Note note) where Field : BaseField<Ttype>
    {
        var textfield = Activator.CreateInstance<Field>();
        textfield.value = (Ttype)info.GetValue(note);
        textfield.label = info.Name;
        textfield.style.minWidth = new StyleLength();
        textfield.RegisterValueChangedCallback(x => info.SetValue(note, x.newValue));
        Parent.Add(textfield);

    }
}