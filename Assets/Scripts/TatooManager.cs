using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TatooManager : MonoBehaviour
{
    private static TatooManager mInstance;
    [HideInInspector]
    public static TatooManager Instance
    {
        get
        {
            return mInstance;
        }
    }

    private ARTrackedImageManager mManager;

    [SerializeField]
    private Camera arCamera;

    public GameObject CameraCanvas;
    private RenderTexture mCameraTex;

    private TrackImage mSelectedTatoo;
    private TrackImage mSelectedPrefabs;

    private void Awake()
    {
        if (Instance == null)
        {
            mInstance = this;
            mManager = GetComponent<ARTrackedImageManager>();
            DontDestroyOnLoad(this);
            return;
        }
        Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        mManager.trackedImagesChanged += TrackedImagesChanged;
    }

    private void TrackedImagesChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (var image in obj.added)
        {
            ChangeTrackImage(image);
        }

        foreach (var image in obj.updated)
        {
            UpdateTrackImage(image);
        }
    }

    private void OnDisable()
    {
        mManager.trackedImagesChanged -= TrackedImagesChanged;
    }

    public void SetTrackImage(TrackImage trackImage)
    {
        mSelectedTatoo = trackImage;
        //if (mManager.referenceLibrary == null)
        //{
        //    Debug.Log("Create run time lib");
            mManager.referenceLibrary  = mManager.CreateRuntimeLibrary();
        //}

        StartCoroutine(AddImage(trackImage));

        //if (mManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        //{
        //    Debug.Log("Add runtime image");
        //    var jobHandle = mutableLibrary.ScheduleAddImageWithValidationJob(
        //        trackImage.Image,
        //        trackImage.Name,
        //        0.1f /* 20 cm */).jobHandle;
        //    while (!jobHandle.IsCompleted)
        //    {
        //        Debug.Log("Adding images");
        //    }

        //    jobHandle.Complete();

        //    Debug.LogFormat("Added image: %d", mutableLibrary.count);
        //}
    }

    IEnumerator AddImage(TrackImage trackImage)
    {
        if (mManager.referenceLibrary is MutableRuntimeReferenceImageLibrary mutableLibrary)
        {
            Debug.Log("Add runtime image");
            var jobHandle = mutableLibrary.ScheduleAddImageWithValidationJob(
                trackImage.Image,
                trackImage.Name,
                0.1f /* 20 cm */).jobHandle;
            //while (!jobHandle.IsCompleted)
            //{
                Debug.Log("Adding images");
                yield return new WaitUntil(() => jobHandle.IsCompleted);
            //}

            //jobHandle.Complete();

            Debug.LogFormat("Added image: " + mutableLibrary.count);
        }
    }

    public void SetPrefab(TrackImage trackImage)
    {
        mSelectedPrefabs = trackImage;
    }

    void ChangeTrackImage(ARTrackedImage trackedImage)
    {
        Debug.Log("Got ar image");
        // Set the texture
        var material = trackedImage.GetComponentInChildren<MeshRenderer>().material;
        material.mainTexture = mSelectedPrefabs.Image;
    }

    void UpdateTrackImage(ARTrackedImage image)
    {
        switch (image.trackingState)
        {
            case TrackingState.Tracking:
                image.gameObject.SetActive(true);
                break;
            case TrackingState.None:
            case TrackingState.Limited:
                image.gameObject.SetActive(false);
                break;
        }
    }
}
