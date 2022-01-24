using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassScript : MonoBehaviour
{
    [System.Serializable]
    public class BufferHandle
    {
        protected Dictionary<string, int> recordedInput = new Dictionary<string, int>();
        public virtual Action GetNextAction()
        {
            return null;
        }
        public virtual void HandleInputs(Dictionary<string, int> thisFrameInput)
        {

        }
    }
    [System.Serializable]
    public class JumpBufferHandle: BufferHandle
    {
        [SerializeField] private float m_jumpVelX;
        [SerializeField] private float m_jumpVelY;
        [SerializeField] private Action jumpUp;
        [SerializeField] private Action jumpRight;
        [SerializeField] private Action jumpLeft;
        public JumpBufferHandle(float jumpVelX = 5, float jumpVelY = 15)
        {
            m_jumpVelX = jumpVelX;
            m_jumpVelY = jumpVelY;
            Vector2 jumpForceY = Vector2.up * m_jumpVelY;
            Vector2 JumpForceX = Vector2.right * m_jumpVelX;
            List<Movement> movement_list = new List<Movement>();
            movement_list.Add(new Movement(1, jumpForceY));
            jumpUp = new Action(new List<Attack>(), movement_list, new InputBuffer(-1, -1, new BufferHandle()), 1);
            movement_list = new List<Movement>();
            movement_list.Add(new Movement(1, jumpForceY + JumpForceX));
            jumpRight = new Action(new List<Attack>(), movement_list, new InputBuffer(-1, -1, new BufferHandle()), 1);
            movement_list = new List<Movement>();
            movement_list.Add(new Movement(1, jumpForceY - JumpForceX));
            jumpLeft = new Action(new List<Attack>(), movement_list, new InputBuffer(-1, -1, new BufferHandle()), 1);
        }
        public override Action GetNextAction()
        {
            if (recordedInput["Left"] != 0 && recordedInput["Right"] == 0)
            {
                return jumpLeft;
            }else if (recordedInput["Left"] == 0 && recordedInput["Right"] != 0)
            {
                return jumpRight;
            }
            return jumpUp;
        }
        public override void HandleInputs(Dictionary<string, int> inputsThisFrame)
        {
            recordedInput["Left"] = inputsThisFrame["Left"];
            recordedInput["Right"] = inputsThisFrame["Right"];
        }
        public Dictionary<string,Action> GetActionDict()
        {
            Dictionary<string, Action> actionDict = new Dictionary<string, Action>();
            actionDict.Add("Jump_Up", jumpUp);
            actionDict.Add("Jump_Right", jumpRight);
            actionDict.Add("Jump_Left", jumpLeft);
            return actionDict;
        }
    }
    [System.Serializable]
    public class Attack
    {
        private int m_startingFrame;
        private int m_endingFrame;
        private Vector2 m_point;
        private Vector2 m_pointReverse;
        private Vector2 m_size;
        private float m_angle;
        private float m_damage;
        private int m_hitStunt;
        private LayerMask m_mask;
        private int m_special;

        public Attack(int startingFrame, int endingFrame, Vector2 point, Vector2 size, float angle, float damage, int hitStunt, string layerMask, int special)
        {
            m_startingFrame = startingFrame;
            m_endingFrame = endingFrame;
            m_point = point;
            m_pointReverse = new Vector2(point.x * -1, point.y);
            m_size = size;
            m_angle = angle;
            m_damage = damage;
            m_hitStunt = hitStunt;
            m_mask = LayerMask.GetMask(layerMask);
            m_special = special;
        }
        public bool IsActive(int currentFrame)
        {
            return m_startingFrame <= currentFrame && currentFrame <= m_endingFrame;
        }
        public void DealDamage(Rigidbody2D rb, Character_Base thisCharacterBase, bool facingRight)
        {
            Collider2D[] DetectedHitboxes;
            if (facingRight)
            {
                DetectedHitboxes = Physics2D.OverlapBoxAll(point: m_point + rb.position, size: m_size, angle: m_angle, layerMask: m_mask);
            }
            else
            {
                DetectedHitboxes = Physics2D.OverlapBoxAll(point: m_pointReverse + rb.position, size: m_size, angle: m_angle, layerMask: m_mask);
            }
            foreach (Collider2D detectedHitBox in DetectedHitboxes)
            {
                Character_Base detectedCharacter = detectedHitBox.GetComponentInParent<Character_Base>();
                if (detectedCharacter != thisCharacterBase)
                {
                    m_special = 0;
                    detectedCharacter.TakeDamage(m_damage, m_hitStunt, m_special);
                }
            }
        }
    }
    [System.Serializable]
    public class Movement
    {
        private int m_activeFrame;
        private Vector2 m_force;
        public Movement(int activeFrame, Vector2 force)
        {
            m_activeFrame = activeFrame;
            m_force = force;
        }
        public bool IsActive(int currentFrame)
        {
            return m_activeFrame == currentFrame;
        }
        public void Move(Rigidbody2D rb)
        {
            rb.velocity = m_force;
        }
    }
    [System.Serializable]
    public class InputBuffer
    {
        private int m_firstFrame;
        private int m_lastFrame;
        private BufferHandle m_buffer;
        public InputBuffer(int firstFrame, int lastFrame, BufferHandle buffer)
        {
            m_firstFrame = firstFrame;
            m_lastFrame = lastFrame;
            m_buffer = buffer;
        }
        public bool IsActive(int currentFrame)
        {
            return m_firstFrame <= currentFrame && currentFrame <= m_lastFrame;
        }
        public Action BufferCaluculate(int currentFrame, Dictionary<string, int> inputsThisFrame)
        {
            m_buffer.HandleInputs(inputsThisFrame);
            if (currentFrame == m_lastFrame)
            {
                return m_buffer.GetNextAction();
            }
            return null;
        }
    }
    [System.Serializable]
    public class Action
    {
        private List<Attack> m_attacks;
        private List<Movement> m_movements;
        [SerializeField] private InputBuffer m_buffer;
        [SerializeField] private int m_lastFrame;
        [SerializeField] private Action m_nextAction = null;
        /// <param name="lastFrame"> how long will this action last (formula : sencond = (2 * lastFrame + 3) /60 )</param>
        public Action(List<Attack> attacks, List<Movement> movements, InputBuffer buffer, int lastFrame)
        {
            m_attacks = attacks;
            m_movements = movements;
            m_lastFrame = lastFrame;
            m_buffer = buffer;
        }
        public bool NextStep(int currentFrame, Rigidbody2D rb, Character_Base thisCharacterBase,bool facingRight, Dictionary<string, int> inputsThisFrame)
        {
            foreach (Attack attack in m_attacks)
            {
                if (attack.IsActive(currentFrame))
                {
                    attack.DealDamage(rb, thisCharacterBase,facingRight);
                }
            }
            foreach (Movement movement in m_movements)
            {
                if (movement.IsActive(currentFrame))
                {
                    movement.Move(rb);
                }
            }
            if (m_buffer.IsActive(currentFrame))
            {
                m_nextAction = m_buffer.BufferCaluculate(currentFrame,inputsThisFrame);
            }
            return currentFrame < m_lastFrame;
        }
        public Action GetNextAction()
        {
            return m_nextAction;
        }
    }
}
