using UnityEngine;

[CreateAssetMenu]
public class WaveData : ScriptableObject
{
    public int health;
    public int amount;
    public float interval;
    public float attackRange;
    public float damage;
    public Factory.MinionType type;
}
