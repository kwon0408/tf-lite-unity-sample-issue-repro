using System;
using System.IO;
using TensorFlowLite;
using UnityEngine;

public class SceneScript: MonoBehaviour
{
    [SerializeField, FilePopup("*.tflite")]
    private string HorsepowerModelFile;
    private Horsepower Horsepower;
    
    public void OnClickTestButton()
    {
        var input = new float[] { 6f, 232f, 90f, 3210f, 17.2f, 78f, 0f, 1f, 0f };
        string path = Path.Combine(Application.streamingAssetsPath, HorsepowerModelFile);
        Horsepower ??= new Horsepower(path);

        try
        {
            var value = Horsepower.Invoke(input);
            Debug.Log($"invoke success: {value}"); // this will only hit once at the first call
        }
        catch (Exception e)
        {
            Debug.LogError($"invoke failed: {e.Message}"); // this will hit for second and further inputs
        }
    }
    
    public void OnClickTestButtonWithWorkaround()
    {
        var input = new float[] { 6f, 232f, 90f, 3210f, 17.2f, 78f, 0f, 1f, 0f };
        string path = Path.Combine(Application.streamingAssetsPath, HorsepowerModelFile);

        using var horsepower = new Horsepower(path); // this object will be used only once
        try
        {
            var value = horsepower.Invoke(input);
            Debug.Log($"invoke success: {value}"); // this will always hit, because it is always the first input
        }
        catch (Exception e)
        {
            Debug.LogError($"invoke failed: {e.Message}"); // this will never hit, because there is no second input
        }
    }
}
