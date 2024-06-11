using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action<Resource> ReleasingResource;

    public void Release()
    {
        ReleasingResource?.Invoke(this);
    }
}
