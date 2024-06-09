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
        private int currentPageIndex = 0;
        private MainModel currentModel;
        private PageData currentPage;
        private List<ARRaycastHit> hits;

        void Update()
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                RaycastHit tapHit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out tapHit, Mathf.Infinity))
                {
                    Debug.Log(tapHit.collider.gameObject.name);
                    tapHit.collider.transform.DOPunchScale(Vector3.one * 1.1f, 0.25f);
                }
            }
            // ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // if (Input.GetMouseButtonDown(0))
            // {
            //     if (Physics.Raycast(ray, out tapHit, Mathf.Infinity))
            //     {
            //         Debug.Log(tapHit.collider.gameObject.name);
            //     }
            // }
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
    }
}