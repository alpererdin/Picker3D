using DG.Tweening;
using Runtime.Data.ValueObjects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Runtime.Controllers.Player
{
    public class PlayerMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private new Renderer renderer;
        [SerializeField] private TextMeshPro scaleText;
        [SerializeField] private ParticleSystem confetti;
        

        #endregion

        #region Private Variables

        [ShowInInspector] private PlayerMeshData _data;

        #endregion

        #endregion

        private void Awake()
        {
            scaleText.gameObject.SetActive(false);
        }

        internal void SetData(PlayerMeshData data)
        {
            _data = data;
        }

        
        
        internal void ScaleUpPlayer()
        {
            renderer.gameObject.transform.DOScaleX(_data.ScaleCounter, 1f).SetEase(Ease.Flash);
            
        }

        internal void ShowUpText()
        {
            scaleText.gameObject.SetActive(true);
            scaleText.DOFade(1, 0f).SetEase(Ease.Flash).OnComplete(() => scaleText.DOFade(0, 0).SetDelay(.65f));
            scaleText.rectTransform.DOAnchorPosY(.85f, .65f).SetRelative(true).SetEase(Ease.OutBounce).OnComplete(() =>
                scaleText.rectTransform.DOAnchorPosY(-.85f, .65f).SetRelative(true));
        }

        internal void PlayConfetti()
        {
            confetti.Play();
            
        }

        internal void OnReset()
        {
            renderer.gameObject.transform.DOScaleX(1, 1).SetEase(Ease.Linear);
        }
    }
    /*var transform1 = transform;
    confetti.Emit(new ParticleSystem.Particle()
    {
        position = transform.position,
        rotation = transform.rotation,
        velocity = Vector3.zero
                
    });*/
}