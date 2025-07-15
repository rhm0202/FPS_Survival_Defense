using System;
using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 핸드형 타입 무기
    [SerializeField]
    private Hand currentHand;

    //공격중??
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitinfo;

    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        currentHand.anim.SetTrigger("Attack");

        yield return new WaitForSeconds(currentHand.attackDelayA);
        isSwing = true;

        // 공격 활성화 시점
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(currentHand.attackDelayB);
        isSwing = false;

        yield return new WaitForSeconds(currentHand.attackDelay - currentHand.attackDelayA - currentHand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckObject())
            {
                isSwing = false;
                Debug.Log(hitinfo.transform.name);
            }
        }
        yield return null;
    }

    private bool CheckObject()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hitinfo, currentHand.range))
        {
            return true;
        }
        return false;
    }
}
