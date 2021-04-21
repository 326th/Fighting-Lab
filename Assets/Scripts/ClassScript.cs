using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassScript : MonoBehaviour
{
    public class Attack
    {
        private int m_startingFrame;
        private int m_endingFrame;
        private Vector2 m_point;
        private Vector2 m_size;
        private float m_angle;
        private float m_damage;
        private int m_hitStunt;
        private LayerMask m_mask;

        public Attack(int startingFrame, int endingFrame, Vector2 point, Vector2 size, float angle, float damage, int hitStunt, string layerMask)
        {
            m_startingFrame = startingFrame;
            m_endingFrame = endingFrame;
            m_point = point;
            m_size = size;
            m_angle = angle;
            m_damage = damage;
            m_hitStunt = hitStunt;
            m_mask = LayerMask.GetMask(layerMask);
        }
        public bool IsActive(int currentFrame)
        {
            return m_startingFrame <= currentFrame && currentFrame <= m_endingFrame;
        }
        public void DealDamage(Rigidbody2D rb, Character_Base thisCharacterBase)
        {
            Collider2D[] DetectedHitboxes = Physics2D.OverlapBoxAll(point: m_point + rb.position, size: m_size, angle: m_angle, layerMask: m_mask);
            foreach (Collider2D detectedHitBox in DetectedHitboxes)
            {
                Character_Base detectedCharacter = detectedHitBox.GetComponentInParent<Character_Base>();
                if (detectedCharacter != thisCharacterBase)
                {
                    detectedCharacter.TakeDamage(m_damage, m_hitStunt);
                }
            }
        }
    }
    public class Movement
    {
        private int m_startingFrame;
        private Vector2 m_force;
        public Movement(int startingFrame, Vector2 force)
        {
            this.m_startingFrame = startingFrame;
            this.m_force = force;
        }
        public bool IsActive(int currentFrame)
        {
            return m_startingFrame == currentFrame;
        }
        public void Move(Rigidbody2D rb)
        {
            rb.velocity = m_force;
        }
    }
    public class InputReading
    {

    }
    public class Action
    {
        private List<Attack> m_attacks;
        private List<Movement> m_movements;
        private int m_lastFrame;
        public Action(List<Attack> attacks, List<Movement> movements, int lastFrame)
        {
            m_attacks = attacks;
            m_movements = movements;
            m_lastFrame = lastFrame;
        }
        public bool NextStep(int currentFrame, Rigidbody2D rb, Character_Base thisCharacterBase)
        {
            foreach (Attack attack in m_attacks)
            {
                if (attack.IsActive(currentFrame))
                {
                    attack.DealDamage(rb, thisCharacterBase);
                }
            }
            foreach (Movement movement in m_movements)
            {
                if (movement.IsActive(currentFrame))
                {
                    movement.Move(rb);
                }
            }
            return currentFrame < m_lastFrame;
        }
    }
}
