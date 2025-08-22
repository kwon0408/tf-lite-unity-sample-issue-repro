using System;
using TensorFlowLite;
using UnityEngine;

public class Horsepower: IDisposable
{
    private Interpreter interpreter;

    public Horsepower(string modelPath)
    {
        // NO GPU
        var options = new InterpreterOptions()
        {
            threads = 2
        };
        interpreter = new Interpreter(FileUtil.LoadFile(modelPath), options);

        var inputInfo = interpreter.GetInputTensorInfo(0);
        var outputInfo = interpreter.GetOutputTensorInfo(0);
        // inputs = new float[inputInfo.shape[1]];
        // outputs = new float[outputInfo.shape[1]];
        Debug.Log($"inputInfo.shape = {string.Join(',', inputInfo.shape)}");
        Debug.Log($"outputInfo.shape = {string.Join(',', outputInfo.shape)}");
        interpreter.ResizeInputTensor(0, inputInfo.shape);
        interpreter.AllocateTensors();
    }

    public float Invoke(float[] input)
    {
        if (interpreter == null)
        {
            Debug.LogError("Interpreter is not initialized.");
            return -1;
        }

        interpreter.SetInputTensorData(0, input);

        try
        {
            interpreter.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Invoke failed: {e}");
            throw;
        }

        var output = new float[1] { float.NaN };
        interpreter.GetOutputTensorData(0, output);
        if (float.IsNaN(output[0]))
        {
            throw new Exception("GetOutputTensorData() did not create output");
        }

        return output[0];
    }

    #region IDisposable
    public bool IsDisposed { get; private set; } = false;

    public void Dispose()
    {
        interpreter.Dispose();
        IsDisposed = true;
    }
    #endregion
}