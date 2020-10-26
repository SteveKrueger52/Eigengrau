using System;
using UnityEngine;

public class StaticButtonWrapper : MonoBehaviour
{
    public string className;
    public string methodName;

    public void Activate()
    {
        Type.GetType(className).GetMethod(methodName).Invoke(null, null);
    }
}
