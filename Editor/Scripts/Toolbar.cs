using System;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

[EditorToolbarElement(id, typeof(SceneView))]
class EditNoteToggle : EditorToolbarToggle, IAccessContainerWindow
{
    // This ID is used to populate toolbar elements.
    public const string id = "SceneNoteToolbar/ToggleEdit";

    // IAccessContainerWindow provides a way for toolbar elements to access the `EditorWindow` in which they exist.
    // Here we use `containerWindow` to focus the camera on our newly instantiated objects after creation.
    public EditorWindow containerWindow { get; set; }

    // As this is ultimately just a VisualElement, it is appropriate to place initialization logic in the constructor.
    // In this method you can also register to any additional events as required. Here we will just set up the basics:
    // a tooltip, icon, and action.

    private Material material { get; set; }
    public EditNoteToggle()
    {
        // A toolbar element can be either text, icon, or a combination of the two. Keep in mind that if a toolbar is
        // docked horizontally the text will be clipped, so usually it's a good idea to specify an icon.
        text = "Toggle edit notes";
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CreateCubesIcon.png");

        tooltip = "Toggle edit notes";

        RegisterCallback<ChangeEvent<bool>>(Changed);
        material = new Material(Shader.Find("Standard"));
        material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/ns-logo - Copy.png");
    }

    private void Changed(ChangeEvent<bool> evt)
    {
        SceneNotes.Instance.IsEditing = evt.newValue;
    }

    Vector3 Position;
    Vector3 Scale = Vector3.one;
    Quaternion quaternion = Quaternion.identity;
    
}
[EditorToolbarElement(id, typeof(SceneView))]
class CreateNewNote : EditorToolbarDropdown
{
    public const string id = "SceneNoteToolbar/CreateNewNote";

    public CreateNewNote()
    {
        text = "Create new note";
        clicked += OnClick;        
    }

    private void OnClick()
    {
        var NewNote = new TextNote
        {
            Position = Vector3.zero,
            color = Color.red,
            Text = text
        };
        SceneNotes.Instance.CreateNew(NewNote);
    }
}


// All Overlays must be tagged with the OverlayAttribute
[Overlay(typeof(SceneView), "Scene notes",true)]
// IconAttribute provides a way to define an icon for when an Overlay is in collapsed form. If not provided, the first
// two letters of the Overlay name will be used.
[Icon("Assets/PlacementToolsIcon.png")]
// Toolbar overlays must inherit `ToolbarOverlay` and implement a parameter-less constructor. The contents of a toolbar
// are populated with string IDs, which are passed to the base constructor. IDs are defined by
// EditorToolbarElementAttribute.
public class SceneNoteToolbar : ToolbarOverlay
{
    // ToolbarOverlay implements a parameterless constructor, passing 2 EditorToolbarElementAttribute IDs. This will
    // create a toolbar overlay with buttons for the CreateCubes and DropdownToggleExample examples.
    // This is the only code required to implement a toolbar overlay. Unlike panel overlays, the contents are defined
    // as standalone pieces that will be collected to form a strip of elements.
    SceneNoteToolbar() : base(
        EditNoteToggle.id,
        CreateNewNote.id
        )
    {
        _ = SceneNotes.Instance;
    }


}