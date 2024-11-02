using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDriver_Old : MonoBehaviour
{
    //refs
    [SerializeField] Image _cardImage = null;
    [SerializeField] Image _energyCostImage = null;

    //settings
    [SerializeField] Sprite[] _energyBarLevels = null;

    //state
    //Card
    int _cost = 0;

    private void Start()
    {
        RenderCost();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _cost++;
            _cost = Mathf.Clamp(_cost, 0, _energyBarLevels.Length);
            RenderCost();
        }        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _cost--;
            _cost = Mathf.Clamp(_cost, 0, _energyBarLevels.Length);
            RenderCost();
        }
    }
    private void RenderCost()
    {
        _energyCostImage.sprite = _energyBarLevels[_cost];
    }


}
