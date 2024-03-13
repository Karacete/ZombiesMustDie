
using UnityEngine;
using UnityEngine.AI;

public class Dusman : MonoBehaviour
{
    GameObject anaKontrol;
    GameObject hedef;
    NavMeshAgent dusman;
    float can;
    float canAzaltma;
    Animator olme;
    CharacterController karakter;
    AudioSource zombi;

    void Start()
    {
        anaKontrol = GameObject.FindWithTag("Ana");
        dusman = GetComponent<NavMeshAgent>();
        can = 100;
        olme = GetComponent<Animator>();
        karakter = GetComponent<CharacterController>();
        zombi= GetComponent<AudioSource>();
        zombi.Play();
    }

    // Update is called once per frame
    void Update()
    {
        dusman.SetDestination(hedef.transform.position);
    }
    public void HedefBelirle(GameObject obje)
    {
        hedef = obje;
    }
    public void CanAzalt(float vurus)
    {
        can -= vurus;
        if (can <= 0)
        {
            DusmanYokEtme();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hedef"))
        {
            if (gameObject.CompareTag("BigEnemy"))
                canAzaltma = .5f;
            else
                canAzaltma = .1f;
            anaKontrol.GetComponent<GenelAyarlar>().HealthBar(canAzaltma);
            DusmanYokEtme();
        }
    }
    void DusmanYokEtme()
    {
        olme.SetBool("OlduMu", true);
        Destroy(gameObject, 3f);
        karakter.enabled = false;
        anaKontrol.GetComponent<GenelAyarlar>().DusmanSayisiGuncelle();
    }
}
