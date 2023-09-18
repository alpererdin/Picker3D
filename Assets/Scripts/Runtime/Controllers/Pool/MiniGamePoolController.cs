using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Signals;
using Sirenix.OdinInspector;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace Runtime.Controllers.Pool
{
    public class MiniGamePoolController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
 
        [SerializeField] private TextMeshPro poolText;
        [SerializeField] private byte stageID;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private float3 poolAfterColor = new float3(0.1607843f, 0.3144797f, 0.6039216f);

        #endregion

        #region Private Variables

        [ShowInInspector] private PoolData _data;
        [ShowInInspector] private byte _collectedCount;

        private readonly string _collectable = "Collectable";

        #endregion

        #endregion
 
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
       
            CoreGameSignals.Instance.onMiniGameStageAreaExit += OnChangePoolColor;
        }

     

        private void OnChangePoolColor()
        {
            
            renderer.material.DOColor(new Color(poolAfterColor.x, poolAfterColor.y, poolAfterColor.z, 1), .5f)
                .SetEase(Ease.Flash)
                .SetRelative(false);
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
                return _collectedCount >= _data.RequiredObjectCount;
            }

            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_collectable)) return;
            IncreaseCollectedAmount();
            SetCollectedAmountToPool();
        }

        private void IncreaseCollectedAmount()
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
            if (!other.CompareTag(_collectable)) return;
            DecreaseCollectedAmount();
            SetCollectedAmountToPool();
        }

        private void UnSubscribeEvents()
        {
     
            CoreGameSignals.Instance.onMiniGameStageAreaExit -= OnChangePoolColor;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}