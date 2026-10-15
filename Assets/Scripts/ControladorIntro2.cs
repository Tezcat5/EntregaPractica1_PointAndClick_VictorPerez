using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ControladorIntro2 : MonoBehaviour
{
private VideoPlayer videoPlayer;
private bool cambiandoEscena = false;

public GameObject botonContinuar;
public float segundosParaMostrar = 2f;

void Awake()
{
    videoPlayer = GetComponent<VideoPlayer>();
}

void Start()
{
    videoPlayer.loopPointReached += AlTerminarVideo;

    // Ocultar el botón al empezar
    botonContinuar.SetActive(false);

    // Mostrarlo después de los segundos indicados
    Invoke(nameof(MostrarBoton), segundosParaMostrar);
}

void Update()
{
    // Ya no cambiamos de escena al hacer clic en cualquier parte.
    // Ahora se utiliza el botón.
}

void MostrarBoton()
{
    if (!cambiandoEscena)
    {
        botonContinuar.SetActive(true);
    }
}

void AlTerminarVideo(VideoPlayer vp)
{
    if (cambiandoEscena) return;

    cambiandoEscena = true;
    SceneManager.LoadScene("Intro_1");
}

public void IrAlMenu()
{
    if (cambiandoEscena) return;

    cambiandoEscena = true;
    SceneManager.LoadScene("Menu_Principal");
}

void OnDestroy()
{
    if (videoPlayer != null)
    {
        videoPlayer.loopPointReached -= AlTerminarVideo;
    }
}

}