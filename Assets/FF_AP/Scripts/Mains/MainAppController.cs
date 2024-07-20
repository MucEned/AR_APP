using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;

namespace FF_ArApp
{
    public class MainAppController : MonoBehaviour
    {
        [SerializeField] private MainAppUIController uiController;
        [SerializeField] private PagesConfig pagesConfig;
        [SerializeField] public ARRaycastManager raycastManager;
        [SerializeField] private float rotationSpeed = 40f;
        [SerializeField] private MainModel sampleModel;
        [SerializeField] private Transform placementIndicator;
        [SerializeField] private GameObject crosshair;
        [SerializeField] private bool editorMode = true;

        public bool IsUsingModel => this.mainModel != null && this.mainModel.gameObject != null;


        private int currentPageIndex = 0;
        private MainModel mainModel;
        private PageData currentPage;
        private List<ARRaycastHit> hits;
        private Camera mainCam;



        private bool placementPoseIsValid = false;
        private Pose placementPose;



        private int rotationDir = 0;
        bool isRotating = false;
        float currentScale = 1;

        private Tweener animationTween;



        private void Start()
        {
            this.mainCam = Camera.main;
            this.currentPageIndex = 0;
            ChangePageByIndex(this.currentPageIndex);
        }

        private void Update()
        {
            CheckTapAction();
            CheckRotate();
            UpdatePlacementPose();
            UpdatePlacementIndicator();
        }

        private void CheckTapAction()
        {
            if (this.mainCam == null)
                return;

            RaycastHit tapHit;
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = this.mainCam.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out tapHit, Mathf.Infinity))
                {
                    CheckoutItemInfo(tapHit.collider.gameObject);
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = this.mainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out tapHit, Mathf.Infinity))
                {
                    CheckoutItemInfo(tapHit.collider.gameObject);
                }
            }
        }

        private void CheckRotate()
        {
            if (mainModel == null) return;
            if (rotationDir == 0) return;
            if (isRotating)
                mainModel.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * rotationDir);
        }
        private void UpdatePlacementPose()
        {
            if (Camera.current == null)
                return;
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            this.raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;
            }
        }

        private void UpdatePlacementIndicator()
        {
            if (sampleModel != null && placementPoseIsValid)
            {
                placementIndicator.gameObject.SetActive(true);
                placementIndicator.SetPositionAndRotation(placementPose.position, placementPose.rotation);
            }
            else
            {
                placementIndicator.gameObject.SetActive(false);
            }
        }

        Tweener animTween;
        private void CheckoutItemInfo(GameObject item)
        {
            if (uiController.IsCheckingLayer)
                return;
            Debug.Log(item.name);
            if (animTween != null) 
                animTween.Kill();
            item.transform.localScale = Vector3.one;
            // animTween = item.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f);

            LayerInformation layerInfo = item.GetComponentInParent<LayerInformation>();
            if (layerInfo != null)
            {
                MainEvents.OnLayerTap?.Invoke(layerInfo.Data);
            }
        }

        public void TurnPage(int turnDir, out bool isStart, out bool isEnd) //1 is next, -1 is back
        {
            int nextPageIndex = this.currentPageIndex + turnDir;
            isStart = (nextPageIndex <= 0);
            isEnd = nextPageIndex >= (pagesConfig.GetNumberOfPage() - 1);
            if (nextPageIndex < pagesConfig.GetNumberOfPage() && nextPageIndex >= 0)
            {
                ChangePageByIndex(nextPageIndex);
            }
        }
        private void ChangePageByIndex(int nextPageIndex)
        {
            PageData nextPage = this.pagesConfig.GetPageDataByPageIndex(nextPageIndex);
            this.currentPageIndex = nextPage == null ? this.currentPageIndex : nextPageIndex;
            this.currentPage = nextPage == null ? this.currentPage : nextPage;
            OnPageUpdate();
        }
        private void OnPageUpdate()
        {
            RemoveModel();
            UpdateUIInformation();
        }
        private void RemoveModel()
        {
            if (this.mainModel == null || this.mainModel.gameObject == null)
                return;
            Destroy(this.mainModel.gameObject);
        }
        private void UpdateUIInformation()
        {
            this.uiController.UpdateUIInformation(this.currentPage);
        }
        public void OnScaleSliderChange(float currentScale)
        {
            if (mainModel == null) return;
            mainModel.transform.localScale = Vector3.one * currentScale;
            mainModel.SetScale(currentScale);
        }



        //Commands
        public void SetRotateDir(int newRotateDir)
        {
            if (newRotateDir == 0)
                isRotating = false;
            else
                isRotating = true;
            this.rotationDir = newRotateDir;
        }
        public void ToggleCrosshair(bool toggle)
        {
            if (this.crosshair.activeSelf == toggle)
                return;
            this.crosshair.SetActive(toggle);
        }
        public void PlaceModel()
        {
            this.sampleModel = this.currentPage.MainModel;
            if (sampleModel == null)
                return;
                
            if (this.mainModel == null)
            {
                if (editorMode == false)
                    this.mainModel = Instantiate(sampleModel, placementPose.position, placementPose.rotation);
                else
                    this.mainModel = Instantiate(sampleModel, Vector3.zero, Quaternion.identity);
            }
            else
            {
                if (editorMode == false)
                {
                    this.mainModel.transform.position = placementPose.position;
                    this.mainModel.transform.rotation = placementPose.rotation;
                }
                else
                {
                    this.mainModel.transform.position = Vector3.zero;
                    this.mainModel.transform.rotation = Quaternion.identity;
                }
            }

            if (animationTween != null)
            {
                animationTween.Kill();
                this.mainModel.transform.localScale = currentScale * Vector3.one;
            }
            animationTween = this.mainModel.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2);

            this.mainModel.SetLayersDisplay(true);

            if (this.mainModel != null)
                this.uiController.OnMainModelSpawn(this.mainModel);
        }
    }
}