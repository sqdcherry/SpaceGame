using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GrablerController : MonoBehaviour
{
    [SerializeField] private Text textCoins;
    [SerializeField] private int _curentCoins;
    [SerializeField] private GameObject graplerPrefab;
    [SerializeField] private Transform greplerSpawnPos;

    private Animator animator;

    public static Action onGrab;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            var currentGrapler = Instantiate<GameObject>(graplerPrefab, greplerSpawnPos.position, Quaternion.identity);

            var targetRotation = Quaternion.LookRotation(other.transform.position - currentGrapler.transform.position);
            currentGrapler.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.fixedDeltaTime);
            StartCoroutine("Grab", other);
        }
    }

    IEnumerator Grab(Collider coin)
    {
        //var targetRotation = Quaternion.LookRotation(coin.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);
        if (transform.position.y - coin.transform.position.y <= 3)
        {
            yield return new WaitForSeconds(1.6f);
            if (coin)
            {
                animator.SetTrigger("Grab");
                Destroy(coin.gameObject);
                _curentCoins++;
                textCoins.text = _curentCoins.ToString();
            }
        }
    }
}
