using System;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    private float velocity;
    private Vector3 direction;
    private bool isShot;
    private float damage;
    private float range;
    private float travelledDistance;

    private Vector3 firePosition;
    private Action finishedCallback;

    void LateUpdate()
    {
        if (isShot == false)
            return;

        Vector3 distance = Time.deltaTime * velocity * direction;
        travelledDistance += distance.magnitude;
        transform.position = firePosition + direction * travelledDistance;

        if (travelledDistance > range)
        {
            finishedCallback();
            isShot = false;
        }
    }

    public void InitProjectile(float velocity, float range, Action callback)
    {
        this.velocity = velocity;
        this.range = range;
        finishedCallback = callback;
    }

    public void Fire(Vector3 direction, Vector3 firePosition)
    {
        if (isShot)
            finishedCallback();

        this.firePosition = firePosition;

        this.direction = direction;
        travelledDistance = 0.0f;
        isShot = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag != Constants.MinionTag && other.tag != Constants.MercenaryTag)
            return;

        AgentController ac = other.GetComponent<AgentController>();
        HealthController hc = ac.GetHealthController();
        hc.ReduceHealth(damage);

        Explode();
        isShot = false;
        finishedCallback();
    }

    private void Explode()
    {
        //Todo
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
}
