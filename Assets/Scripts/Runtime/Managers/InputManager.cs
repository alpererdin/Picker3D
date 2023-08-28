using System;
using System.Collections.Generic;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Keys;
using Runtime.Signals;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables

        private InputData _data;
        private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity;
        private float3 _moveVector;
        private Vector2? _mousePosition;


        #endregion

        #endregion

        private void Awake()
        {
            _data = GetInputData();
        }

        private InputData GetInputData()
        {
            return Resources.Load<CD_Input>("Data/CD_Input").Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onReset += OnReset;
            InputSignals.Instance.onEnableInput += OnEnableInput;
            InputSignals.Instance.onDisableInput += OnDisableInput;
            //InputSignals.Instance.onInputStateChanged += OnInputStateChanged;
            

        }

       /* private void OnInputStateChanged(bool state)
        {
            _isAvailableForTouch = state;
        }*/

        private void OnDisableInput()
        {
            _isAvailableForTouch = false;

        }
        private void OnEnableInput()
        {
            _isAvailableForTouch = true;

        }

        private void OnReset()
        {
            _isAvailableForTouch=false;
             //_isFirstTimeTouchTaken =false;
             _isTouching = false;
        }


        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onReset -= OnReset;
            InputSignals.Instance.onEnableInput -= OnEnableInput;
            InputSignals.Instance.onDisableInput -= OnDisableInput;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Update()
        {
            if (!_isAvailableForTouch) return;

            if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElemet())
            {
                _isTouching = false;
                InputSignals.Instance.onInputReleased?.Invoke();
                Debug.LogWarning("Executed --->  OnInputReleased");
                
            }

            if (Input.GetMouseButtonDown(0)&& !IsPointerOverUIElemet())
            {
                _isTouching = true;
                InputSignals.Instance.onInputTaken?.Invoke();
                Debug.LogWarning("Executed --->  OnInputTaken");
                if (!_isFirstTimeTouchTaken)
                {
                    _isFirstTimeTouchTaken = true;
                    InputSignals.Instance.onFirstTimeTouchTaken?.Invoke();
                    Debug.LogWarning("Executed --->  OnFirstTimeTouchTaken");
                }

                _mousePosition = Input.mousePosition;

            }
            if(Input.GetMouseButton(0)&& !IsPointerOverUIElemet())
            {
                if (_isTouching)
                {
                    if (_mousePosition != null)
                    {
                        Vector2 mouseDeltaPos = (Vector2)Input.mousePosition - _mousePosition.Value;
                        if (mouseDeltaPos.x > _data._HorizontalInputSpeed)
                        {
                            _moveVector.x = _data._HorizontalInputSpeed /  10f * mouseDeltaPos.x;
                        }else if (mouseDeltaPos.x < _data._HorizontalInputSpeed)
                        {
                            _moveVector.x = -_data._HorizontalInputSpeed / 10f * mouseDeltaPos.x;
                        }
                        else
                        {
                            _moveVector.x = Mathf.SmoothDamp(-_moveVector.x, 0f, ref _currentVelocity,
                                _data.ClampSpeed);
                        }

                        _mousePosition = Input.mousePosition;

                        InputSignals.Instance.onInputDragged?.Invoke(new HorizontalInputParams()
                        {
                            HorizontalValue = _moveVector.x,
                                ClampValues=_data.ClampValues
                        });
                    }
                }
            }
        }

        private bool IsPointerOverUIElemet()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position =Input.mousePosition
            };
             
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData,results);
            return results.Count > 0;
        }
    }
}