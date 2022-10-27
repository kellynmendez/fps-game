using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnwalkableColliders : MonoBehaviour
{
    public Collider[] unwalkableColliders;

    public Collider[] GetUnwalkableColliders()
    {
        return unwalkableColliders;
    }
}
