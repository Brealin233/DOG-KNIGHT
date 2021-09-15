using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using UnityEngine.Playables;

public class Dart : MonoBehaviour
{
    public enum DartStates { HitPlayer, HitEnemy, HitNothing }
    public ItemDate_SO itemDate;
    private Rigidbody rb;

    PlayableDirector director;

    public DartStates dartStates;
    float removeTime = 60f;
    bool isEnter;

    [Header("Basic Settings")]
    public float force;
    //public GameObject target;
    public int damage;
    private Vector3 direction;

    public GameObject hitReport;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.one * 4;

        director = FindObjectOfType<PlayableDirector>();

        dartStates = DartStates.HitEnemy;
        FlyToTarget();
    }

    void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude < 1f)
        {
            dartStates = DartStates.HitNothing;
        }
        removeTime -= Time.deltaTime;
        if (removeTime <= 0)
            Destroy(gameObject);
    }

    void FlyToTarget()
    {
        //if (target == null)
        //    target = FindObjectOfType<EnemyController>().gameObject;

        //direction = (target.transform.position - transform.position + Vector3.up).normalized;
        //director.Play();
        rb.AddForce(GameManager.Instance.playStats.transform.forward * force, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        switch (dartStates)
        {
            case DartStates.HitEnemy:
                if (other.gameObject.CompareTag("Enemy"))
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = direction * force;
                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                    other.gameObject.GetComponent<CharacterStates>().TakeDamage(damage, other.gameObject.GetComponent<CharacterStates>());

                    GameObject tm = Instantiate(hitReport, (transform.position + new Vector3(0.0f, 1.6f, 0.0f)), Quaternion.identity);
                    tm.gameObject.SetActive(true);
                    Hit tmHit = tm.GetComponent<Hit>();
                    tm.transform.forward = tmHit.cam.forward;
                    tmHit.text = "- " + damage.ToString();

                    dartStates = DartStates.HitNothing;
                }
                break;
            case DartStates.HitNothing:
                //director.stopped+=StopAction;
                break;

        }
    }

    void StopAction(PlayableDirector obj)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && dartStates == DartStates.HitNothing)
        {
            InventoryManager.Instance.inventoryDate.AddItem(itemDate, itemDate.itemAmount);
            InventoryManager.Instance.inventoryUI.RefreshUI();
            Destroy(gameObject);
        }

    }
}
