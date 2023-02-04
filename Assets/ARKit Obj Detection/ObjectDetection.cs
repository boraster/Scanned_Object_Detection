using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


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
            }

        }

    }

    private void LateUpdate()
    {
        if (ObjectInsideFrustum(sceneInstance))
        {
            textObj.text = manager.referenceLibrary[0].name;
        }
        else
        {
            textObj.text = String.Empty;
        }
    }


}
