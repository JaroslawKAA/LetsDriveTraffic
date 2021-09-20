using System;
using Mapbox.Unity.Map;
using Scripts;
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
    [SerializeField] private InteractionMode _mode = InteractionMode.Default;

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
    }
    
    #region Events

    public Action OnLevelGenerated;

    #endregion
}