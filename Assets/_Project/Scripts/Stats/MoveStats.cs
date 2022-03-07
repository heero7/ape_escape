using System;
using UnityEngine;

namespace ApeEscape.Stats
{
    [Serializable]
    public class MoveStats
    {
        [field: SerializeField] public float SneakMovementSpeed { get; private set; } = 1.5f;
        [field: SerializeField] public float RunMovementSpeed { get; private set; } = 3.5f;
        [field: SerializeField] public float CrawlMovementSpeed { get; private set; } = 1.5f;
    }
}