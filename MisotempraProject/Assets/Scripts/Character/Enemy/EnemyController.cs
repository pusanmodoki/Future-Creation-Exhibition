using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private TimeManagement.TimeLayer m_timeLayer = null;

    [SerializeField]
    private Animator m_animator = null;

    [SerializeField]
    private ProcessingLoad.Physics physiscs = null;

    [SerializeField]
    private float m_speed = 5.0f;

    [SerializeField]
    private float m_distance = 2.0f;

    [SerializeField]
    private DurableArmor armor = null;

    [SerializeField]
    private OriginalPhysics.LandingDetect m_landingDetect = null;

    [SerializeField]
    private Damage.DamageController m_damageController = null;

    private bool isMove = true;

    [SerializeField]
    GameObject attackCollision = null;

    public enum State
    {
        AttackStanbai,
        Moving
    }

    State m_state = State.Moving;

    private void Start()
    {
        TimeManagement.TimeLayer.InitLayer(ref m_timeLayer);
        StartCoroutine("CheckAttack");
    }

    private void Update()
    {
        m_animator.speed = m_timeLayer.timeScale;

        if (m_landingDetect.IsLanding(gameObject) && (Player.PlayerController.instance.transform.position - transform.position).magnitude > m_distance)
        {
            isMove = true;
            m_state = State.Moving;
        }

        if (armor.enableKnockBack)
        {
            isMove = false;
        }

        if ((Player.PlayerController.instance.transform.position - transform.position).magnitude <= m_distance)
        {
            isMove = false;
            m_state = State.AttackStanbai;
        }

        Move();


        transform.LookAt(Player.PlayerController.instance.transform);
    }


    private void Move()
    {
        if (!isMove)
        {
            return;
        }

        Vector3 vec = Player.PlayerController.instance.transform.position - transform.position;
        vec.y = 0.0f;
        vec.Normalize();
        vec *= m_speed * Time.deltaTime;
        physiscs.newVelocity = vec;
    }

    private void Attack()
    {
        m_animator.SetTrigger("ToAttack");
        m_damageController.EnableAction("EnemyAttack", 1.0f);
        attackCollision.SetActive(true);

    }

    private IEnumerator CheckAttack()
    {
        while (true)
        {
            if (m_state == State.AttackStanbai)
            {
                if (Random.value < 0.5f * m_timeLayer.timeScale)
                {
                    Attack();
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
}
