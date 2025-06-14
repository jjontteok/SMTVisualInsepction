
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarcodeManager : MonoBehaviour
{
    public TMP_InputField barcodeInputField;
    public TMP_Text pageCountText;
    public TMP_Text barcodeNameText;
    public Image imageDisplay;
    public GameObject errorText;

    string _currentBarcodeName = "";

    List<Texture2D> _pages = new();
    int _currentPage = 0;

    private void Start()
    {
        errorText.SetActive(false);
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return))
        {
            string input = barcodeInputField.text;
            OnSubmitBarcode(input);
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangePage(-1);
        }
        if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangePage(1);
        }
    }

    void OnSubmitBarcode(string input)
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "image");
        _pages.Clear();
        _currentPage = 0;

        for (int i=1; i <= 50; i++)
        {
            string filePath = Path.Combine(folderPath, $"{input}_{i}.jpg");
            if (File.Exists(filePath))
            {
                byte[] fileData = File.ReadAllBytes(filePath);
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
                _pages.Add(tex);
            }
            else
            {
                break;
            }
        }
        if (_pages.Count > 0)
        {
            errorText.SetActive(false);
            ShowImage(0);
        }
        else
        {
            barcodeNameText.text = "";
            pageCountText.text = "";
            _currentBarcodeName = "";
            imageDisplay.sprite = null;
            errorText.SetActive(true);
        }
        barcodeInputField.ActivateInputField();
        barcodeInputField.MoveTextEnd(false);
        barcodeInputField.text = "";
    }
    void ShowImage(int index)
    {
        if (index < 0 || index >= _pages.Count) return;
        _currentPage = index;
       
        if(_currentBarcodeName=="")
            _currentBarcodeName = barcodeInputField.text;
        barcodeNameText.text = _currentBarcodeName;

        Texture2D tex = _pages[index];

        imageDisplay.sprite = Sprite.Create(_pages[index], 
            new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        pageCountText.text = (index == 0) ? "TOP¸é" : "BOT¸é";

    }

    void ChangePage(int offset)
    {
        int newIndex = _currentPage + offset;
        if (newIndex >= 0 && newIndex < _pages.Count)
            ShowImage(newIndex);
    }
}
