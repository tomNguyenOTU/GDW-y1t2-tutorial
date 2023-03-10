using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class NumberDefinition : MonoBehaviour
{
    public string _numericValue = ""; // string to actually hold leading 0's
    private string _oldNumeric = "";
    public bool _enableConversion = false;
    private bool _converted = false;

    public int _numDigits = 1;
    public GameObject _defaultObj;
    public Sprite _noNumberImage;

    public int _spacePadding = 5; // spacing between numbers
    private int _lastPadding = 5;

    public NumberFont _font;

    private void Update()
    {
        if (!_converted && _enableConversion)
        {
            ConvertToSprites();
        }

        LockVariables();

        if (gameObject.transform.childCount < _numDigits)
        {
            CreateDigits();
        }
        else if (gameObject.transform.childCount > _numDigits)
        {
            DeleteOldDigits();
        }

        if (_numericValue != _oldNumeric)
        {
            _converted = false;
        }
        
        if (_spacePadding != _lastPadding)
        {
            UpdateSpacing();
        }

        _oldNumeric = _numericValue;
        _lastPadding = _spacePadding;
    }

    public void CreateDigits() // adds new digits
    {
        _converted = false;

        for (int i = transform.childCount; i < _numDigits; i++) 
        {
            // add digits as children

            GameObject temp = Instantiate(_defaultObj, transform);
            temp.GetComponent<Image>().sprite = _noNumberImage;
        }

        UpdateSpacing();
    }

    public void UpdateSpacing()
    {
        for (int i = 0; i < _numDigits; i++)
        {
            GameObject temp = transform.GetChild(i).gameObject;
            RectTransform pos = temp.GetComponent<RectTransform>();
            pos.localPosition = new Vector3(i * (pos.sizeDelta.x + _spacePadding), 0, pos.localPosition.z);
        }
    }

    public void DeleteOldDigits()
    {
        _converted = false;

        for (int i = transform.childCount - 1; i >= _numDigits; i--)
        {
            // remove from bottom
            DestroyImmediate(transform.GetChild(i).gameObject.GetComponent<Image>());
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    public void ConvertToSprites()
    {
        string splitVal = _numericValue;
        if (splitVal.Length > _numDigits)
        {
            Debug.LogWarning("number is longer than digits availible, updating digits");
            _numDigits = splitVal.Length;
            CreateDigits();
        }
        else if (splitVal.Length < _numDigits)
        {
            Debug.LogWarning("number is shorter than digits availible, updating digits");
            _numDigits = splitVal.Length;
            DeleteOldDigits();
        }

        for (int i = 0; i < _numDigits; i++)
        {
            transform.GetChild(i).GetComponent<Image>().sprite = _font._numberFont[int.Parse(splitVal[i].ToString())];
        }

        _converted = true;
    }

    public void LockVariables()
    {
        if (_enableConversion)
        {
            _numDigits = _numericValue.Length;
        }
    }
}
