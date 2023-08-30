using UnityEngine;
using UnityEngine.Events;
using Runtime.Extensions;
namespace Runtime.Signals
{
    public class CameraSignals : MonoSingleton<CameraSignals>
    {
         
        
        public UnityAction onSetCameraTarget= delegate { };
    }
}