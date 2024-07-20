using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace FF_ArApp
{
    public class MainAppUIController : MonoBehaviour
    {
        [SerializeField] private MainAppController appController;
        [SerializeField] private Slider scaleSlider;

        //Header
        [SerializeField] private TMP_Text pageTitle;
        [SerializeField] private CanvasGroup leftPageBtnImg;
        [SerializeField] private CanvasGroup rightPageBtnImg;
        //Container - Layer infor
        [SerializeField] private GameObject layerInfoContainer;
        [SerializeField] private TMP_Text layerName;
        [SerializeField] private TMP_Text layerInformation;
        //Container - Layers toggle
        [SerializeField] private GameObject layersToggleContainer;
        [SerializeField] private UILayerToggle layerToggleSample;
        [SerializeField] private Transform layersView;

        public bool IsCheckingLayer => this.layersToggleContainer.activeSelf;

        private void Start()
        {
            MainEvents.OnLayerTap += ShowLayerInformation;
        }
        private void OnDestroy()
        {
            MainEvents.OnLayerTap -= ShowLayerInformation;
        }
        public void OnMainModelSpawn(MainModel newMainModel)
        {
            CleanLayersView();
            foreach (LayerInformation layer in newMainModel.Layers)
            {
                UILayerToggle newLayerToggle = Instantiate(layerToggleSample, layersView);
                newLayerToggle.Setup(layer);
            }
        }
        private void CleanLayersView()
        {
            foreach (Transform item in layersView)
            {
                Destroy(item.gameObject);
            }
        }
        public void UpdateUIInformation(PageData pageData)
        {
            this.pageTitle.text = pageData.PageTitle;
        }
        public void ShowLayerInformation(LayerInforData data)
        {
            Debug.Log("Ha?");
            if (data == null)
                return;

            this.layerInfoContainer.SetActive(true);
            this.layersToggleContainer.SetActive(false);
            this.layerName.text = data.LayerName;
            this.layerInformation.text = data.LayerDescription;
        }
    
        public void OnScaleSliderValueChange()
        {
            float currentScale = scaleSlider.value;
            this.appController?.OnScaleSliderChange(currentScale);
        }

        public void OnPageChangeBtnClick(int turnPageDir)
        {
            bool isStart = false;
            bool isEnd = false;
            this.appController?.TurnPage(turnPageDir, out isStart, out isEnd);
            SetTurnPageDisplay(isStart, isEnd);
            
        }
        
        private void SetTurnPageDisplay(bool isStart, bool isEnd)
        {
            SetToggleBtn(this.leftPageBtnImg, isStart == false);
            SetToggleBtn(this.rightPageBtnImg, isEnd == false);
        }

        private void SetToggleBtn(CanvasGroup btnImg, bool isToggle)
        {
            btnImg.alpha = isToggle ? 1f : 0.1f;
        }

        public void OnRotationBtnClick(int rotateDir)
        {
            this.appController?.SetRotateDir(rotateDir);
        }

        public void OnPlaceBtnClick()
        {
            this.appController?.PlaceModel();
        }

        public void OnInformationWindowCloseBtnClick()
        {
            this.layerInfoContainer.SetActive(false);
        }

        public void ToggleLayerContainerCloseBtnClick()
        {
            if (this.appController.IsUsingModel)
            {
                bool toggle = !this.layersToggleContainer.activeSelf;
                this.layersToggleContainer.SetActive(toggle);
                if (toggle)
                    this.layerInfoContainer.SetActive(false);
            }
        }
        public void ResetUI()
        {
            this.layersToggleContainer.SetActive(false);
            this.layerInfoContainer.SetActive(false);
        }
        public void ToggleCrosshair(bool toggle)
        {
            this.appController.ToggleCrosshair(toggle);
        }
    }
}