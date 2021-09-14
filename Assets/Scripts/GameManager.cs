using Mapbox.Unity.Map;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    
    public AbstractMap AbstractMap;

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
            }
        };
    }
}