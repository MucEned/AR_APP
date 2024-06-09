using UnityEngine.UI;
using UnityEngine;

namespace FF_ArApp
{
    public class UILayerToggle : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        [SerializeField] private Text layerName;
        private LayerInformation linkedLayer;
        public void Setup(LayerInformation layer)
        {
            this.linkedLayer = layer;
            this.layerName.text = layer.Data.LayerName;
        }
        public void OnToggle()
        {
            this.linkedLayer.SetToggleLayerDisplay(toggle.isOn);
        }
    }
}