using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

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


        private int currentPageIndex = 0;
        private MainModel currentModel;
        private PageData currentPage;
        private List<ARRaycastHit> hits;
        private Camera mainCam;



        private bool placementPoseIsValid = false;
        private Pose placementPose;




        private MainModel mainModel;
        private int rotationDir = 0;
        bool isRotating = false;
        float currentScale = 1;



        private void Start()
        {
            this.mainCam = Camera.main;
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

        private void CheckoutItemInfo(GameObject item)
        {
            Debug.Log(item.name);
            item.transform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
        }

        public void TurnPage(int turnDir) //1 is next, -1 is back
        {
            int nextPageIndex = this.currentPageIndex + turnDir;
            ChangePageByIndex(nextPageIndex);
        }
        private void ChangePageByIndex(int pageIndex)
        {
            this.currentPageIndex = pageIndex;
            this.currentPage = this.pagesConfig.GetPageDataByPageIndex(this.currentPageIndex);
            OnPageUpdate();
        }
        private void OnPageUpdate()
        {
            RemoveModel();
            UpdateUIInformation();
        }
        private void RemoveModel()
        {
            Destroy(this.currentModel);
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
    }
}