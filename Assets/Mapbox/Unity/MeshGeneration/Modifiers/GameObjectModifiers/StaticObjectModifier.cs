using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEngine;

[CreateAssetMenu(menuName = "Mapbox/Modifier/Static Object Modifier")]
public class StaticObjectModifier : GameObjectModifier
{
    [SerializeField] private bool _isStatic = true;

    public override void Run(VectorEntity ve, UnityTile title)
    {
        ve.GameObject.isStatic = true;
    }
}
