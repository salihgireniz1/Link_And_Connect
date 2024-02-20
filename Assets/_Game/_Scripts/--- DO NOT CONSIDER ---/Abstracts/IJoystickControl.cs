using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJoystickControl
{
    public void Tick(float speed, float rotationSpeed);
}
