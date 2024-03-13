using System.Collections;
using TMPro;
using UnityEngine;

public class Keles : MonoBehaviour
{
    [Header("Ayarlar")]
    bool atesEdebilirMi;
    float atesEtmeSikligi;
    float atesSikligi;
    float menzil;
    int kalanMermi;
    int toplamMermi;
    int sarjorKapasitesi = 20;
    int tasimaKapasitesi = 100;
    int atilanMermi;
    int alinanMermi;
    float darbeGucu;
    int originalCamDeger = 50;
    int yakinCamDeger = 40;

    [Header("Kamera")]
    public Camera cam;

    [Header("Sesler")]
    public AudioSource atesSesi;
    public AudioSource sarjorSesi;
    public AudioSource bosSarjorSesi;

    [Header("Efektler")]
    public ParticleSystem atesEfekt;
    public ParticleSystem mermiIzi;
    public ParticleSystem kanEfekti;

    [Header("Animasyon")]
    Animator kelesAnimation;

    [Header("TextMeshPro")]
    public TextMeshProUGUI kalanText;
    public TextMeshProUGUI toplamText;

    [Header("Objeler")]
    public GameObject mermiKovaný;
    public GameObject cýkacakYer;
    public GameObject cross;
    [SerializeField]
    private GameObject mermiAl;

    void Start()
    {
        atesEdebilirMi = true;
        toplamMermi = 100;
        //animator componentini geriTepme degiskenine atiyoruz
        kelesAnimation = GetComponent<Animator>();
        kalanMermi = 20;
        atesSikligi = 0.1f;
        menzil = 100;
        Yazdir();
    }

    // Update is called once per frame
    void Update()
    {
        Yazdir();
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1) && !GenelAyarlar.oyunDurduMu)
        {
            //sol mouse tiklandiginda ates sikliginda AtesEt() fonksiyonu calisacak
            if (atesEdebilirMi && Time.time > atesEtmeSikligi && kalanMermi != 0)
            {
                AtesEt();
                kelesAnimation.Play("Geri Tepme");
                //disaridan verilen ates sikigi ayari
                atesEtmeSikligi = Time.time + atesSikligi;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && kalanMermi == 0)
            {
                atesEdebilirMi = false;
                bosSarjorSesi.Play();
                kelesAnimation.Play("Geri Tepme");
            }
        }
        if (Input.GetKey(KeyCode.Mouse0) && Input.GetKey(KeyCode.Mouse1) && !GenelAyarlar.oyunDurduMu)
        {
            //sol mouse tiklandiginda ates sikliginda AtesEt() fonksiyonu calisacak
            if (atesEdebilirMi && Time.time > atesEtmeSikligi && kalanMermi != 0)
            {
                AtesEt();
                //disaridan verilen ates sikigi ayari
                atesEtmeSikligi = Time.time + atesSikligi;
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && kalanMermi == 0)
            {
                atesEdebilirMi = false;
                bosSarjorSesi.Play();
                kelesAnimation.Play("Geri Tepme");
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !Input.GetKey(KeyCode.LeftShift))
            ZoomYap(true);

        if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKey(KeyCode.LeftShift))
            ZoomYap(false);

        if (Input.GetKeyDown(KeyCode.R))
        {
            atilanMermi = sarjorKapasitesi - kalanMermi;
            kelesAnimation.Play("Sarjor Degisimi");
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
            atesEdebilirMi = true;
            Yazdir();
        }
        //mermi al
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
        void MermiAl()
        {
            mermiAl.SetActive(false);
            alinanMermi = tasimaKapasitesi - toplamMermi;
            if (alinanMermi > 20)
            {
                toplamMermi += 20;
            }

            else
            {
                toplamMermi = tasimaKapasitesi;
            }
            Yazdir();
        }


    }
    void ZoomYap(bool durum)
    {
        if (durum)
        {
            cross.SetActive(false);
            kelesAnimation.SetBool("zoomYapildiMi", true);
            cam.fieldOfView = yakinCamDeger;
        }
        else
        {
            cross.SetActive(true);
            kelesAnimation.SetBool("zoomYapildiMi", false);
            cam.fieldOfView = originalCamDeger;
        }
    }
    void AtesEt()
    {
        //ates sesi ve ates efekti calisacak
        //Geri Tepme animasyonunu calistirir
        atesSesi.Play();
        atesEfekt.Play();
        StartCoroutine(KameraTitret(.02f, .05f));
        kalanMermi -= 1;
        kalanText.text = kalanMermi.ToString();
        //bir isin gondermeye yarar
        RaycastHit hit;
        //isin kameranin tam orta noktasindan ileriye disaridan verilen menzil kadar gidecek
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, menzil))
        {
            if (hit.transform.gameObject.CompareTag("Enemy") || hit.transform.gameObject.CompareTag("BigEnemy"))
            {
                if (hit.transform.gameObject.CompareTag("BigEnemy"))
                    darbeGucu = 5;
                else
                    darbeGucu = 20;
                //kan efekti isinin son noktasinda ve acili bir sekilde gelecek
                Instantiate(kanEfekti, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusman>().CanAzalt(darbeGucu);
            }
            else
                //mermi efekti isinin son noktasinda ve acili bir sekilde gelecek
                Instantiate(mermiIzi, hit.point, Quaternion.LookRotation(hit.normal));
        }
        GameObject kovan = Instantiate(mermiKovaný, cýkacakYer.transform.position, cýkacakYer.transform.rotation);
        Rigidbody rb = kovan.GetComponent<Rigidbody>();
        rb.AddRelativeForce(new Vector3(-5f, 1, 0) * 20);
    }
    void SarjorSesi()
    {
        sarjorSesi.Play();
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
    void Yazdir()
    {
        kalanText.text = kalanMermi.ToString();
        toplamText.text = toplamMermi.ToString();
    }
}
