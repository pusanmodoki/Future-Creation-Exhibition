using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator = null;

    [SerializeField]
    Damage.DamageController controller = null;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void AttackEnd()
    {
        animator.SetTrigger("ToStandby");
        controller.DisableAction("EnemyAttack");
    }
}
