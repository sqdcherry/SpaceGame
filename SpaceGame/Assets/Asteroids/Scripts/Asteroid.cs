using System;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private int _damage = 1;
    [SerializeField] private int _health = 1;

    public static Action onCrash;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("SpaceShip"))
        {
            onCrash?.Invoke();
            collider.GetComponent<SpaceShipController>().GetDamage(_damage);
            Destroy(gameObject);
        }
        else if (collider.CompareTag("Bullet"))
        {
            GetDamage(collider);
        }
    }

    private void GetDamage(Collider col)
    {
        _health -= 1;

        if (_health == 0)
        {
            Destroy(gameObject);
        }    
    }
}
