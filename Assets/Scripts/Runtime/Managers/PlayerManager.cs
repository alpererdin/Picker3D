using System;
using Keys;
using Runtime.Commands.Player;
using Runtime.Controllers.Player;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class PlayerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public byte StageValue;
        internal ForceBallsToPoolCommand ForceCommand;

        #endregion

        #region Serialized Variables

        [SerializeField] private PlayerMovementController movementController;
        [SerializeField] private PlayerMeshController meshController;
        [SerializeField] private PlayerPhysicsController physicsController;
            

        #endregion

        #region Private Variables

        private PlayerData _data;

        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPlayerData();
            SendDataToController();
            Init();
        }
   
        private void SendDataToController()
        {
            movementController.SetData(_data.MovementData);
            meshController.SetData(_data.MeshData);
 
        }
        private PlayerData GetPlayerData()
        {
            return Resources.Load<CD_Player>("Data/CD_Player").Data;
            
        }
        private void Init()
        {
            ForceCommand = new ForceBallsToPoolCommand(this,_data.ForceData);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            InputSignals.Instance.onInputTaken += ()=>movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputReleased +=()=>movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged += OnInputDragged;
            UISignals.Instance.onPlay += ()=> movementController.IsReadyToPlay(true);

            CoreGameSignals.Instance.onLevelSuccessful += ()=>movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed += ()=>movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaEntered += ()=>  movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaSuccesful += OnStageAreaSuccessful;
            CoreGameSignals.Instance.onFinishAreaEntered += OnFinishAreaEntered;
            CoreGameSignals.Instance.onReset += OnReset;
        }
    
        private void OnInputDragged(HorizontalInputParams inputParams)
        {
            movementController.UpdateInputParams(inputParams);
        }
        
         private void OnFinishAreaEntered()
        {
            CoreGameSignals.Instance.onLevelSuccessful?.Invoke();
            //minigame
        }

        private void OnStageAreaSuccessful(byte value)
        {
            StageValue = ++value;
        }
        
        private void OnReset()
        {
            StageValue = 0;
            movementController.OnReset();
            physicsController.OnReset();
            meshController.OnReset();
        }

        private void UnSubscribeEvents()
        {
            InputSignals.Instance.onInputTaken -= ()=>movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputReleased -= ()=>movementController.IsReadyToMove(false);
            InputSignals.Instance.onInputDragged -= OnInputDragged;
            
            UISignals.Instance.onPlay -=  ()=> movementController.IsReadyToPlay(true);
             
            CoreGameSignals.Instance.onLevelSuccessful -= ()=>movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onLevelFailed -=()=>movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaEntered -=  ()=>movementController.IsReadyToPlay(false);
            CoreGameSignals.Instance.onStageAreaSuccesful -= OnStageAreaSuccessful;
            CoreGameSignals.Instance.onFinishAreaEntered -= OnFinishAreaEntered;
            CoreGameSignals.Instance.onReset -= OnReset;
        } 
        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}