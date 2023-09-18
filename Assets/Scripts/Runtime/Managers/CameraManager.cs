using Cinemachine;
using Runtime.Signals;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using System;
using Runtime.Enums;
using Enums;
using CameraState = Runtime.Enums.CameraState;


namespace Runtime.Managers
{
    public class CameraManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

       // [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        [ShowInInspector] private float3 _initialPosition;

        #endregion

        #endregion

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            _initialPosition = transform.position;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CameraSignals.Instance.onSetCameraTarget += OnSetCameraTarget;
            CoreGameSignals.Instance.onReset += OnReset;
            CameraSignals.Instance.onChangeCameraState += OnChangeCameraState;
        }
        
        private void OnSetCameraTarget()
        {
            var player = FindObjectOfType<PlayerManager>().transform;
            stateDrivenCamera.Follow = player;
            //virtualCamera.LookAt = player;
        }
   
        private void OnChangeCameraState(CameraState state)
        {
            animator.SetTrigger(state.ToString());
        }
        
        private void UnSubscribeEvents()
        {
            CameraSignals.Instance.onSetCameraTarget -= OnSetCameraTarget;
            CoreGameSignals.Instance.onReset -= OnReset;
            CameraSignals.Instance.onChangeCameraState -= OnChangeCameraState;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void OnReset()
        {
             CameraSignals.Instance.onChangeCameraState?.Invoke(CameraState.Follow);
            transform.position = _initialPosition;
        }
    }
}