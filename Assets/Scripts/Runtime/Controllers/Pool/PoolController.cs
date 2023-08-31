using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using Sirenix.OdinInspector;
using Unity.VisualScripting;

namespace Runtime.Controllers.Pool
{
    public class PoolController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private List<DOTweenAnimation> tweens = new List<DOTweenAnimation>();
        [SerializeField] private TextMeshPro poolText;
        [SerializeField] private byte stageID;
        [SerializeField] private new Renderer renderer;

        #endregion

        #region Private Variables

        [ShowInInspector] private PoolData _data;
        [ShowInInspector] private byte _collectedCount;

        private readonly string _collectable = "Collectable";
        #endregion

        #endregion

        private void Awake()
        {
            _data = GetPoolData();
        }

        private PoolData GetPoolData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level")
                .Levels[(int)CoreGameSignals.Instance.onGetLevelValue?.Invoke()].PoolList[stageID];
        }

      

        private void OnEnable()
        {
            SubscribeEvents();
            
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaSuccessful += OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful += OnChangePoolColor;

        }
        private void OnActivateTweens(byte stageValue)
        {
            if (stageValue != stageID) return;
            renderer.material.DOColor(new Color(0.1607842f, 0.6039216f, 0.1766218f), 1).SetEase(Ease.Linear);
        }
        private void OnChangePoolColor(byte stageValue)
        {
            if (stageValue!=stageID)return;
            
                foreach (var tween in tweens)
                {
                    tween.DOPlay(); 
                }
            
        }

      

        private void Start()
        {
            SetRequiredAmountText();
        }

        private void SetRequiredAmountText()
        {
            poolText.text = $"0/{_data.RequiredObjectCount}";
        }  
        public bool TakeResults(byte managerStageValue)
        {
            if (stageID == managerStageValue)
            {
                return _collectedCount >=_data.RequiredObjectCount;
            }

            return false;
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!other.CompareTag(_collectable))return;
            IncreaseCollectedAmaunt();
            SetCollectedAmountToPool();
        }
        private void IncreaseCollectedAmaunt()
        {
            _collectedCount++;
        }
        private void SetCollectedAmountToPool()
        {
            poolText.text = $"{_collectedCount}/{_data.RequiredObjectCount}";
        }

        private void DecreaseCollectedAmount()
        {
            _collectedCount--;
        }
        private void OnTriggerExit(Collider other)
        {
            if(!other.CompareTag(_collectable))return;
            DecreaseCollectedAmount();
            SetCollectedAmountToPool();
        }
        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnActivateTweens;
            CoreGameSignals.Instance.onStageAreaSuccessful -= OnChangePoolColor;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

     
        
    }
}