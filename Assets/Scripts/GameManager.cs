using System;
using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    #region Fields

    public static GameManager S;
    
    public AbstractMap AbstractMap;

    private bool _levelGenerated;

    #endregion
    
    private void Awake()
    {
        S = this;
        Assert.IsNotNull(AbstractMap);
    }

    // Start is called before the first frame update
    void Start()
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
    }

    #region Events

    public Action OnLevelGenerated;

    #endregion
}