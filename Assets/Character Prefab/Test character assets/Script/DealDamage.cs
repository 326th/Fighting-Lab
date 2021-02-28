using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage: MonoBehaviour
{

    [SerializeField] float Damage = 5f;
    [SerializeField] float Hurt_force = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            Test test = collision.GetComponentInParent<Test>();
            test.Damage(Damage);
        }
    }
}
