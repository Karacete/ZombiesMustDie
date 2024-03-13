using System.Collections;
using TMPro;
using UnityEngine;

public class Sniper : MonoBehaviour
{
    [Header("Resimler")]
    public GameObject cross;
    public GameObject scope;
    [SerializeField]
    private GameObject mermiAl;

    [Header("Islemler")]
    int kalan;
    int toplam;
    int atilanMermi;
    int sarjorKapasitesi;
    int tasimaKapasitesi;
    int alinanMermi;
    float darbeGucu;

    [Header("Animasyon")]
    Animator sniperAnimation;

    [Header("Efektler")]
    public ParticleSystem ates;
    public ParticleSystem mermiIzi;
    public ParticleSystem kan;

    [Header("Sesler")]
    public AudioSource sarjorDegisimi;
    public AudioSource atesSesi;

    [Header("Kamera")]
    public Camera cam;
    float camFieldPov;
    float yaklasmaPov;

    [Header("TextMesh")]
    public TextMeshProUGUI kalanText;
    public TextMeshProUGUI toplamText;
    void Start()
    {
        sniperAnimation = GetComponent<Animator>();
        kalan = 5;
        toplam = 20;
        sarjorKapasitesi = 5;
        tasimaKapasitesi = 20;
        Yazdir();
        camFieldPov = cam.fieldOfView;
        yaklasmaPov = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Yazdir();
        if (Input.GetKeyDown(KeyCode.Mouse0) && !GenelAyarlar.oyunDurduMu)
        {
            if (kalan != 0)
                TetikCek();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            atilanMermi = sarjorKapasitesi - kalan;
            sniperAnimation.Play("Sarjor Degisimi");
            if (atilanMermi > toplam)
            {
                kalan += toplam;
                toplam = 0;
            }
            else
            {
                kalan = sarjorKapasitesi;
                toplam -= atilanMermi;
            }
            Yazdir();
        }
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 5))
        {
            if (hit.transform.gameObject.CompareTag("Mermi Kutusu"))
            {
                mermiAl.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if(toplam!=tasimaKapasitesi)
                    {
                        MermiAl();
                        Destroy(hit.transform.gameObject);
                    }
                }
            }
            else
                mermiAl.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1)&&!Input.GetKey(KeyCode.LeftShift))
        {
            ZoomYap(true);
}
        if (Input.GetKeyUp(KeyCode.Mouse1) || Input.GetKey(KeyCode.LeftShift))
{
    ZoomYap(false);
}

    }
    void ZoomYap(bool durum)
{
    if (durum)
    {
        cross.SetActive(false);
        cam.cullingMask = ~(1 << 6);
        sniperAnimation.SetBool("zoomYapildiMi", true);
        cam.fieldOfView = yaklasmaPov;
        scope.SetActive(true);
    }
    else
    {
        scope.SetActive(false);
        cam.cullingMask = -1;
        sniperAnimation.SetBool("zoomYapildiMi", false);
        cam.fieldOfView = camFieldPov;
        cross.SetActive(true);
    }
}

private void MermiAl()
{
        mermiAl.SetActive(false);
        alinanMermi = tasimaKapasitesi - toplam;
        if (alinanMermi > 2)
            toplam += 2;
        else
            toplam = tasimaKapasitesi;
    Yazdir();
}

    void TetikCek()
    {
        sniperAnimation.Play("Geri Tepme");
    }
    void AtesEt()
    {
        ates.Play();
        atesSesi.Play();
        kalan -= 1;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 100))
        {
            if (hit.transform.gameObject.CompareTag("Enemy") || hit.transform.gameObject.CompareTag("BigEnemy"))
            {
                if (hit.transform.gameObject.CompareTag("BigEnemy"))
                    darbeGucu = 25;
                else
                    darbeGucu = 100;
                Instantiate(kan, hit.point, Quaternion.LookRotation(hit.normal));
                hit.transform.gameObject.GetComponent<Dusman>().CanAzalt(darbeGucu);
            }

            else
                Instantiate(mermiIzi, hit.point, Quaternion.LookRotation(hit.normal));
        }
        StartCoroutine(KameraTitret(.10f, .2f));
        Yazdir();
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
        kalanText.text = kalan.ToString();
        toplamText.text = toplam.ToString();
    }
}
