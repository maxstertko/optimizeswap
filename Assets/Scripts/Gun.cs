using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum GunType { Semi, Burst, Auto};
    public GunType gunType;
    public Transform spawn;
    public float rpm;
    public float damage = 1;

    public LayerMask collisionMask;

    private float secondsBetweenShots;
    private float nextPossibleShootTime;
    public LineRenderer tracer;

    void Start()
    {
        secondsBetweenShots = 60 / rpm;
        /*if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }*/
    }
    public void Shoot()
    {
        Ray ray = new Ray(spawn.position, spawn.forward);
        RaycastHit hit;

        float shotDistance = 20f;

        if(Physics.Raycast(ray, out hit, shotDistance, collisionMask))
        {
            shotDistance = hit.distance;

            if (hit.collider.GetComponent<Entity>())
            {
                hit.collider.GetComponent<Entity>().TakeDamage(damage);
            }
        }

        nextPossibleShootTime = Time.time + secondsBetweenShots;

        if (tracer)
        {
            StartCoroutine("RenderTracer", ray.direction * shotDistance);
        }
        Debug.Log(tracer.GetPosition(1));
    }

    public void ShootContinuous()
    {
        if(gunType == GunType.Auto)
        {
            Shoot();
        }
    }

    private bool CanShoot()
    {
        bool canShoot = true;
        if(Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }
        return canShoot;
    }

    IEnumerator RenderTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, spawn.position);
        tracer.SetPosition(1, spawn.position + hitPoint);
        
        yield return null;
        tracer.enabled = false;
    }
}
