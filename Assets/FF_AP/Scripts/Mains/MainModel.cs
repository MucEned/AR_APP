using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    public class MainModel : MonoBehaviour
    {
        [SerializeField] private List<LayerInformation> layers;
        public void SetScale(float scale)
        {

        }
        public void SetLayersDisplay(bool isActive)
        {
            foreach (LayerInformation layer in layers)
            {
                layer.SetToggleLayerDisplay(isActive);
            }
        }
        public void SetLayerDisplay(int layerIndex, bool isActive)
        
        {
            if (layerIndex >= layers.Count)
                return;
            
            layers[layerIndex].SetToggleLayerDisplay(isActive);
        }
    }   
}