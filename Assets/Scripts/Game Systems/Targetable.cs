using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    // Sets the unit to be able to be targetted and can either be a minion target or player target
    public enum EnemyType { Minion, Player }
    public EnemyType enemyType;
}
