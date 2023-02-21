using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

[EditorToolbarElement(id, typeof(SceneView))]
class CreateNote : EditorToolbarToggle, IAccessContainerWindow
{
    // This ID is used to populate toolbar elements.
    public const string id = "SceneNoteToolbar/Button";

    // IAccessContainerWindow provides a way for toolbar elements to access the `EditorWindow` in which they exist.
    // Here we use `containerWindow` to focus the camera on our newly instantiated objects after creation.
    public EditorWindow containerWindow { get; set; }

    // As this is ultimately just a VisualElement, it is appropriate to place initialization logic in the constructor.
    // In this method you can also register to any additional events as required. Here we will just set up the basics:
    // a tooltip, icon, and action.

    private Material material { get; set; }
    public CreateNote()
    {
        // A toolbar element can be either text, icon, or a combination of the two. Keep in mind that if a toolbar is
        // docked horizontally the text will be clipped, so usually it's a good idea to specify an icon.
        text = "Create new Note";
        icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/CreateCubesIcon.png");

        tooltip = "creates new note in scene";
        SceneView.duringSceneGui -= SceneView_duringSceneGui;
        SceneView.duringSceneGui += SceneView_duringSceneGui;

        material = new Material(Shader.Find("Standard"));
        material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/Textures/ns-logo - Copy.png");
    }
    Vector3 Position;
    Vector3 Scale = Vector3.one;
    Quaternion quaternion = Quaternion.identity;
    private void SceneView_duringSceneGui(SceneView obj)
    {
        Graphics.DrawMesh(Resources.GetBuiltinResource<Mesh>("Plane.fbx"), Matrix4x4.TRS(Position, quaternion, Scale), material, 1, obj.camera);
        if (!value)
        {
            if (Handles.Button(Position, Quaternion.identity, .1f, .1f, Handles.DotHandleCap))
            {
                Debug.Log(Tools.current);
            }


            Handles.Label(Position, "test");
            return;
        }
        Handles.color = Color.green;

        if(Tools.current == Tool.Move)
        Position = Handles.DoPositionHandle(Position, Quaternion.identity);
        else if (Tools.current == Tool.Rotate)
            quaternion = Handles.DoRotationHandle(quaternion, Position);
        else if (Tools.current == Tool.Scale)
            Scale = Handles.DoScaleHandle(Scale, Position, quaternion, 1f);


    }
}


// All Overlays must be tagged with the OverlayAttribute
[Overlay(typeof(SceneView), "Placement Tools")]
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
        CreateNote.id)
    {

    }
}