namespace PAG.Swerve
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SwerveWithMouse
    {
        private float _lastFrameFingerPositionX;
        private float _moveFactorX;
        private Transform _transform;
        private float _maxSwerveAmount = 1f;
        private float _clampedPositionX = 1f;

        public SwerveWithMouse(Transform transform, float clampedPositionX)
        {
            _transform = transform;
            _clampedPositionX = clampedPositionX;
        }

        public void StartSwerve(float swerveSpeed = 0.5f, float forwardSpeed = 20f)
        {
            GetMouseInput();

            float swerveAmount = _moveFactorX * swerveSpeed * Time.deltaTime;
            swerveAmount = Mathf.Clamp(swerveAmount, -_maxSwerveAmount, _maxSwerveAmount); // burası değiştirilebilir

            _transform.Translate(swerveAmount, 0, forwardSpeed * Time.deltaTime);
            ClampTransformX();
        }

        private void GetMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;

            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0;
            }
        }

        private void ClampTransformX()
        {
            Vector3 position = _transform.position;
            position.x = Mathf.Clamp(position.x, -_clampedPositionX, _clampedPositionX);
            _transform.position = position;
        }
    }
}
