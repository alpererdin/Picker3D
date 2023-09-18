using System;
using System.Collections.Generic;
using Interfaces;
using Runtime.Commands.Input;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Keys;
using Runtime.Signals;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Managers
{
    public class InputManager : MonoBehaviour
    {
        #region Self Variables

        #region Private Variables
        
        private ICommand _onInputTakenCommand;
        private ICommand _onInputReleasedCommand;
        private ICommand _onInputDraggedCommand;

        [ShowInInspector] private InputData _data;
        [ShowInInspector] private bool _isAvailableForTouch, _isFirstTimeTouchTaken, _isTouching;

        private float _currentVelocity;
        private float3 _moveVector;
        private Vector2? _mousePosition;

        #endregion

        #endregion

        private void Start()
        {
            _onInputTakenCommand = new OnInputTakenCommand();
            _onInputReleasedCommand = new OnInputReleasedCommand();
            _onInputDraggedCommand = new OnInputDraggedCommand(_data);  
        }

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
        }

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
            _isAvailableForTouch = false;
            //_isFirstTimeTouchTaken = false;
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
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                _onInputTakenCommand.Execute();
            }
            else if (Input.GetMouseButtonUp(0) && !IsPointerOverUIElement())
            {
                _onInputReleasedCommand.Execute();
            }
            else if (Input.GetMouseButton(0) && !IsPointerOverUIElement())
            {
                _onInputDraggedCommand.Execute();
            }
        }

        private bool IsPointerOverUIElement()
        {
            var eventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0;
        }
    }
    
}