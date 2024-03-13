using UnityEngine;

public class BosKovan : MonoBehaviour
{
    AudioSource bosKovanSesi;

    void Start()
    {
        bosKovanSesi = GetComponent<AudioSource>();
        Destroy(gameObject, 2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            bosKovanSesi.Play();
            if(!bosKovanSesi.isPlaying)
            {
                Destroy(gameObject, 1f);
            }
        }
    }
}
