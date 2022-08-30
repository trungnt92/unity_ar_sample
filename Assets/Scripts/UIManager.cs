using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackImage
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public Texture2D Image;
}

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    [HideInInspector]
    public static UIManager Instance
    {
        get
        {
            return mInstance;
        }
    }


    [Header("Tatoos Settings")]
    public List<TrackImage> TatooTracks;
    public List<TrackImage> TatooPrefabs;

    public GameObject TatoosGrid;
    public GameObject TatoosView;
    public SelectionButton TatooButtonPrefabs;

    public GameObject PrefabGrid;
    public GameObject PrefabView;


    private List<SelectionButton> mBtnTatoos = new List<SelectionButton>();
    private List<SelectionButton> mBtnPrefabs = new List<SelectionButton>();

    private void Awake()
    {
        if (Instance == null)
        {
            mInstance = this;
            DontDestroyOnLoad(this);
            InitUI();
            return;
        }
        Destroy(this);
    }

    private void InitUI()
    {
        foreach (var tatoo in TatooTracks)
        {
            var button = Instantiate<SelectionButton>(TatooButtonPrefabs, TatoosGrid.transform);
            button.SetImage(tatoo);
            button.OnButtonClicked += OnTatooClicked;
            mBtnTatoos.Add(button);
        }

        foreach (var tatoo in TatooPrefabs)
        {
            var button = Instantiate<SelectionButton>(TatooButtonPrefabs, PrefabGrid.transform);
            button.SetImage(tatoo);
            button.OnButtonClicked += OnPrefabClicked;
            mBtnPrefabs.Add(button);
        }

        //mBtnTatoos[0].Select();
        //TatooManager.Instance.SetTrackImage(mBtnTatoos[0].TrackImage);
        //mBtnPrefabs[0].Select();
        //TatooManager.Instance.SetPrefab(mBtnPrefabs[0].TrackImage);
    }

    private void OnTatooClicked(SelectionButton button)
    {
        foreach(var btn in mBtnTatoos)
        {
            btn.UnSelect();
        }
        button.Select();
        TatooManager.Instance.SetTrackImage(button.TrackImage);
    }

    private void OnPrefabClicked(SelectionButton button)
    {
        foreach (var btn in mBtnPrefabs)
        {
            btn.UnSelect();
        }
        button.Select();
        TatooManager.Instance.SetPrefab(button.TrackImage);
    }

    public void ShowTatooGrid()
    {
        TatoosView.SetActive(true);
    }

    public void HideTatooGrid()
    {
        TatoosView.SetActive(false);
    }

    public void ShowPrefabsGrid()
    {
        PrefabView.SetActive(true);
    }

    public void HidePrefabsGrid()
    {
        PrefabView.SetActive(false);
    }
}
