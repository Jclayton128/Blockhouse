using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EncounterController : MonoBehaviour
{
    /// <summary>
    /// Vector3 Position is the midpoint of the encounter
    /// </summary>
    public Action<Vector3> EncounterStarted;
    public Action<Vector3> EncounterFinished;
    public static EncounterController Instance { get; private set; }


    //settings

    [SerializeField] float _timeBetweenChecks = 10f;
    [SerializeField] Vector3 _encounterOffset = new Vector3(13, 0, 0);
    //[SerializeField] float _additionalWalkDistance = 10f;
    [SerializeField] float[] _xOffsets_Encounter = { 2.5f, 6f, 9.5f, 13f };

    //state 
    float _timeOfNextCheck = 0;
    Encounter _currentEncounter;
    int _diceRollsRemainingInEncounter;
    [SerializeField] List<FaceHandler> _fastFaces = new List<FaceHandler>();
    [SerializeField] List<FaceHandler> _mediumFaces = new List<FaceHandler>();
    [SerializeField] List<FaceHandler> _heavyFaces = new List<FaceHandler>();
    [SerializeField] List<FaceHandler> _slowedFaces = new List<FaceHandler>();
    List<ActorVisualsHandler> _actorsToBeUnfaded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.GameModeChanged += HandleGameModeChanged;
    }

    private void HandleGameModeChanged(GameController.GameModes obj)
    {
        if (obj == GameController.GameModes.WalkingToNextEncounter)
        {
            _timeOfNextCheck = Time.time + _timeBetweenChecks;
        }

    }
    public void AdvanceEncounter()
    {
        if (GameController.Instance.GameMode == GameController.GameModes.EncounterIntro)
        {
            GameController.Instance.SetGameMode(GameController.GameModes.EncounterInspection);
        }
        else if (GameController.Instance.GameMode == GameController.GameModes.EncounterInspection)
        {

            GameController.Instance.SetGameMode(GameController.GameModes.EncounterRollingLocking);
        }
    }

    public void AttemptRollDice()
    {
        if (_diceRollsRemainingInEncounter <= 0)
        {
            //unable to roll anymore, should replace Roll Dice panel with 'click to resolve'
            return;
        }

        //ActorController.Instance.ActivateActiveFaceOfPartyDice();
        //ActorController.Instance.ActivateActiveFaceOfEncounterDice();

        Invoke(nameof(Delay_RollDice), 0.5f);
        _diceRollsRemainingInEncounter--;
    }

    private void Delay_RollDice()
    {
        ActorController.Instance.RollPartyDice();
        ActorController.Instance.RollEncounterDice();
    }

    public void StartResolvingDice()
    {
        Debug.Log("Resolving Dice");

        _fastFaces.Clear();
        _mediumFaces.Clear();
        _heavyFaces.Clear();
        _slowedFaces.Clear();

        CollectFaces();

        ResolveNextFaceAnimation();

        //PushEffectsFromParty(_fastFaces_Party);
        //PushEffectsFromEncounter(_fastFaces_Enc);
        ////Resolve effects of fast

        //PushEffectsFromParty(_mediumFaces_Party);
        //PushEffectsFromEncounter(_mediumFaces_Enc);
        ////Resolve effects of medium


        //PushEffectsFromParty(_heavyFaces_Party);
        //PushEffectsFromParty(_heavyFaces_Enc);
        ////Resolve effects of medium

    }

    private void ResolveNextFaceAnimation()
    {

        FaceHandler nextFace;
        if (_fastFaces.Count > 0)
        {
            nextFace = _fastFaces[0];
        }
        else if (_mediumFaces.Count > 0)
        {
            nextFace = _mediumFaces[0];
        }
        else if (_heavyFaces.Count > 0)
        {
            nextFace = _heavyFaces[0]; 
        }
        else if (_slowedFaces.Count > 0)
        {
            nextFace = _slowedFaces[0];
        }
        else
        {
            Debug.Log("complete with resolving");
            //trigger the next game mode, or maybe another round of dice rolling?
            return;
        }

        _actorsToBeUnfaded = DetermineActorsToBeUnfaded(nextFace);
        ActorController.Instance.PartiallyFadeAllActors(true);
        foreach (var actor in _actorsToBeUnfaded)
        {
            actor.SetPartialFade(false);
        }

        ActorVisualsHandler avh = nextFace.transform.root.GetComponentInChildren<ActorVisualsHandler>();
        avh.ExecuteEffectAnimation(nextFace.ActiveDiceFace.Animation);



        Invoke(nameof(ResolveNextFace), 2f);
    }

    private List<ActorVisualsHandler> DetermineActorsToBeUnfaded(FaceHandler nextFace)
    {
        List<ActorVisualsHandler> avhs;
        

        if (nextFace.transform.root.GetComponentInChildren<IFFHandler>().Allegiance == IFFHandler.Allegiances.Player)
        {
           avhs = GatherAVHsInvolvedWithPartyEffect(nextFace);

        }
        else
        {
            avhs = GatherAVHsInvolvedWithEncounterEffect(nextFace);
        }
        avhs.Add(nextFace.transform.root.GetComponentInChildren<ActorVisualsHandler>());
        return avhs;
    }

    private void ResolveNextFace()
    {
        if (_fastFaces.Count > 0)
        {
            FaceHandler nextFace = _fastFaces[0];
            if (nextFace.transform.root.GetComponentInChildren<IFFHandler>().Allegiance == IFFHandler.Allegiances.Player)
            {
                PushEffectsFromParty(nextFace);

            }
            else
            {
                PushEffectsFromEncounter(nextFace);
            }
            _fastFaces.RemoveAt(0);
        }
        else if (_mediumFaces.Count > 0)
        {
            FaceHandler nextFace = _mediumFaces[0];
            if (nextFace.transform.root.GetComponentInChildren<IFFHandler>().Allegiance == IFFHandler.Allegiances.Player)
            {
                PushEffectsFromParty(nextFace);

            }
            else
            {
                PushEffectsFromEncounter(nextFace);
            }

            _mediumFaces.RemoveAt(0);
        }
        else if (_heavyFaces.Count > 0)
        {
            FaceHandler nextFace = _heavyFaces[0];
            if (nextFace.transform.root.GetComponentInChildren<IFFHandler>().Allegiance == IFFHandler.Allegiances.Player)
            {
                PushEffectsFromParty(nextFace);

            }
            else
            {
                PushEffectsFromEncounter(nextFace);
            }

            _heavyFaces.RemoveAt(0);
        }
        else if (_slowedFaces.Count > 0)
        {
            FaceHandler nextFace = _slowedFaces[0];
            if (nextFace.transform.root.GetComponentInChildren<IFFHandler>().Allegiance == IFFHandler.Allegiances.Player)
            {
                PushEffectsFromParty(nextFace);

            }
            else
            {
                PushEffectsFromEncounter(nextFace);
            }
            _slowedFaces.RemoveAt(0);
        }
        else
        {
            Debug.Log("completed resolution - but should't be seeing this.");
            return;
        }
        ActorController.Instance.PartiallyFadeAllActors(false);
        Invoke(nameof(ResolveNextFaceAnimation), 1f);

    }

    private void CollectFaces()
    {
        FaceHandler face;
        foreach (var party in ActorController.Instance.Party)
        {
            face = party.MyDiceHandler.ActiveFace.FaceHandlerInSlot;

            if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Light)
            {
                _fastFaces.Add(face);
            }
            else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Medium)
            {
                _mediumFaces.Add(face);
            }
            else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Heavy)
            {
                _heavyFaces.Add(face);
            }
            else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Slowed)
            {
                _slowedFaces.Add(face);
            }
        }       

        Debug.Log($"In Party, found {_fastFaces.Count} fast faces, {_mediumFaces.Count} medium faces, and {_heavyFaces.Count} heavy faces");

        foreach(var enc in ActorController.Instance.Encounter)
        {
            ActorHandler ah;
            if (enc.TryGetComponent<ActorHandler>(out ah))
            {
                face = ah.MyDiceHandler.ActiveFace.FaceHandlerInSlot;

                if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Light)
                {
                    _fastFaces.Add(face);
                }
                else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Medium)
                {
                    _mediumFaces.Add(face);
                }
                else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Heavy)
                {
                    _heavyFaces.Add(face);
                }
                else if (face.ActiveDiceFace.DiceSpeed == Dice.DiceSpeeds.Slowed)
                {
                    _slowedFaces.Add(face);
                }
            }
        }

        Debug.Log($"In Encounter, found {_fastFaces.Count} fast faces, {_mediumFaces.Count} medium faces, and {_heavyFaces.Count} heavy faces");

    }




    private void PushEffectsFromParty(FaceHandler face)
    {
        EffectsHandler eh;
        EffectPacket ep = new EffectPacket(face.ActiveDiceFace.Effect, face.ActiveDiceFace.Magnitude);
        switch (face.ActiveDiceFace.Range)
        {
            case DiceFace.Ranges.FirstEnemy:
                eh = ActorController.Instance.Encounter[0].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.RandomEnemy:
                int count = ActorController.Instance.Encounter.Count;
                int rand = UnityEngine.Random.Range(0, count);
                eh = ActorController.Instance.Encounter[rand].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.LastEnemy:
                int last = ActorController.Instance.Encounter.Count - 1;
                eh = ActorController.Instance.Encounter[last].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.AllEnemy:
                var list = ActorController.Instance.Encounter;
                foreach (var item in list)
                {
                    item.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                }                    
                break;

            case DiceFace.Ranges.AllParty:
                var list2 = ActorController.Instance.Party;
                foreach (var item in list2)
                {
                    item.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                }
                break;

            case DiceFace.Ranges.Self:
                face.transform.root.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                break;
        }
    }

    private void PushEffectsFromEncounter(FaceHandler face)
    {
        EffectsHandler eh;
        EffectPacket ep = new EffectPacket(face.ActiveDiceFace.Effect, face.ActiveDiceFace.Magnitude);
        switch (face.ActiveDiceFace.Range)
        {
            case DiceFace.Ranges.FirstEnemy:
                eh = ActorController.Instance.Party[0].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.RandomEnemy:
                int count = ActorController.Instance.Party.Count;
                int rand = UnityEngine.Random.Range(0, count);
                eh = ActorController.Instance.Party[rand].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.LastEnemy:
                int last = ActorController.Instance.Party.Count - 1;
                eh = ActorController.Instance.Party[last].GetComponent<EffectsHandler>();
                eh.ReceiveEffect(ep);
                break;

            case DiceFace.Ranges.AllEnemy:
                var list = ActorController.Instance.Party;
                foreach (var item in list)
                {
                    item.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                }
                break;

            case DiceFace.Ranges.AllParty:
                var list2 = ActorController.Instance.Encounter;
                foreach (var item in list2)
                {
                    item.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                }
                break;

            case DiceFace.Ranges.Self:
                face.transform.root.GetComponent<EffectsHandler>().ReceiveEffect(ep);
                break;



        }
        
    }

    private List<ActorVisualsHandler> GatherAVHsInvolvedWithPartyEffect(FaceHandler face)
    {
        List<ActorVisualsHandler> avhs = new List<ActorVisualsHandler>();

        switch (face.ActiveDiceFace.Range)
        {        

            case DiceFace.Ranges.FirstEnemy:
                var avh0 = ActorController.Instance.Encounter[0].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh0);
                break;

            case DiceFace.Ranges.RandomEnemy:
                int count = ActorController.Instance.Encounter.Count;
                int rand = UnityEngine.Random.Range(0, count);

                var avh1 = ActorController.Instance.Encounter[rand].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh1);
                break;

            case DiceFace.Ranges.LastEnemy:
                int last = ActorController.Instance.Encounter.Count - 1;
                var avh2 = ActorController.Instance.Encounter[last].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh2);
                break;

            case DiceFace.Ranges.AllEnemy:
                var list = ActorController.Instance.Encounter;
                foreach (var item in list)
                {
                    var avh3 = item.GetComponentInChildren<ActorVisualsHandler>();
                    avhs.Add(avh3);
                }
                break;

            case DiceFace.Ranges.AllParty:
                var list2 = ActorController.Instance.Party;
                foreach (var item in list2)
                {
                    var avh4 = item.GetComponentInChildren<ActorVisualsHandler>();
                    avhs.Add(avh4);
                }
                break;

            case DiceFace.Ranges.Self:
                var avh5 = face.transform.root.GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh5);
                break;
        }
        return avhs;
    }

    private List<ActorVisualsHandler> GatherAVHsInvolvedWithEncounterEffect(FaceHandler face)
    {
        List<ActorVisualsHandler> avhs = new List<ActorVisualsHandler>();

        switch (face.ActiveDiceFace.Range)
        {

            case DiceFace.Ranges.FirstEnemy:
                var avh0 = ActorController.Instance.Party[0].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh0);
                break;

            case DiceFace.Ranges.RandomEnemy:
                int count = ActorController.Instance.Party.Count;
                int rand = UnityEngine.Random.Range(0, count);

                var avh1 = ActorController.Instance.Party[rand].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh1);
                break;

            case DiceFace.Ranges.LastEnemy:
                int last = ActorController.Instance.Party.Count - 1;
                var avh2 = ActorController.Instance.Party[last].GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh2);
                break;

            case DiceFace.Ranges.AllEnemy:
                var list = ActorController.Instance.Party;
                foreach (var item in list)
                {
                    var avh3 = item.GetComponentInChildren<ActorVisualsHandler>();
                    avhs.Add(avh3);
                }
                break;

            case DiceFace.Ranges.AllParty:
                var list2 = ActorController.Instance.Encounter;
                foreach (var item in list2)
                {
                    var avh4 = item.GetComponentInChildren<ActorVisualsHandler>();
                    avhs.Add(avh4);
                }
                break;

            case DiceFace.Ranges.Self:
                var avh5 = face.transform.root.GetComponentInChildren<ActorVisualsHandler>();
                avhs.Add(avh5);
                break;
        }
        return avhs;
    }

    #region Flow

    private void Update()
    {
        UpdateCheckForNextEncounter();
    }

   
    
    private void UpdateCheckForNextEncounter()
    {
        if (GameController.Instance.GameMode != GameController.GameModes.WalkingToNextEncounter) return;

        if (!_currentEncounter && Time.time >= _timeOfNextCheck)
        {
            _timeOfNextCheck = Time.time + _timeBetweenChecks;

            Encounter enc = EncounterLibrary.Instance.FindValidRandomEncounter(RunController.Instance.DistanceTraveled);
            if (enc)
            {
                _currentEncounter = enc;
                StartEncounter();

                //AUDIO somekind of bell announcing a new encounter
                RunController.Instance.SetTargetCountdown(_encounterOffset.x);
                RunController.Instance.TargetDistanceReached += HandleTargetDistanceReached;
            }
        }
    }

    private void HandleTargetDistanceReached()
    {
        GameController.Instance.SetGameMode(GameController.GameModes.EncounterIntro);
    }

    private void StartEncounter()
    {
        _diceRollsRemainingInEncounter = RunController.Instance.RollsPerEncounter;
        SpawnEncounterActors();
        //EncounterStarted?.Invoke(_encounterOffset);
        EncounterStarted?.Invoke(new Vector3(2.5f,0,0));
    }

    private void SpawnEncounterActors()
    {
        int count = 0;
        if (_currentEncounter.Actors.Length > 4)
        {
            Debug.LogWarning("More actors requested for this encounter than can fit");
        }
        foreach (var actor in _currentEncounter.Actors)
        {

            ActorHandler ah = ActorController.Instance.SpawnActor(actor,
                _encounterOffset + new Vector3(_xOffsets_Encounter[count], 0, 0),
                IFFHandler.Allegiances.Enemy);

            //ah.Initialize(IFFHandler.Allegiances.Enemy);

            count++;
        }
    }

    #endregion

}
