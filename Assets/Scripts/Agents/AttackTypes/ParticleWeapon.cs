using UnityEngine;

public class ParticleWeapon : Weapon
{
    public float attackDuration;
    public Transform firePosition;
    public GameObject particleObject;

    private bool attackInProgress;
    private ParticleShoot shoot;
    private AudioSource weaponSound;

    void Start()
    {
        shoot = GetComponent<ParticleShoot>();
        timeTillAttack = 0.0f;
        particleObject.GetComponent<ParticleSystem>().Stop();
        weaponSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeTillAttack -= Time.deltaTime;
    }

    public override void Attack()
    {
        if (timeTillAttack > 0.0f)
            return;
        if (target == null || target.activeInHierarchy == false)
            return;

        timeTillAttack = attackDuration + attackDelay;
        Vector3 shootDir = (target.transform.position + Vector3.up) - firePosition.position;
        shootDir.Normalize();
        //weaponSound.Play();
        shoot.Fire(shootDir, firePosition.position);
        particleObject.GetComponent<ParticleSystem>().Play();
    }

}
