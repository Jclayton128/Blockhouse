using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDataHandler : MonoBehaviour
{
    [SerializeField] Sprite _icon = null;
    [SerializeField] string _name = null;
    
    public Sprite Icon => _icon;
    public string Name => _name;
}
