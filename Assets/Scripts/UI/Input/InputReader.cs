using System;
using UnityEngine;
using Zenject;

public class InputReader : ITickable
{
    public event Action MouseClicked;

    void ITickable.Tick()
    {
        if (Input.GetKeyDown(InputData.LeftClick)) 
            MouseClicked?.Invoke();
    }
}
