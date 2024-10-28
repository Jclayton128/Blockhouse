using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InspectionPanelDriver : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nameTMP = null;

    public void DisplayFaceInformation(DiceFace diceFace)
    {
        _nameTMP.text = diceFace.FaceName;
    }

    public void ClearFaceInformation()
    {
        _nameTMP.text = " ";
    }
}
