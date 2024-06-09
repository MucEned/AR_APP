using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    public class LayerInformation : MonoBehaviour
    {
        public LayerInforData Data;
        public GameObject LayerDisplay;
        private MainAppUIController uIController;
        private void OnTap()
        {
            if (Data == null)
                return;
                
            ShowInformation();
        }
        private void ShowInformation()
        {
            this.uIController.ShowLayerInformation(Data);
        }
        public void SetToggleLayerDisplay(bool isActive)
        {
            this.LayerDisplay.SetActive(isActive);
        }
    }
    [Serializable]
    public class LayerInforData
    {
        public string LayerName;
        public string LayerDescription;
    }
}