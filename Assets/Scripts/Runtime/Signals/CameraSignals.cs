using Runtime.Extensions;
using UnityEngine.Events;
using Runtime.Enums;

namespace Runtime.Signals
{
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
        public UnityAction onSetCameraTarget = delegate { };
        public UnityAction<CameraState> onChangeCameraState = delegate { };
    }
}