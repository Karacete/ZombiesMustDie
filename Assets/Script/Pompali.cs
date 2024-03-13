using System.Collections;
using TMPro;
using UnityEngine;

public class Pompali : MonoBehaviour
{
    [Header("Ayarlar")]
    bool atesEdebilirMi;
    int kalanMermi;
    int toplamMermi;
    int sarjorKapasitesi;
    int tasimaKapasitesi;
    int atilanMermi;
    int alinanMermi;
    int menzil;
    float darbeGucu;

    [Header("Kamera")]
    public Camera cam;

    [Header("Sesler")]
    public AudioSource mermiAtisSesi;
    public AudioSource sarjorSesi;
    public AudioSource bosSarjorSesi;

    [Header("Efektler")]
    public ParticleSystem mermiIzi;
    public ParticleSystem kanIzi;
    public ParticleSystem atesEfekt;

    [Header("Animasyon")]
    Animator pompaliAniation;

    [Header("Text")]
    public TextMeshProUGUI kalanText;
    public TextMeshProUGUI toplamText;

    [Header("GameObject")]
    private GameObject oyunDurduMu;
    [SerializeField]
    private GameObject mermiAl;

    void Start()
    {
        atesEdebilirMi = true;
        toplamMermi = 70;
        kalanMermi = 10;
        sarjorKapasitesi = 10;
        menzil = 30;
        tasimaKapasitesi = 70;
        Yazdir();
        pompaliAniation = GetComponent<Animator>();
        pompaliAniation = GetComponent<Animator>();
        pompaliAniation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Yazdir();
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GenelAyarlar.oyunDurduMu)
        {
            if (atesEdebilirMi && kalanMermi != 0)
                AtesEt();
            else
            {
                atesEdebilirMi = false;
                bosSarjorSesi.Play();
                pompaliAniation.Play("Tetik Çekme");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            atilanMermi = sarjorKapasitesi - kalanMermi;
            if (atilanMermi > toplamMermi)
            {
                kalanMermi += toplamMermi;
                toplamMermi = 0;
            }
            else
            {
                toplamMermi -= atilanMermi;
                kalanMermi = sarjorKapasitesi;
            }
            Yazdir();
            pompaliAniation.Play("Sarjor Degisimi");
            atesEdebilirMi = true;

        }

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5))
        {
            if (hit.transform.gameObject.CompareTag("Mermi Kutusu"))
            {
                mermiAl.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (toplamMermi != tasimaKapasitesi)
                    {
                        MermiAl();
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            else
                mermiAl.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
            ZoomYap(true);

        if (Input.GetKeyUp(KeyCode.Mouse1))
            ZoomYap(false);

    }
    void MermiAl()
    {
        mermiAl.SetActive(false);
            alinanMermi = tasimaKapasitesi - toplamMermi;
            if (alinanMermi > 10)
                toplamMermi += 10;
            else
                toplamMermi = tasimaKapasitesi;
            Yazdir();

    }
    void AtesEt()
    {
        pompaliAniation.Play("Geri Tepme");
    }
    IEnumerator KameraTitret(float titremeSuresi, float magnitude)
    {
        Vector3 originalPos = cam.transform.localPosition;
        float gecenSure = 0.0f;
        while (gecenSure < titremeSuresi)
        {
            float titremePos = Random.Range(-1f, 1) * magnitude;
            cam.transform.localPosition = new(titremePos, originalPos.y, originalPos.z);
            gecenSure += Time.deltaTime;
            yield return null;
        }
        cam.transform.localPosition = originalPos;
    }
    void AtesSesi()
    {
        mermiAtisSesi.Play();
        atesEfekt.Play();
        StartCoroutine(KameraTitret(.10f, .2f));
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, menzil))
        {
            if (hit.transform.gameObject.CompareTag("Enemy") || hit.transform.gameObject.CompareTag("BigEnemy"))
            {
                if (hit.transform.gameObject.CompareTag("BigEnemy"))
                    darbeGucu = 20;
                else
                    darbeGucu = 50;
                Instantiate(kanIzi, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusman>().CanAzalt(darbeGucu);
            }
            else
            {
                Instantiate(mermiIzi, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        kalanMermi -= 1;
        kalanText.text = kalanMermi.ToString();
    }
    void ZoomYap(bool durum)
    {
        if (durum)
        {
            pompaliAniation.SetBool("zoomYapildiMi", true);
        }
        else
        {
            pompaliAniation.SetBool("zoomYapildiMi", false);
        }
    }
    void SarjorSesi()
    {
        sarjorSesi.Play();
    }
    void Yazdir()
    {
        kalanText.text = kalanMermi.ToString();
        toplamText.text = toplamMermi.ToString();
    }
}
