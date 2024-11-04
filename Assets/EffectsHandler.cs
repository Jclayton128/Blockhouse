using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class EffectsHandler : MonoBehaviour
{
    //refs
    [SerializeField] TextMeshProUGUI _attackTMP = null;
    [SerializeField] TextMeshProUGUI _defendTMP = null;
    [SerializeField] TextMeshProUGUI _healTMP = null;

    //state
    List<EffectPacket> _effectPacketsToResolve = new List<EffectPacket>();
    int _attack = 0;
    int _defend = 0;
    int _heal = 0;

    private void Start()
    {
        //GameController.Instance.GameModeChanged += HandleGameModeChanged;
        ResetEffectsHandler();
    }

    public void ReceiveEffect(EffectPacket packet)
    {
        //_effectPacketsToResolve.Add(packet);

        switch (packet.Effect)
        {
            case DiceFace.Effects.Attack:
                AddAttack(packet.Magnitude);
                break;

            case DiceFace.Effects.Defend:
                AddDefense(packet.Magnitude);
                break;

            case DiceFace.Effects.Heal:
                AddHeal(packet.Magnitude);
                break;
        }

    }

    private void AddHeal(int magnitude)
    {
        _heal += magnitude;
        if (_heal > 0)
        {
            _healTMP.text = _heal.ToString();
        }
        else
        {
            _healTMP.text = "";
        }
    }

    private void AddDefense(int magnitude)
    {
        _defend += magnitude;
        if (_defend > 0)
        {
            _defendTMP.text = _defend.ToString();
        }
        else
        {
            _defendTMP.text = "";
        }

    }

    private void AddAttack(int magnitude)
    {
        _attack += magnitude;
        if (_attack > 0)
        {
            _attackTMP.text = _attack.ToString();
        }
        else
        {
            _attackTMP.text = "";

        }
    }

    public void ResolveEffects()
    {
        int damage = _attack - _defend;
        damage = Mathf.Clamp(damage, 0, 99);
        Debug.Log($"{transform.root.name} defended at {_defend} against a total attack of {_attack}, resulting in {damage}. Also, healed for {_heal}");

        ResetEffectsHandler();

    } 

    private void ResetEffectsHandler()
    {
        _attack = 0;
        _defend = 0;
        _heal = 0;
        AddAttack(0);
        AddDefense(0);
        AddHeal(0);
    }

}
