using System.Collections.Generic;
using Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Controllers.UI
{
    public class UIPanelController : MonoBehaviour
    {
        #region SelfVariables

        #region Serialized Variables

        [SerializeField] private List<Transform> layers = new List<Transform>();

        #endregion

        #endregion

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanel;
        }
        [Button(name:"close ALL panel")]
        public void OnCloseAllPanel()
        {
            foreach (var layer in layers)
            {
                if(layer.childCount <= 0 )return;
#if UNITY_EDITOR
                DestroyImmediate(layer.GetChild(0).gameObject);
#else
                Destroy(layer.GetChild(0).gameObject);
#endif
                
            }
        }
        [Button(name:"open panel")]
        public void OnOpenPanel(UIPanelTypes panelType,int value)
        {
            OnClosePanel(value);
            Instantiate(Resources.Load<GameObject>($"Screens/{panelType}Panel"),layers[value]);
        }

        [Button(name:"close panel")]
        private void OnClosePanel(int value)
        {
            if (layers[value].childCount <= 0) return;
            
#if UNITY_EDITOR
                DestroyImmediate(layers[value].GetChild(0).gameObject);
#else
  
              Destroy(layers[value].GetChild(0).gameObject);
#endif
            

        }
        private void UnSubscribeEvents()
        {
            CoreUISignals.Instance.onClosePanel += OnClosePanel;
            CoreUISignals.Instance.onOpenPanel += OnOpenPanel;
            CoreUISignals.Instance.onCloseAllPanels += OnCloseAllPanel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}