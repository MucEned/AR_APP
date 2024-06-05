using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    public class MainAppController : MonoBehaviour
    {
        [SerializeField] private MainAppUIController uiController;
        [SerializeField] private PagesConfig pagesConfig;
        private int currentPageIndex = 0;
        private MainModel currentModel;
        private PageData currentPage;

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