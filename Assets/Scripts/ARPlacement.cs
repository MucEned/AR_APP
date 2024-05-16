using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    [SerializeField] private GameObject sampleObject;
    [SerializeField] private Transform placementIndicator;

    private Pose placementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;

    private GameObject spawnedObject;

    [SerializeField] private Material[] materials;
    private MeshRenderer objMeshRenderer;
    private int materialIndex = 0; 

    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
    }
    void Update()
    { 
        // if (sampleObject != null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        // {
        //     PlaceObject();
        // }
        UpdatePlacementPose();
        UpdatePlacementIndicator();
        CheckRotate();
    }

    private void UpdatePlacementPose()
    {
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

    // private void PlaceObject()
    // {
    //     if (spawnedObject == null)
    //     {
    //         spawnedObject = Instantiate(sampleObject, placementPose.position, placementPose.rotation);
    //         spawnedObject.transform.DOPunchScale(Vector3.one * 1.1f, 0.2f, 2); 
    //     }
    // }

    public void OnSpawnBtnDown()
    {
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(sampleObject, placementPose.position, placementPose.rotation);
            objMeshRenderer =  spawnedObject.GetComponentInChildren<MeshRenderer>();
        }
        else
        {
            spawnedObject.transform.position = placementPose.position;
            spawnedObject.transform.rotation = placementPose.rotation;
        }
        spawnedObject.transform.DOPunchScale(Vector3.one * 1.1f, 0.2f, 2); 
    }
    public void OnRotateBtnDown()
    {
        if (spawnedObject == null) return;
        isRotating = true;
    }
    public void OnRotateBtnUp()
    {
        if (spawnedObject == null) return;
        isRotating = false;
    }
    bool isRotating = false;
    private void CheckRotate()
    {
        if (spawnedObject == null) return;
        if (isRotating)
        spawnedObject.transform.Rotate(Vector3.up * 20f * Time.deltaTime);
    }
    public void OnScaleBtnDown()
    {
        if (spawnedObject == null) return;
        spawnedObject.transform.localScale *= 1.05f;
    }
    public void OnChangeBtnDown()
    {
        if (spawnedObject == null) return;
        if (materialIndex >= (materials.Length))
            materialIndex = 0;
        objMeshRenderer.material = materials[materialIndex++];
        // spawnedObject.
    }
}
