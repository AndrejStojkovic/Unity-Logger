using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoggerScript : MonoBehaviour
{
    private Vector3 prevPosition;
    private Quaternion prevRotation;
    private Vector3 prevScale;

    void Start()
    {
        Logger.Log($"Started testing!");
        setValues();
    }

    void Update()
    {
        if(prevPosition != transform.position)
        {
            Logger.Log($"New Position: {prevPosition} -> {transform.position}");
        }
        if(prevRotation != transform.rotation)
        {
            Logger.LogWarning($"New Rotation: {prevRotation.eulerAngles} -> {transform.rotation.eulerAngles}");
        }
        if(prevScale != transform.lossyScale)
        {
            Logger.LogError($"New Scale: {prevScale} -> {transform.lossyScale}");
        }
        setValues();
    }

    private void setValues()
    {
        prevPosition = gameObject.transform.position;
        prevRotation = transform.rotation;
        prevScale = transform.lossyScale;
    }
}
