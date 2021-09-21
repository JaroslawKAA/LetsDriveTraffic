using System;
using System.Collections.Generic;
using System.Linq;
using Mapbox.Unity.Map;
using Scripts;
using UI;
using UnityEngine;
using UnityEngine.Assertions;

public enum InteractionMode
{
    Default,
    PlacingSign,
    DraggingSign
}

public class GameManager : Singleton<GameManager>
{
    #region Fields

    public AbstractMap AbstractMap;

    public InteractionMode Mode
    {
        get => _mode;
        private set => _mode = value;
    }

    private bool _levelGenerated;

    [Header("Defined dynamically")] [SerializeField]
    private InteractionMode _mode = InteractionMode.Default;

    [SerializeField] private float _time;
    [SerializeField] private string _timeFormated;
    [SerializeField] private int _accidentsCount;
    [SerializeField] private List<float> _accidentsDate;

    #endregion

    protected override void OnAwake()
    {
        Assert.IsNotNull(AbstractMap);
    }

    // Start is called before the first frame update
    private void Start()
    {
        AbstractMap.MapVisualizer.OnMapVisualizerStateChanged += s =>
        {
            if (s == ModuleState.Finished)
            {
                foreach (Transform child in AbstractMap.transform)
                {
                    child.gameObject.isStatic = true;
                    child.gameObject.AddComponent<NavMeshSourceTag>();
                }

                if (!_levelGenerated)
                {
                    OnLevelGenerated?.Invoke();
                    _levelGenerated = true;
                }
            }
        };

        GameEvents.S.OnPlacingSignStart += () => Mode = InteractionMode.PlacingSign;
        GameEvents.S.OnPlacingSignEnd += () => Mode = InteractionMode.Default;
        GameEvents.S.OnDraggingSignStart += () => Mode = InteractionMode.DraggingSign;
        GameEvents.S.OnDraggingSignEnd += () => Mode = InteractionMode.Default;
        GameEvents.S.OnRoadAccident += () =>
        {
            _accidentsCount++;
            _accidentsDate.Add(_time);
            UIManager.S.SetAccidents(_accidentsCount.ToString());

            int lastMinuteAccidents = _accidentsDate.Count(x => x >= _time - 60);
            UIManager.S.SetAccidentsInLastMin(lastMinuteAccidents.ToString());
        };
    }

    private void Update()
    {
        _time += Time.deltaTime;
        _timeFormated = $"{(int) (_time / 60)}:{(int) (_time % 60)}";
        UIManager.S.SetTime(_timeFormated);
    }

    #region Events

    public Action OnLevelGenerated;

    #endregion
}