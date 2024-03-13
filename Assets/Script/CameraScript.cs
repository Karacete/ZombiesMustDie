using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] private GameObject basla;
    [SerializeField] private GameObject eminMisin;
    [SerializeField] private GameObject loading;
    [SerializeField]
    private GameObject ogretici;

    [Header("UI")]
    [SerializeField] private Slider loadingSlider;

    [Header("Animator")]
    private Animator cameraAnimation;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        cameraAnimation = GetComponent<Animator>();
    }
    public void Uzaklas()
    {
        Time.timeScale = 1;
        basla.SetActive(false);
        cameraAnimation.Play("Uzaklasma");
    }
    IEnumerator Loading()
    {
        AsyncOperation yukleme = SceneManager.LoadSceneAsync(1);
        loading.SetActive(true);
        while (!yukleme.isDone)
        {
            float ilerleme = Mathf.Clamp01(yukleme.progress / .9f);
            loadingSlider.value = ilerleme;
            yield return null;
        }
    }
    public void OyunaBasla()
    {
        StartCoroutine(Loading());
    }
    public void EminMisin()
    {
        Time.timeScale = 1;
        cameraAnimation.Play("Dondur");
        basla.SetActive(false);
        eminMisin.SetActive(true);
    }
    public void Hayýr()
    {
        Time.timeScale = 1;
        cameraAnimation.Play("Ters Dondur");
        eminMisin.SetActive(false);
        basla.SetActive(true);
    }
    public void Cikis()
    {
        Application.Quit();
    }
    public void NasilOynanir()
    {
        Time.timeScale = 1;
        basla.SetActive(false);
        cameraAnimation.Play("Yakinlasma");
    }
    public void Kilavuz()
    {
        ogretici.SetActive(true);
    }
    public void AnaMenu()
    {
        Time.timeScale = 1;
        cameraAnimation.Play("YerineGitme");
        ogretici.SetActive(false);
        basla.SetActive(true);
    }
}
