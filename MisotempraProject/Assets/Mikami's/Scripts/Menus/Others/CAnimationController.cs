using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CAnimationController : MonoBehaviour
{
    private Animator m_animator;
    // Start is called before the first frame update
    public virtual void Start()
    {
        SecureAnimator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// アニメーターのparameter等を変更する( bool型 )
    /// </summary>
    public void ChangeBoolAnimation(bool state, string parameterName)
    {
        m_animator.SetBool(parameterName, state);
    }

    public void ChangeBoolAnimation(Animator animator,bool state, string parameterName)
    {
        animator.SetBool(parameterName, state);
    }

    /// <summary>
    /// parameterの値をもらう
    /// </summary>
    /// <return> パラメーターの値 </return>>
    public bool GetBoolParameter (string parameterName)
    {
        return m_animator.GetBool(parameterName);
    }

    public bool GetBoolParameter(Animator animator,string parameterName)
    {
        return animator.GetBool(parameterName);
    }

    /// <summary>
    /// 指定のアニメーションが終わったか、 
    /// </summary>
    /// <return>( bool型 )</return>
    public bool JudgeAnimation(string chipName)
    {
        if (m_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(chipName))
            if (m_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
        return false;
    }
    public bool JudgeAnimation(Animator animator,string chipName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash == Animator.StringToHash(chipName))
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                return true;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void SecureAnimator()
    {
        if (m_animator == null)
            m_animator = this.gameObject.GetComponent<Animator>();

        // timeScale=0でもアニメーションをさせるため
                m_animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        //        m_animator.updateMode = AnimatorUpdateMode.AnimatePhysics;
        //        m_animator.updateMode = AnimatorUpdateMode.Normal;
    }
}
