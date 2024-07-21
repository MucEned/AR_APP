using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FF_ArApp
{
    [CreateAssetMenu(menuName = "App Configs/Pages Config")]
    public class PagesConfig : ScriptableObject
    {
        public List<PageData> pageDatas;
        public int GetNumberOfPage()
        {
            return pageDatas.Count;
        }
        public PageData GetPageDataByPageIndex(int pageIndex)
        {
            if (pageIndex >= pageDatas.Count)
                return null;
            return pageDatas[pageIndex];
        }
        public PageData GetPageDataByPageNumber(int pageNumber)
        {
            foreach (PageData pageData in pageDatas)
            {
                if (pageData.PageNumber == pageNumber)
                    return pageData;
            }
            return null;
        }
    }

    [Serializable]
    public class PageData
    {
        public int PageNumber;
        public string PageTitle;
        public string PagContent;
        public MainModel MainModel;
        public bool IsAllowLayers;
    }
}
