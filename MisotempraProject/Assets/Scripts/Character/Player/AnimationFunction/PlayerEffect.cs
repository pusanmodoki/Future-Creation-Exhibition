using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField]
    private Effect.EffectDictionary m_effectDictionary = null;

    [SerializeField]
    private string m_attackTrail = "AttackTrail";

    private void StartAttackTrail()
    {
        m_effectDictionary.PlayEffect(m_attackTrail);
    }
    private void EndAttackTrail()
    {
        m_effectDictionary.StopEffect(m_attackTrail);
    }
}
