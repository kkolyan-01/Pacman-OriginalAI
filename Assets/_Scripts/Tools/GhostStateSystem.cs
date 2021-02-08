using System;
using UnityEngine;

[Serializable]
public class StateTime
{
    public GhostState state;
    public float timeStart;
}

[CreateAssetMenu(fileName = "GhostStateSystem", menuName = "Pacman Systems/Ghost State System")]
public class GhostStateSystem : ScriptableObject
{
     public StateTime[] _stateTimes;
}
