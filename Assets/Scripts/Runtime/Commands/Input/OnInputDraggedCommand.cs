using Interfaces;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Runtime.Commands.Input
{
    public class OnInputDraggedCommand : ICommand
    {
        private readonly InputData _data;
        private Vector2 _mousePosition;
        private Vector2 _moveVector;
        private float _currentVelocity;

        public OnInputDraggedCommand(InputData data)
        {
            _data = data;
        }

        public void Execute()
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseDeltaPos = currentMousePosition - _mousePosition;

            if (mouseDeltaPos.x > _data.HorizontalInputSpeed)
                _moveVector.x = _data.HorizontalInputSpeed / 2f * mouseDeltaPos.x;
            else if (mouseDeltaPos.x < -_data.HorizontalInputSpeed)
                _moveVector.x = -_data.HorizontalInputSpeed / 2f * -mouseDeltaPos.x;
            else
                _moveVector.x = Mathf.SmoothDamp(_moveVector.x, 0f, ref _currentVelocity, _data.ClampSpeed);

            _moveVector.x = mouseDeltaPos.x;

            _mousePosition = currentMousePosition;

            InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
            {
                HorizontalValue = _moveVector.x,
                ClampValues = _data.ClampValues
            });
        }

        public void Execute(int value)
        {
            throw new System.NotImplementedException();
        }
    }
}