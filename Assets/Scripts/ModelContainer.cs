using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ModelContainer : MonoBehaviour
{
    private Tweener animationTween;
    public GameObject heart;
    public GameObject lung;
    private float scale = 1f;
    public void Swap()
    {
        heart.SetActive(!heart.activeSelf);
        lung.SetActive(!lung.activeSelf);
        if (animationTween != null)
        {
            animationTween.Kill();
            this.transform.localScale = scale * Vector3.one;
        }
        animationTween = this.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2); 
    }

    public void SaveScale(float scale)
    {
        this.scale = scale;
    }

    public void OnActive()
    {
        heart.gameObject.SetActive(true);
        lung.gameObject.SetActive(true);
    }

    public void ToggleLung()
    {
        lung.gameObject.SetActive(!lung.activeSelf);
    }
    public void ToggleHeart()
    {
        heart.gameObject.SetActive(!heart.activeSelf);
    }
}
