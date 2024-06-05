using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    public class MainModel : MonoBehaviour
    {
        [SerializeField] private List<GameObject> layers;
        public void SetLayersDisplay(bool isActive)
        {
            foreach (GameObject layer in layers)
            {
                layer.SetActive(isActive);
            }
        }
        public void SetLayerDisplay(int layerIndex, bool isActive)
        {
            if (layerIndex >= layers.Count)
                return;
            
            layers[layerIndex].SetActive(isActive);
        }
    }   
}