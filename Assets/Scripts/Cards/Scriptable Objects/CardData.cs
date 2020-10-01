using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    public string Name;
    public int Health;
    public int Damage;
    public int AttackRange;
    public int ActiveRounds;
    public Factory.MercenaryType type;
}
