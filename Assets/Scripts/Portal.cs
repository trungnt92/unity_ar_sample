using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector3 camPostionInPortalSpace;

    bool wasInFront;
    bool inOtherWorld;

    bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        whileCameraColliding();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform != Camera.main.transform)
            return;
        wasInFront = GetIsInFront();
        hasCollided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform != Camera.main.transform)
            return;
        wasInFront = GetIsInFront();
        hasCollided = false;
    }

    //Set bidirectional function
    bool GetIsInFront()
    {
        GameObject MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Vector3 worldPos = MainCamera.transform.position + MainCamera.transform.forward * Camera.main.nearClipPlane;
        camPostionInPortalSpace = transform.InverseTransformPoint(worldPos);
        return camPostionInPortalSpace.y >= 0 ? true : false;
    }

    void whileCameraColliding()
    {
        if (!hasCollided)
            return;
        bool isInFront = GetIsInFront();
        if ((isInFront && !wasInFront) || (wasInFront && !isInFront))
        {
            inOtherWorld = !inOtherWorld;
            PortalManager.Instance.UpdateWorld(inOtherWorld);
        }
        wasInFront = isInFront;
    }
}
