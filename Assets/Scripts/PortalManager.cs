using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARAnchorManager))]
[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PortalManager : MonoBehaviour
{
    List<ARAnchor> mAnchorPoints;

    ARRaycastManager mRaycastManager;

    ARAnchorManager mAnchorManager;

    ARPlaneManager mPlaneManager;

    List<ARRaycastHit> mHits = new List<ARRaycastHit>();

    public GameObject DoorPrefab;
    public GameObject WorldPrefab;

    private GameObject mWorld;

    public static PortalManager Instance
    {
        get
        {
            return mInstance;
        }
    }

    private static PortalManager mInstance;

    private bool addedDoor = false;

    // Start is called before the first frame update
    void Awake()
    {
        if (mInstance != null)
        {
            Destroy(this);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(this);

        mRaycastManager = GetComponent<ARRaycastManager>();
        mAnchorManager = GetComponent<ARAnchorManager>();
        mPlaneManager = GetComponent<ARPlaneManager>();
        mAnchorPoints = new List<ARAnchor>();
    }

    // Update is called once per frame
    void Update()
    {
        // If there is no tap, then simply do nothing until the next call to Update().
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        if (addedDoor)
            return;

        if (mRaycastManager.Raycast(touch.position, mHits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = mHits[0].pose;
            var hitTrackableId = mHits[0].trackableId;
            var hitPlane = mPlaneManager.GetPlane(hitTrackableId);

            // This attaches an anchor to the area on the plane corresponding to the raycast hit,
            // and afterwards instantiates an instance of your chosen prefab at that point.
            // This prefab instance is parented to the anchor to make sure the position of the prefab is consistent
            // with the anchor, since an anchor attached to an ARPlane will be updated automatically by the ARAnchorManager as the ARPlane's exact position is refined.
            var anchor = mAnchorManager.AttachAnchor(hitPlane, hitPose);


            if (anchor == null)
            {
                Debug.Log("Error creating anchor.");
            }
            else
            {
                Debug.Log("Adding anchor.");

                // Stores the anchor so that it may be removed later.
                mAnchorPoints.Add(anchor);

                var door = Instantiate(DoorPrefab, hitPose.position, DoorPrefab.transform.rotation, anchor.transform);
                var targetPosition = new Vector3(Camera.main.transform.position.x,
                    door.transform.position.y,
                    Camera.main.transform.position.z);
                door.transform.LookAt(targetPosition);
                door.SetActive(true);

                mWorld = Instantiate(WorldPrefab, anchor.transform);
                mWorld.SetActive(true);

                addedDoor = true;
            }
        }
    }


    public void UpdateWorld(bool inOtherWorld)
    {
        if (mWorld == null)
            return;
        int children = mWorld.transform.childCount;
        int defaultLayer = LayerMask.NameToLayer("WithinPortal");
        int insidePortalLayer = LayerMask.NameToLayer("InsidePortal");
        for (int i = 0; i < children; ++i)
        {
            if (inOtherWorld)
            {
                // reset layer to default
                mWorld.transform.GetChild(i).gameObject.layer = defaultLayer;
            }
            else
            {
                // reset layer to default
                mWorld.transform.GetChild(i).gameObject.layer = insidePortalLayer;
            }
        }
    }
}
