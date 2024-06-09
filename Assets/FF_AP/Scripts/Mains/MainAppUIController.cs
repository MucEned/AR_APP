using UnityEngine.UI;
using UnityEngine;

namespace FF_ArApp
{
    public class MainAppUIController : MonoBehaviour
    {
        [SerializeField] private MainAppController appController;
        [SerializeField] private Slider scaleSlider;
        public void UpdateUIInformation(PageData pageData)
        {

        }
        public void ShowLayerInformation(LayerInforData data)
        {
            if (data == null)
                return;
        }
    
        public void OnScaleSliderValueChange()
        {
            float currentScale = scaleSlider.value;
            this.appController?.OnScaleSliderChange(currentScale);
        }

        public void OnRotationBtnClick(int rotateDir)
        {

        }

        public void OnPlaceBtnClick()
        {

        }

        public void OnInformationWindowCloseBtnClick()
        {

        }

        public void OnLayerContainerCloseBtnClick()
        {
            
        }
    }
}