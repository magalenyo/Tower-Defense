using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float attackSpeed = 1.0f;
    [SerializeField] private GameObject bulletPrefab;
    

    private Queue<GameObject> enemies = new Queue<GameObject>();
    private GameObject sentry;
    private Transform sentryShotPoint;
    private Projectile projectile;


    private float attackTimer = 0.0f;

    void Start()
    {
        sentry = transform.GetChild(1)?.gameObject;
        if (sentry)
        {
            sentryShotPoint = sentry.transform.GetChild(0).gameObject.transform;
        }

        attackTimer = attackSpeed;

        if (bulletPrefab)
        {
            projectile = bulletPrefab.GetComponent<Projectile>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        OrientateGun();
        Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemies.Enqueue(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemies.Dequeue();
    }

    private void OrientateGun()
    {
        if (enemies.Count > 0 && sentry)
        {
            GameObject enemyGO = enemies.Peek();
            Enemy enemy = enemyGO.GetComponent<Enemy>();
            Vector3 enemyPosition = enemyGO.transform.position;
            enemyPosition.y = 0;
            float timeToReach = (enemyPosition - transform.position).magnitude / projectile.GetProjectileSpeed();
            Vector3 positionToLookAt = enemyPosition + (enemyGO.transform.forward * enemy.GetMovementSpeed() * timeToReach);

            sentry.transform.LookAt(positionToLookAt);
        }
    }

    private void Shoot()
    {
        if (attackTimer >= attackSpeed)
        {
            if (enemies.Count > 0)
            {
                attackTimer = 0.0f;
                //shoot
                if (bulletPrefab)
                {
                    GameObject bullet = Instantiate(bulletPrefab, sentryShotPoint.position, sentryShotPoint.rotation);
                    Projectile projectile = bullet.GetComponent<Projectile>();
                }
            }
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }
}
