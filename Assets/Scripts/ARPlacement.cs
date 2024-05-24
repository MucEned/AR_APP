using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    [SerializeField] private ModelContainer sampleObject;
    [SerializeField] private Transform placementIndicator;
    [SerializeField] private Slider scaleSlider;
    [SerializeField] private float rotationSpeed = 40f;

    private Pose placementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private ModelContainer spawnedObject;

    private MeshRenderer objMeshRenderer;
    private int materialIndex = 0;

    private int rotationDir = 0;

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    void Update()
    { 
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     spawnedObject = Instantiate(sampleObject, Vector3.zero, Quaternion.identity);
        //     spawnedObject.OnActive();
        // }
        CheckRotate();
        // return;
        UpdatePlacementPose();
        UpdatePlacementIndicator();
    }

    private void UpdatePlacementPose()
    {
        if (Camera.current == null)
            return;
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
        }
    }

    private void UpdatePlacementIndicator()
    {
        if (sampleObject != null && placementPoseIsValid)
        {
            placementIndicator.gameObject.SetActive(true);
            placementIndicator.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placementIndicator.gameObject.SetActive(false);
        }
    }

    private Tweener animationTween;
    public void OnSpawnBtnDown()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(sampleObject, placementPose.position, placementPose.rotation);
        }
        else
        {
            spawnedObject.transform.position = placementPose.position;
            spawnedObject.transform.rotation = placementPose.rotation;
        }

        if (animationTween != null)
        {
            animationTween.Kill();
            spawnedObject.transform.localScale = currentScale * Vector3.one;
        }
        animationTween = spawnedObject.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 2); 
        spawnedObject.OnActive();
    }

    public void OnRotateBtnDown(int dir)
    {
        if (spawnedObject == null) return;
        rotationDir = dir;
        isRotating = true;
    }

    public void OnRotateBtnUp()
    {
        if (spawnedObject == null) return;
        isRotating = false;
    }

    bool isRotating = false;
    float currentScale = 1;
    private void CheckRotate()
    {
        if (spawnedObject == null) return;
        if (rotationDir == 0) return;
        if (isRotating)
        spawnedObject.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime * rotationDir);
    }

    public void OnScaleBSliderValueChange()
    {
        if (spawnedObject == null) return;
        currentScale = scaleSlider.value;
        spawnedObject.transform.localScale = Vector3.one * currentScale;
        spawnedObject.SaveScale(currentScale);
    }

    public void OnLungButtonDown()
    {
        if (spawnedObject == null) return;
        spawnedObject.ToggleLung();
    }
    public void OnHeartButtonDown()
    {
        if (spawnedObject == null) return;
        spawnedObject.ToggleHeart();
    }
}
