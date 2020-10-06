using UnityEngine;


public class ProjectileWeapon : Weapon
{
    public float projectileVelocity;
    public Transform firePosition;
    public GameObject projectile;

    private bool attackInProgress;
    private AudioSource weaponSound;

    void Start()
    {
        name = name + " " + GetInstanceID().ToString();
        ResetProjectile();
        timeTillAttack = 0.0f;
        float projectileRange = attackRange * 2;
        projectile.GetComponent<Shoot>().InitProjectile(projectileVelocity, projectileRange, ResetProjectile);
        weaponSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeTillAttack -= Time.deltaTime;
    }

    public override void SetDamage(float damage)
    {
        damagePerSecond = damage;
        projectile.GetComponent<Shoot>().SetDamage(damagePerSecond);
    }

    public override void Attack() 
    {
        if (timeTillAttack > 0.0f)
            return;
        if (target == null || target.activeInHierarchy == false)
            return;

        timeTillAttack = attackDelay;
        Vector3 shootDir = (target.transform.position + Vector3.up) - firePosition.position;
        shootDir.Normalize();
        weaponSound.Play();
        projectile.GetComponent<Shoot>().Fire(shootDir, firePosition.position);
        projectile.SetActive(true);
    }

    public void ResetProjectile()
    {
        projectile.transform.position = firePosition.position;
        projectile.SetActive(false);
    }
}
