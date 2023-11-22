using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetsManager : Singleton<PlayerTargetsManager>
{
    [SerializeField] private Transform[] targets;


    public Transform[] GetAllTargets()
    {
        return targets;
    }
}
