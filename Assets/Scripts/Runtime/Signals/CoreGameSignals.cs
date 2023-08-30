using System;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoBehaviour
    {
        #region Singleton

        public static CoreGameSignals Instance;
        
        private void Awake()
        {
            if (Instance != this && Instance !=null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }
  

        #endregion
        
        public UnityAction<byte> onLevelInitialize=delegate {  };
        public UnityAction onClearActiveLevel=delegate {  };
        
        public UnityAction onLevelSuccessful = delegate { };
        public UnityAction onLevelFailed=delegate {  };
        
        public UnityAction onNextLevel = delegate { };
        public UnityAction onRestartLevel = delegate { };
        public UnityAction onPlay = delegate { };
        public UnityAction onReset = delegate { };
        
        public Func<byte> onGetLevelValue = delegate { return 0; };
        public UnityAction onStageAreaEntered = delegate {  };
        public UnityAction<byte> onStageAreaSuccesful = delegate {  };
        public UnityAction onFinishAreaEntered = delegate {  };
    }
}