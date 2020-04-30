using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    private Animator anim;

    public int winAnimation;
    public int winAnimation2;
    public bool isAttacking;
    public bool isDefending;
    public bool doneDefending;
    public bool isTalking;
    public bool doneTalking;
    public bool isDeath;
    public bool isDeath2;
    public bool hasWon;
    public bool hasWon2;
    public bool isLow;
    public bool isNotLow;
    public bool isDamaged;
    public bool misc;
    public bool isCrying;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        StartCoroutine(Animate());
    }

    public IEnumerator Animate()
    {
        if (isAttacking)
        {
            isAttacking = false;
            anim.SetInteger("WeaponType_int", 12);
            anim.SetInteger("MeleeType_int", 1);

            yield return new WaitForSeconds(0.5f);

            anim.SetInteger("WeaponType_int", 0);
            anim.SetInteger("MeleeType_int", 1);
        }

        else if (isDefending)
        {
            isDefending = false;
            anim.SetBool("Crouch_b", true);
            yield return new WaitForSeconds(0.5f);
        }

        else if (doneDefending)
        {
            doneDefending = false;
            anim.SetBool("Crouch_b", false);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isTalking)
        {
            isTalking = false;
            anim.SetInteger("Animation_int", 2);
            yield return new WaitForSeconds(0.5f);
        }

        else if (doneTalking)
        {
            doneTalking = false;
            anim.SetInteger("Animation_int", 0);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isDeath)
        {
            isDeath = false;
            anim.SetBool("Death_b", true);
            anim.SetInteger("DeathType_int", 2);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isDeath2)
        {
            isDeath = false;
            anim.SetBool("Death_b", true);
            anim.SetInteger("Animation_int", 9);
            yield return new WaitForSeconds(0.5f);
        }

        else if (hasWon)
        {
            hasWon = false;
            anim.SetInteger("Animation_int", winAnimation);
            yield return new WaitForSeconds(0.5f);
        }

        else if (hasWon2)
        {
            hasWon = false;
            anim.SetInteger("Animation_int", winAnimation2);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isDamaged)
        {
            isDamaged = false;
            anim.SetFloat("Head_Vertical_f", 1);
            yield return new WaitForSeconds(0.5f);
            anim.SetFloat("Head_Vertical_f", 0);
        }

        else if (misc)
        {
            misc = false;
            anim.SetInteger("Animation_int", 1);
            yield return new WaitForSeconds(0.5f);
        }

        else if (isCrying)
        {
            isCrying = false;

            anim.SetInteger("Animation_int", 7);
            yield return new WaitForSeconds(0.5f);
            anim.SetInteger("Animation_int", 1);
        }
    }
}
