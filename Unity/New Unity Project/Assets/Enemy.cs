using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
   // public GameObject AIP;
    public int maxHealth = 20;
    int currentHealth;
    



        void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamege(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <=0)
        {
            Stunned();
            
        }
       
    }

    void Stunned()
    {
        Debug.Log("Ennemy stunned");
        // animator.SetBool("IsStunned",true);
      //  UnStunned();
      //  AIP.GetComponent<AIPatrol>().mustPatrol = false;
    }

    // IEnumerator UnStunned(){
    // yield return new WaitForSeconds (1f);
   // GetComponent<AIPatrol>().enabled(true);
    // animator.SetBool("IsStunned",false);
   //  AIP.enabled = true;
   // AIP.GetComponent<AIPatrol>().mustPatrol = true;
}











