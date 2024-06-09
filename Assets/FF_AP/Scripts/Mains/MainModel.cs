using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    public class MainModel : MonoBehaviour
    {
        public List<LayerInformation> Layers;
        public void SetScale(float scale)
        {

        }
        public void SetLayersDisplay(bool isActive)
        {
            foreach (LayerInformation layer in Layers)
            {
                layer.SetToggleLayerDisplay(isActive);
            }
        }
        public void SetLayerDisplay(int layerIndex, bool isActive)
        
        {
            if (layerIndex >= Layers.Count)
                return;
            
            Layers[layerIndex].SetToggleLayerDisplay(isActive);
        }
    }   
}