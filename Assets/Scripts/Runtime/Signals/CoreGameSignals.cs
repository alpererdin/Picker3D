using System;
using Runtime.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Signals
{
    public class CoreGameSignals : MonoSingleton<CoreGameSignals>
    {
        
        
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