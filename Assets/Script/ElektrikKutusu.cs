
using UnityEngine;

public class ElektrikKutusu : MonoBehaviour
{
    public ParticleSystem patlama;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")||other.gameObject.CompareTag("BigEnemy"))
            patlama.Play();
    }
}
