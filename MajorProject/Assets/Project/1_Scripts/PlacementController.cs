using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementController : MonoBehaviour
{
    [SerializeField]
    private GameObject placedPrefab;

    private GameObject spawnedPrefab;
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager planeManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }



    private void Update()
    {

        if(arRaycastManager.Raycast(new Vector2(Input.touches[0].position.x, Input.touches[0].position.y), hits, trackableTypes:TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (spawnedPrefab == null)
            {
                spawnedPrefab = Instantiate(placedPrefab, hitPose.position, hitPose.rotation);
                foreach (var plane in planeManager.trackables)
                {
                    plane.gameObject.SetActive(false);
                }
            }
            else
                spawnedPrefab.transform.position = hitPose.position;
        }
    }

}
