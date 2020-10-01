using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damagePerSecond;
    public float attackDelay;
    public float attackRange;

    protected GameObject target;
    protected float timeTillAttack;

    public virtual void SetDamage(float damage)
    {

    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public virtual void Attack()
    {
    }
}
