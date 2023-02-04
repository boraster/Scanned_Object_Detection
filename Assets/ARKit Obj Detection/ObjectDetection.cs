using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Vector3 = UnityEngine.Vector3;


public class ObjectDetection : MonoBehaviour
{
    public GameObject trackedObj;
    public ARTrackedObjectManager manager;
    public Camera raycastCamera;
    public TextMeshProUGUI textObj;
    private Bounds objBounds;
    private GameObject detectedObj;
    private bool isObjDetected;
    private GameObject sceneInstance;
    //private ARAnchor anchor;
   // public ARAnchorManager anchorManager;
    public float distanceCheck = 1.0f;

    private void Awake()
    {
        
        manager.trackedObjectsChanged += ObjectDetected;

    }

    private void CreateObjectWithReferencePose(GameObject obj)
    {
        sceneInstance = Instantiate(trackedObj);
        manager.trackedObjectPrefab = sceneInstance;
        manager.trackedObjectPrefab.transform.position =  obj.transform.position;
      manager.trackedObjectPrefab.transform.rotation = obj.transform.rotation;
      
       
    }

    private bool ObjectInsideFrustum(GameObject obj)
    {
        if (isObjDetected)
        {
            var camFrustum = GeometryUtility.CalculateFrustumPlanes(raycastCamera);
            objBounds = obj.GetComponent<SphereCollider>().bounds;
            
            if (GeometryUtility.TestPlanesAABB(camFrustum, objBounds))
            {
                
                return true;
                
            }
            return false;
        }

        return false;
    }
    

    private void ObjectDetected(ARTrackedObjectsChangedEventArgs args)
    {
        for (int i = 0; i < args.added.Count; i++)
        {
            //count.text = args.added.Count.ToString();
            if (args.added[i].referenceObject.name == manager.referenceLibrary[0].name)
            {
                detectedObj = args.added[i].gameObject;
                isObjDetected = true;
                
                CreateObjectWithReferencePose(detectedObj);
                //anchor.transform.position = detectedObj.transform.position;
                //anchor.transform.rotation = detectedObj.transform.rotation;
            }

        }

        for (int i = 0; i < args.updated.Count; i++)
        {
            if (args.updated[i].referenceObject.name == manager.referenceLibrary[0].name)
            {
                detectedObj = args.updated[i].gameObject;
                isObjDetected = true;
               // CreateObjectWithReferencePose(detectedObj);
            }
        }

    }

    private void Update()
    {
        if (ObjectInsideFrustum(sceneInstance))
        {
            textObj.text = manager.referenceLibrary[0].name;
            if (Vector3.Distance(detectedObj.transform.position, sceneInstance.transform.position) > distanceCheck)
            {
                sceneInstance.transform.position = detectedObj.transform.position;
            }
        }
        else
        {
            textObj.text = String.Empty;
        }
    }


}
