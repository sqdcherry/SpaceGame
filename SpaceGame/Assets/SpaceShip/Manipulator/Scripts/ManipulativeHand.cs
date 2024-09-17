using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ManipulativeHand : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private Text textCoins;
    [SerializeField] private int _curentCoins;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            var targetRotation = Quaternion.LookRotation(other.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.fixedDeltaTime);
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
                _curentCoins++;
                textCoins.text = _curentCoins.ToString();
                Destroy(coin.gameObject);
            }
        }
    }
}
