using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldDataCollection : MonoBehaviour
{
    private static string _inputFieldData;

    public static string inputFieldData()
    {
        if (_inputFieldData == null)
        {
            _inputFieldData = "";
        }
        return _inputFieldData;
    }

    private void Update()
    {
        _inputFieldData = gameObject.GetComponent<InputField>().text;
    }
}
