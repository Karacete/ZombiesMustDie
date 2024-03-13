using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenelAyarlar : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject gameOver;
    public GameObject congrat;
    public GameObject pause;

    [Header("Saðlýk")]
    public Image health;
    float saglik;

    [Header("Silah")]
    public List<GameObject> silahlar;

    [Header("Dusman")]
    public List<GameObject> dusmanlar;
    float cikisSuresi;
    int baslangicDusman;
    int kalanDusman;
    public TextMeshProUGUI kalanDusmanText;

    [Header("Hedef Nokta")]
    public List<GameObject> hedefler;

    [Header("Cikis Nokta")]
    public List<GameObject> cikislar;

    [Header("Muzik")]
    private AudioSource oyunMuzigi;

    [Header("Diger")]
    public static bool oyunDurduMu;

    // Start is called before the first frame update
    void Start()
    {
        baslangicDusman = 30;
        kalanDusman = baslangicDusman;
        kalanDusmanText.text = kalanDusman.ToString();
        oyunDurduMu = false;
        StartCoroutine(DusmanCikar());
        cikisSuresi = 5f;
        saglik = 1;
        oyunMuzigi = GetComponent<AudioSource>();
        oyunMuzigi.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !oyunDurduMu)
            SilahAktif(0);

        if (Input.GetKeyDown(KeyCode.Alpha2) && !oyunDurduMu)
            SilahAktif(1);

        if (Input.GetKeyDown(KeyCode.Alpha3) && !oyunDurduMu)
            SilahAktif(2);

        if (Input.GetKeyDown(KeyCode.Escape) && !oyunDurduMu)
            Pause();
    }
    void SilahAktif(int silahNumara)
    {
        foreach (var silah in silahlar)
        {
            silah.SetActive(false);
            silahlar[silahNumara].SetActive(true);
        }
    }
    IEnumerator DusmanCikar()
    {
        while (true)
        {
            if (baslangicDusman != 0)
            {
                int dusman = Random.Range(0, 5);
                int cikis = Random.Range(0, 2);
                int hedef = Random.Range(0, 2);
                GameObject obje = Instantiate(dusmanlar[dusman], cikislar[cikis].transform.position, Quaternion.identity);
                obje.GetComponent<Dusman>().HedefBelirle(hedefler[hedef]);
                baslangicDusman -= 1;
            }
            yield return new WaitForSeconds(cikisSuresi);
        }
    }
    public void HealthBar(float miktar)
    {
        saglik -= miktar;
        health.fillAmount = saglik;
        if (saglik <= 0)
        {
            GameOver();
        }
    }
    public void Pause()
    {
        oyunDurduMu = true;
        pause.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void DevamEt()
    {
        oyunDurduMu = false;
        pause.SetActive(false);
        Time.timeScale = 1;
    }
    public void AnaMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void GameOver()
    {
        oyunDurduMu = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOver.SetActive(true);
        Time.timeScale = 0;
    }
    public void BastanBaslat()
    {
        oyunDurduMu = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    public void DusmanSayisiGuncelle()
    {
        kalanDusman -= 1;
        if (kalanDusman == 0 && saglik > 0)
        {
            oyunDurduMu = true;
            congrat.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        kalanDusmanText.text = kalanDusman.ToString();
    }
}
