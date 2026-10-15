using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ControladorIntro1 : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool cambiandoEscena = false;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
        videoPlayer.loopPointReached += AlTerminarVideo;
    }

    void Update()
    {
        if (!cambiandoEscena &&
            (Input.GetMouseButtonDown(0) ||
             Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.Return)))
        {
            IrAIntro2();
        }
    }

    void AlTerminarVideo(VideoPlayer vp)
    {
        IrAIntro2();
    }

    void IrAIntro2()
    {
        if (cambiandoEscena) return;

        cambiandoEscena = true;
        SceneManager.LoadScene("Intro_2");
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
            videoPlayer.loopPointReached -= AlTerminarVideo;
    }
}