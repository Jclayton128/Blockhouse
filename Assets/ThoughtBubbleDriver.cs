using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtBubbleDriver : MonoBehaviour
{
    [SerializeField] SpriteRenderer _thoughtBubbleIcon = null;
    [SerializeField] Sprite _searchIcon = null;
    [SerializeField] Sprite _attackIcon = null;

    private void Start()
    {
        var ab = GetComponent<ActorBrain>();
        ab.EnemyDetected += SetAttackIcon;
        ab.EnemyNotDetected += SetSearchIcon;
    }

    public void SetSearchIcon()
    {
        _thoughtBubbleIcon.sprite = _searchIcon;
    }

    public void SetAttackIcon()
    {
        _thoughtBubbleIcon.sprite = _attackIcon;
    }


}
