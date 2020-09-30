using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum Anim { Idle, Walk, Run, Jump }

public class ShakeMagnitude : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI tmp;
	[SerializeField] private float minWalkValue = 1.5f;
	[SerializeField] private float walkValue = 2.5f;
	[SerializeField] private float runValue = 3.5f;

	private Vector3 accel;
	private float maxValue = 0;

	private bool IdleMagnitude = false;
	private bool walkMagnitude = false;
	private bool runMagnitude = false;
	private bool jumpMagnitude = false;

	[SerializeField] private bool m_canIdle	= true;
	[SerializeField] private bool m_canWalk	= true;
	[SerializeField] private bool m_canRun	= true;
	[SerializeField] private bool m_canJump = true;

	[SerializeField] private Anim m_curAnim;
	private Coroutine m_animation_Co;

	[SerializeField] private UnityEvent OnJump;

	private void Start()
    {
		m_canIdle = true;
		m_canWalk = true;
		m_canRun = true;
		m_canJump = true;
	}

    void Update()
	{
		accel = Input.acceleration;

		accel.z = 0;

		float magnitude = (float)Math.Round(accel.sqrMagnitude, 2);

		if (magnitude > maxValue) maxValue = magnitude;


		IdleMagnitude	=  magnitude < minWalkValue;
		walkMagnitude	= (magnitude > minWalkValue && magnitude < walkValue);
		runMagnitude	= (magnitude < runValue		&& magnitude > walkValue);
		jumpMagnitude	=  magnitude > runValue;

        if (IdleMagnitude && m_canWalk && m_canRun)
        {
			m_curAnim = Anim.Idle;
			//SetAnimation(Anim.Idle);
        }
        
		if ( walkMagnitude && m_canWalk && m_canRun)
        {
			m_canWalk = false;
            SetAnimation(Anim.Walk);
        }
        
		if (runMagnitude && m_canRun && !m_canWalk)
        {
			m_canWalk = false;
			m_canRun = false;
			SetAnimation(Anim.Run);
        }

        if (jumpMagnitude && m_canJump)
		{
			m_canJump = false;
			StartCoroutine(SetAnimation_Co(Anim.Jump));
		}
        
	}

	private void SetAnimation(Anim anim) 
	{
        if (m_animation_Co != null) StopCoroutine(m_animation_Co);

        m_animation_Co = StartCoroutine(SetAnimation_Co(anim));

    }

	private IEnumerator SetAnimation_Co(Anim anim) 
	{
		m_curAnim = anim;

		switch (anim)
		{
            case Anim.Idle:

                yield return new WaitForEndOfFrame();
                break;
            case Anim.Walk:

				yield return new WaitForSeconds(3);
				m_canWalk = true;
				break;
			case Anim.Run:

				yield return new WaitForSeconds(3);
				m_canWalk = true;
				m_canRun = true;
				break;
			case Anim.Jump:
				OnJump?.Invoke();
				yield return new WaitForSeconds(3);
				m_canJump = true;
				break;

		}


	}

}
