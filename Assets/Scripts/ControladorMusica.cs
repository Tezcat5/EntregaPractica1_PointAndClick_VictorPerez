using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMusica : MonoBehaviour
{
    private static ControladorMusica instancia;
    private AudioSource audioSource;

    [Header("Pistas de Música por Zonas")]
    public AudioClip musicaMenuPrincipal;    // Menu_Principal
    public AudioClip musicaIntro1;           // Intro_1
    public AudioClip musicaIntro2;           // Intro_2
    public AudioClip musicaEscenaNote;       // Escena_Note
    public AudioClip musicaCasa;             // Casa_Arriba, Casa_Abajo
    public AudioClip musicaPueblo;           // Pueblo
    public AudioClip musicaMonlauExterior;   // Monlau_Exterior (Calle)
    public AudioClip musicaMonlauRecepcion;  // MonlauRecepcion (Interior)
    public AudioClip musicaSalaCampeon;      // Sala_Campeon (Ambiente previo)
    public AudioClip musicaCombateJavier;    // Música exclusiva para el Combate contra Javier
    public AudioClip musicaVictoria;         // Escena_Victoria
    public AudioClip musicaDerrota;          // Escena_Derrota

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene escena, LoadSceneMode modo)
    {
        if (audioSource == null) return;

        AudioClip nuevaMusica = null;
        string nombreEscena = escena.name;

        // 1. Menú Principal
        if (nombreEscena == "Menu_Principal")
        {
            nuevaMusica = musicaMenuPrincipal;
        }
        // 2. Intros y Escena de Nota
        else if (nombreEscena == "Intro_1")
        {
            nuevaMusica = musicaIntro1;
        }
        else if (nombreEscena == "Intro_2")
        {
            nuevaMusica = musicaIntro2;
        }
        else if (nombreEscena == "Escena_Note")
        {
            nuevaMusica = musicaEscenaNote;
        }
        // 3. Casa del protagonista
        else if (nombreEscena == "Casa_Arriba" || nombreEscena == "Casa_Abajo")
        {
            nuevaMusica = musicaCasa;
        }
        // 4. Pueblo / Calles exteriores
        else if (nombreEscena == "Pueblo")
        {
            nuevaMusica = musicaPueblo;
        }
        // 5. Monlau Exterior (Calle)
        else if (nombreEscena == "Monlau_Exterior")
        {
            nuevaMusica = musicaMonlauExterior;
        }
        // 6. Monlau Recepción (Interior)
        else if (nombreEscena == "MonlauRecepcion")
            {
            nuevaMusica = musicaMonlauRecepcion;
        }
        // 7. Sala del Campeón (Antes de luchar)
        else if (nombreEscena == "Sala_Campeon")
        {
            nuevaMusica = musicaSalaCampeon;
        }
        // 8. Pantalla de Victoria
        else if (nombreEscena == "Escena_Victoria")
        {
            nuevaMusica = musicaVictoria;
        }
        // 9. Pantalla de Derrota
        else if (nombreEscena == "Escena_Derrota")
        {
            nuevaMusica = musicaDerrota;
        }

        // Si se asigna una pista (o se deja vacía intencionadamente para silencio), gestiona el audio
        if (audioSource.clip != nuevaMusica)
        {
            audioSource.clip = nuevaMusica;
            
            if (nuevaMusica != null)
            {
                audioSource.loop = true;
                audioSource.Play();
                Debug.Log("🎵 Cambiando música para la escena: " + nombreEscena);
            }
            else
            {
                audioSource.Stop();
                Debug.Log("🔇 Silencio en la escena: " + nombreEscena);
            }
        }
    }

    // Método público para activar la música de combate contra Javier cuando empiece la pelea
    public void CambiarAMusicaCombate()
    {
        if (audioSource != null && musicaCombateJavier != null)
        {
            audioSource.clip = musicaCombateJavier;
            audioSource.loop = true;
            audioSource.Play();
            Debug.Log("⚔️ ¡Activando música de combate contra Javier!");
        }
    }
}