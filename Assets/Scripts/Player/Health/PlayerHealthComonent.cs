using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct PlayerHealthComonent : IComponentData
{
    public int MaximumHealth;
    public int CurrentHealth;
}
