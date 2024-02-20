using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlWithTransform : IJoystickControl
{
    private Transform _transform;
    private VariableJoystick _variableJoystick;

    public JoystickControlWithTransform(Transform transform, VariableJoystick variableJoystick)
    {
        _transform = transform;
        _variableJoystick = variableJoystick;
    }

    public void Tick(float speed, float rotationSpeed)
    {
        Vector3 direction = new Vector3(_variableJoystick.Horizontal, 0, _variableJoystick.Vertical);

        if (direction != Vector3.zero)
        {
            _transform.localPosition += direction * speed * Time.deltaTime;

            Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

        }
    }
}
