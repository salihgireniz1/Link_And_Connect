using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickControlWithRigidbody : IJoystickControl
{
    private VariableJoystick _variableJoystick;
    private Rigidbody _rigidbody;
    private Transform _transform;

    private Vector3 _movementInput;

    public JoystickControlWithRigidbody(Transform transform, Rigidbody rigidbody, VariableJoystick variableJoystick)
    {
        _transform = transform;
        _rigidbody = rigidbody;
        _variableJoystick = variableJoystick;
    }

    public void Tick(float movementSpeed, float rotationSpeed)
    {
        float horizontalInput = _variableJoystick.Horizontal;
        float verticalInput = _variableJoystick.Vertical;

        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (movement == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(movement);

        targetRotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

        _rigidbody.MovePosition(_rigidbody.position + movement * movementSpeed * Time.fixedDeltaTime);
        _rigidbody.MoveRotation(targetRotation);
    }

    public void TickKinematikRigidbody(float movementSpeed)
    {
        _movementInput = _variableJoystick.Horizontal * Vector3.right + _variableJoystick.Vertical * Vector3.forward;

        //movementInput.Normalize();

        float y = _rigidbody.velocity.y;

        if (_movementInput.normalized != Vector3.zero)
        {
            if (_transform.forward != _movementInput.normalized)
            {
                //_transform.rotation = Quaternion.RotateTowards(_transform.rotation, Quaternion.LookRotation(movementInput), Time.fixedDeltaTime * rotationSpeed);
                _rigidbody.velocity = Vector3.zero;
                _transform.rotation = Quaternion.LookRotation(_movementInput.normalized);
                //_rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero, Time.deltaTime * 30);
            }
            _rigidbody.velocity = _movementInput * movementSpeed * Time.fixedDeltaTime;

            //_rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, movementInput * movementSpeed, Time.deltaTime * movementSpeed);
        }
        //else
        //{
        //    _rigidbody.velocity = Vector3.MoveTowards(_rigidbody.velocity, Vector3.zero, Time.deltaTime * 30);
        //    Debug.Log("3");

        //}
        Vector3 velocity = _rigidbody.velocity;
        velocity.y = y;
        _rigidbody.velocity = velocity;

    }


}
