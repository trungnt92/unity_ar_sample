using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionButton : MonoBehaviour
{
    [SerializeField]
    private GameObject SelectionMark;
    [SerializeField]
    private RawImage Image;

    public event SelectionButtonHandler OnButtonClicked;
    public TrackImage TrackImage;

    public void Select()
    {
        SelectionMark.SetActive(true);
    }

    public void UnSelect()
    {
        SelectionMark.SetActive(false);
    }

    public void OnClicked()
    {
        OnButtonClicked?.Invoke(this);
    }


    public void SetImage(TrackImage trackImage)
    {
        this.TrackImage = trackImage;
        Image.texture = trackImage.Image;
    }
}

public delegate void SelectionButtonHandler(SelectionButton sender);

