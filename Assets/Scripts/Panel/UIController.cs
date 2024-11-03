using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    public Action AllActiveTweensCompleted;
    public Action FadeToBlackCompleted;
    public Action FadeToWhiteCompleted;

    //single mode panels
    [SerializeField] PanelDriver[] _introPanel = null;
    [SerializeField] PanelDriver[] _heroSelectPanel = null;
    [SerializeField] PanelDriver[] _encounterIntroPanel = null;
    [SerializeField] PanelDriver[] _encounterInspectPanel = null;

    //multi mode panels
    [SerializeField] PanelDriver[] _optionsPanel = null;
    [SerializeField] PanelDriver[] _inspectionPanels = null;
    [SerializeField] PanelDriver[] _inventoryPanels = null;
    [SerializeField] PanelDriver[] _rollDicePanels = null;



    [SerializeField] InspectionPanelDriver _isp = null;
    public InspectionPanelDriver Inspector => _isp;

    [SerializeField] Image _blackoutImage = null;
    [SerializeField] Image _whiteoutImage = null;
    //

    /// <summary>
    /// TRUE anytime a tween commanded by this UI Controller is still active. 
    /// Mechanics should reference this in order to prevent changing modes while UI
    /// elements are still doing things.
    /// </summary>
    public bool IsUIActivelyTweening  = false;
    [SerializeField] private float _timeThatTweensWillBeComplete = 0;

    [SerializeField] float _timeToBlackout = 2f;
    Tween _blackoutTween;
    Tween _whiteoutTween;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var panel in _introPanel) panel?.InitializePanel(this);
        foreach (var panel in _heroSelectPanel) panel?.InitializePanel(this);
        foreach (var panel in _encounterIntroPanel) panel?.InitializePanel(this);
        foreach (var panel in _encounterInspectPanel) panel?.InitializePanel(this);
        foreach (var panel in _optionsPanel) panel?.InitializePanel(this);
        foreach (var panel in _inspectionPanels) panel?.InitializePanel(this);
        foreach (var panel in _inventoryPanels) panel?.InitializePanel(this);
        foreach (var panel in _rollDicePanels) panel?.InitializePanel(this);

        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandlePauseStateChanged(bool isPaused)
    {
        if (isPaused) foreach (var panel in _optionsPanel) panel?.ActivatePanel(false);
        else foreach (var panel in _optionsPanel) panel?.RestPanel(false);
    }

    private void HandleGameModeChanged(GameController.GameModes newGameMode)
    {
        switch (newGameMode)
        {
            case GameController.GameModes.HeroSelect:
                foreach (var panel in _heroSelectPanel) panel?.ActivatePanel(false);

                foreach (var panel in _encounterIntroPanel) panel?.RestPanel(false);
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                foreach (var panel in _encounterInspectPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.WalkingToNextEncounter:

                foreach (var panel in _heroSelectPanel) panel?.RestPanel(false);
                foreach (var panel in _encounterIntroPanel) panel?.RestPanel(false);
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                foreach (var panel in _encounterInspectPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.EncounterIntro:
                foreach (var panel in _encounterIntroPanel) panel?.ActivatePanel(false);

                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _heroSelectPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                foreach (var panel in _encounterInspectPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.EncounterInspection:
                foreach (var panel in _encounterInspectPanel) panel?.ActivatePanel(false);
                ShowRollDicePanels();

                foreach (var panel in _encounterIntroPanel) panel?.RestPanel(false);
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _heroSelectPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                break;

            case GameController.GameModes.EncounterRollingLocking:

                foreach (var panel in _encounterIntroPanel) panel?.RestPanel(false);
                foreach (var panel in _introPanel) panel?.RestPanel(false);
                foreach (var panel in _heroSelectPanel) panel?.RestPanel(false);
                foreach (var panel in _optionsPanel) panel?.RestPanel(false);
                foreach (var panel in _encounterInspectPanel) panel?.RestPanel(false);
                break;
        }
    }


    private void Update()
    {
        if (IsUIActivelyTweening)
        {
            if (Time.time >= _timeThatTweensWillBeComplete)
            {
                IsUIActivelyTweening = false;
                AllActiveTweensCompleted?.Invoke();
            }
        }
    }


    public void SetTweenCompletionTime(float time)
    {
        if (time > _timeThatTweensWillBeComplete && time > Time.time)
        {
            _timeThatTweensWillBeComplete = time;
            IsUIActivelyTweening = true;
        }       
    }

    #region Multi-Mode Panels

    public void ShowInspectionPanels()
    {
        foreach (var panel in _inspectionPanels) panel?.ActivatePanel(false);
    }

    public void HideInspectionPanels()
    {
        foreach (var panel in _inspectionPanels) panel?.RestPanel(false);
    }

    public void ShowInventoryPanels()
    {
        foreach (var panel in _inventoryPanels) panel?.ActivatePanel(false);
    }

    public void HideInventoryPanels()
    {
        foreach (var panel in _inventoryPanels) panel?.RestPanel(false);
    }

    public void ShowRollDicePanels()
    {
        foreach (var panel in _rollDicePanels) panel?.ActivatePanel(false);
    }

    public void HideRollDicePanels()
    {
        foreach (var panel in _rollDicePanels) panel?.RestPanel(false);
    }

    #endregion  

    #region Fades

    public void FadeToBlack()
    {
        _blackoutTween.Kill();
        _blackoutTween = _blackoutImage.DOFade(1, _timeToBlackout).SetUpdate(true).OnComplete(HandleFadeToBlackCompleted);
    }

    private void HandleFadeToBlackCompleted()
    {
        FadeToBlackCompleted?.Invoke();
    }
    public void FadeOutFromBlack()
    {
        _blackoutImage.DOFade(1, .001f);
        _blackoutTween.Kill();        
        _blackoutTween = _blackoutImage.DOFade(0, _timeToBlackout*3).SetUpdate(true).
            SetEase(Ease.InQuint);
    }

    public void FadeToWhite()
    {
        _whiteoutTween.Kill();
        _whiteoutTween = _whiteoutImage.DOFade(1, _timeToBlackout).SetUpdate(true).OnComplete(HandleFadeToWhiteCompleted);
    }

    private void HandleFadeToWhiteCompleted()
    {
        FadeToWhiteCompleted?.Invoke();
    }
    public void FadeOutFromWhite()
    {
        _whiteoutImage.DOFade(1, .001f);
        _whiteoutTween.Kill();
        _whiteoutTween = _whiteoutImage.DOFade(0, _timeToBlackout * 1).SetUpdate(true).
            SetEase(Ease.InQuint);
    }

    #endregion;

}
