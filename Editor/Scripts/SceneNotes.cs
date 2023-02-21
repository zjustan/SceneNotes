using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


public class SceneNotes : Editor
{
    
    public void OnEnable()
    {
        SceneView.duringSceneGui += SceneView_duringSceneGui;
    }

    private void SceneView_duringSceneGui(SceneView obj)
    {

    }
}
