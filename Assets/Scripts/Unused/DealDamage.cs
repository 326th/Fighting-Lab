using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamage: MonoBehaviour
{

    [SerializeField] float DAMAGE = 0f;
    [SerializeField] float HURT_FORCE = 0f;
    [SerializeField] int HIT_STUN = 0;  // frames of hit stunt
    [SerializeField] float FORCE_ANGLE = 0f; //knock back force
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            CharacterLogic characterLogic = collision.GetComponentInParent<CharacterLogic>();
            characterLogic.Damage(DAMAGE,HURT_FORCE,HIT_STUN,FORCE_ANGLE);
        }
    }
}
