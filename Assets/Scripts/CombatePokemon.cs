using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CombatePokemon : MonoBehaviour
{
    [Header("Referencias de UI y Sprites")]
    public TextMeshProUGUI textoMensajeCombate;
    public TextMeshProUGUI textoMiVida;
    public TextMeshProUGUI textoVidaServine;
    public GameObject panelBotonesMovimientos;
    
    public GameObject objetoDewott;
    public GameObject objetoServine;

    [Header("Estadísticas de Combate")]
    public int vidaMiDewottMax = 100;
    private int vidaMiDewottActual;
    public int vidaServineMax = 100;
    private int vidaServineActual;

    private bool turnoDelJugador = false;

    void Start()
    {
        vidaMiDewottActual = vidaMiDewottMax;
        vidaServineActual = vidaServineMax;
        ActualizarUI();

        if (panelBotonesMovimientos != null) panelBotonesMovimientos.SetActive(false);
        if (objetoDewott != null) objetoDewott.SetActive(false);
        if (objetoServine != null) objetoServine.SetActive(false);

        ControladorMusica musicaManager = FindFirstObjectByType<ControladorMusica>();
        if (musicaManager != null)
        {
            musicaManager.CambiarAMusicaCombate();
        }

        StartCoroutine(SecuenciaInicioCombate());
    }

    IEnumerator SecuenciaInicioCombate()
    {
        textoMensajeCombate.text = "¡El profesor JAVIER te reta a un combate!";
        yield return new WaitForSeconds(2.5f);

        textoMensajeCombate.text = "JAVIER: ¡Vamos allá, demuestra lo que has aprendido en programación!";
        yield return new WaitForSeconds(2.8f);

        textoMensajeCombate.text = "¡JAVIER envió a Servine!";
        if (objetoServine != null) objetoServine.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        textoMensajeCombate.text = "¡VICTOR: ¡Adelante, Dewott!";
        if (objetoDewott != null) objetoDewott.SetActive(true);
        yield return new WaitForSeconds(2.5f);

        textoMensajeCombate.text = "¿Qué hará Dewott?";
        
        turnoDelJugador = true;
        if (panelBotonesMovimientos != null) panelBotonesMovimientos.SetActive(true);
    }

    // --- TUS 4 MOVIMIENTOS ---
    public void UsarMovimiento1() { EjecutarAtaqueJugador("Pistola Agua", 12, 18, 1.0f); }  
    public void UsarMovimiento2() { EjecutarAtaqueJugador("Corte Furia", 18, 25, 0.90f); } 
    public void UsarMovimiento3() { EjecutarAtaqueJugador("Rayo Hielo", 25, 35, 0.80f);  } 
    public void UsarMovimiento4() { EjecutarAtaqueJugador("Hidrobomba", 35, 50, 0.65f); }  

    void EjecutarAtaqueJugador(string nombreAtaque, int minDanio, int maxDanio, float precision)
    {
        if (!turnoDelJugador) return;

        turnoDelJugador = false;
        if (panelBotonesMovimientos != null) panelBotonesMovimientos.SetActive(false);

        textoMensajeCombate.text = $"¡VICTOR: Dewott usa {nombreAtaque}!";
        ActualizarUI();

        bool acierta = Random.value <= precision;

        StartCoroutine(ResolverAtaqueJugador(nombreAtaque, minDanio, maxDanio, acierta));
    }

    IEnumerator ResolverAtaqueJugador(string nombreAtaque, int minDanio, int maxDanio, bool acierta)
    {
        yield return new WaitForSeconds(1.8f);

        if (!acierta)
        {
            textoMensajeCombate.text = $"¡Has fallado el movimiento {nombreAtaque}!";
            yield return new WaitForSeconds(2f);
        }
        else
        {
            int danio = Random.Range(minDanio, maxDanio);
            vidaServineActual -= danio;
            if (vidaServineActual < 0) vidaServineActual = 0;

            textoMensajeCombate.text = $"¡Ha sido muy eficaz! Ha quitado {danio} PS al Servine de JAVIER.";
            ActualizarUI();

            yield return new WaitForSeconds(2f);

            if (vidaServineActual <= 0)
            {
                if (objetoServine != null) objetoServine.SetActive(false);
                StartCoroutine(FinalizarCombateConEspera(true));
                yield break;
            }
        }

        TurnoServine();
    }

    void TurnoServine()
    {
        string[] ataquesJavier = { "Látigo Cepa", "Hoja Afilada", "Finta", "Gigadrenado" };
        string ataqueElegido = ataquesJavier[Random.Range(0, ataquesJavier.Length)];

        int danioJavier = Random.Range(15, 28);
        vidaMiDewottActual -= danioJavier;
        if (vidaMiDewottActual < 0) vidaMiDewottActual = 0;

        textoMensajeCombate.text = $"JAVIER: ¡Servine, usa {ataqueElegido}!";
        ActualizarUI();

        StartCoroutine(ContinuarDespuesDelAtaqueServine(danioJavier));
    }

    IEnumerator ContinuarDespuesDelAtaqueServine(int danioJavier)
    {
        yield return new WaitForSeconds(1.8f);
        textoMensajeCombate.text = $"¡El Servine de JAVIER te ha hecho {danioJavier} de daño!";
        ActualizarUI();

        yield return new WaitForSeconds(2f);

        if (vidaMiDewottActual <= 0)
        {
            if (objetoDewott != null) objetoDewott.SetActive(false);
            StartCoroutine(FinalizarCombateConEspera(false));
            yield break;
        }

        textoMensajeCombate.text = "¿Qué hará Dewott?";
        turnoDelJugador = true;
        if (panelBotonesMovimientos != null) panelBotonesMovimientos.SetActive(true);
    }

    void ActualizarUI()
    {
        if (textoMiVida != null) textoMiVida.text = $"Dewott PS: {vidaMiDewottActual}/{vidaMiDewottMax}";
        if (textoVidaServine != null) textoVidaServine.text = $"Servine PS: {vidaServineActual}/{vidaServineMax}";
    }

    // --- ESPERA DE 10 SEGUNDOS TRAS DERROTAR O PERDER ---
    IEnumerator FinalizarCombateConEspera(bool victoria)
    {
        if (panelBotonesMovimientos != null) panelBotonesMovimientos.SetActive(false);

        if (victoria)
        {
            textoMensajeCombate.text = "¡VICTORIA! JAVIER: 'Has demostrado un nivel excelente. ¡Has aprobado el examen con matrícula!'";
        }
        else
        {
            textoMensajeCombate.text = "DERROTA... JAVIER: 'Te ha faltado depurar mejor ese código. Nos vemos en septiembre'.";
        }

        // Esperamos exactamente 10 segundos leyendo el mensaje final en el panel
        yield return new WaitForSeconds(10f);

        // Cambiamos a la escena correspondiente
        if (victoria)
        {
            SceneManager.LoadScene("Escena_Victoria");
        }
        else
        {
            SceneManager.LoadScene("Escena_Derrota");
        }
    }
}