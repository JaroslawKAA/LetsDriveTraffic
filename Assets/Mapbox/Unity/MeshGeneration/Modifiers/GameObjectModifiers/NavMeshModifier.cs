using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Data;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEngine;

[CreateAssetMenu(menuName = "Mapbox/Modifier/NavMesh Object Modifier")]
public class NavMeshModifier : GameObjectModifier
{
    public override void Run(VectorEntity ve, UnityTile tile)
    {
        ve.GameObject.AddComponent<NavMeshSourceTag>();
    }
}
