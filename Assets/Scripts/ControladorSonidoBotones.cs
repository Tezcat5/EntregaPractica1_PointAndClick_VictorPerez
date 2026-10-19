using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ControladorSonidoBotones : MonoBehaviour
{
    private Button boton;
    private static AudioSource audioSourceGlobal;

    [Header("Sonido al hacer clic")]
    public AudioClip sonidoClic;

    void Start()
    {
        boton = GetComponent<Button>();

        // Buscamos o creamos un canal de audio global para los efectos
        if (audioSourceGlobal == null)
        {
            GameObject objetoAudio = new GameObject("EfectosSonidoGlobal");
            audioSourceGlobal = objetoAudio.AddComponent<AudioSource>();
            DontDestroyOnLoad(objetoAudio);
        }

        // Añadimos la acción al evento de clic del botón de Unity
        boton.onClick.AddListener(ReproducirSonido);
    }

    void ReproducirSonido()
    {
        if (sonidoClic != null && audioSourceGlobal != null)
        {
            audioSourceGlobal.PlayOneShot(sonidoClic);
        }
    }
}