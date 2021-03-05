using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage: MonoBehaviour
{

    [SerializeField] float DAMAGE = 5f;
    [SerializeField] float HURT_FORCE = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            CharacterLogic characterLogic = collision.GetComponentInParent<CharacterLogic>();
            characterLogic.Damage(DAMAGE,HURT_FORCE);
        }
    }
}
