﻿using DG.Tweening;
using Runtime.Controllers.Pool;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PlayerManager manager;
        [SerializeField] private new Collider collider;
        [SerializeField] private new Rigidbody rigidbody;

        #endregion

        #region Private Variables

        private readonly string _stageArea = "StageArea";
        private readonly string _finish = "FinishArea";
        private readonly string _miniGame = "MiniGameArea";
        private readonly string _miniGameStageArea = "MiniGameStage";

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
         
            if (other.CompareTag(_stageArea))
            {
             
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onStageAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();

                DOVirtual.DelayedCall(3, () =>
                {
                    var result = other.transform.parent.GetComponentInChildren<PoolController>()
                        .TakeResults(manager.StageValue);

                    if (result)
                    {
                        CoreGameSignals.Instance.onStageAreaSuccessful?.Invoke(manager.StageValue);
                        InputSignals.Instance.onEnableInput?.Invoke();
                    }
                    else
                    {
                        CoreGameSignals.Instance.onLevelFailed?.Invoke();
                    }
                });
                return;
            }

            if (other.CompareTag(_finish))
            {
                CoreGameSignals.Instance.onFinishAreaEntered?.Invoke();
                InputSignals.Instance.onDisableInput?.Invoke();
                return;
            }

            if (other.CompareTag(_miniGame))
            {
                CoreGameSignals.Instance.onMiniGameAreaEntered?.Invoke();
                //+
                DOVirtual.DelayedCall(.5f,
                    () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraState.MiniGame));
                DOVirtual.DelayedCall(3f,
                    () => CameraSignals.Instance.onChangeCameraState?.Invoke(CameraState.Follow));
                return;
            }

            if (other.CompareTag(_miniGameStageArea))
            {
                manager.ForceCommand.Execute();
                CoreGameSignals.Instance.onMiniGameStageAreaEntered?.Invoke();
               // InputSignals.Instance.onDisableInput?.Invoke();
               DOVirtual.DelayedCall(3, () =>
               {
                       CoreGameSignals.Instance.onMiniGameStageAreaExit?.Invoke();
                       InputSignals.Instance.onEnableInput?.Invoke();
                   
               });
               return;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform1 = manager.transform;
            var position1 = transform1.position;

            Gizmos.DrawSphere(new Vector3(position1.x, position1.y - 1f, position1.z + .9f), 1.7f);
        }

        public void OnReset()
        {
        }
    }
}