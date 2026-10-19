using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetoInteractuable : MonoBehaviour
{
    [Header("Información del objeto")]
    public string nombreObjeto = "Objeto";

    [TextArea] public string textoMirar = "No ves nada especial.";
    [TextArea] public string textoHablar = "No parece que pueda responderte.";
    [TextArea] public string textoInteractuar = "No ocurre nada.";

    [Header("Cambio de escena / Puerta / Bus")]
    public string escenaDestino = "";
    public bool requiereLlave = false;
    [TextArea] public string textoSinLlave = "La puerta está cerrada. Debería hablar con Mamá primero.";

    [Header("Punto de Aparición al cambiar de escena")]
    public string nombrePuntoEntrada = "";

    [Header("Bloqueo Puerta Exterior (Requiere Carta)")]
    public bool requiereCartaParaEntrar = false;
    [TextArea] public string textoSinCarta = "YO: Antes debo revisar el buzón.";

    [Header("Bloqueo Escaleras / Cama (Requiere Hablar con Mamá)")]
    public bool requiereHablarConMadrePostCarta = false;
    [TextArea] public string textoSinHablarConMadre = "YO: Primero debería enseñarle la carta a Mamá.";

    [Header("Bloqueo Puerta Campeón (Requiere Hablar con Guardia)")]
    public bool requiereHablarConGuardia = false;
    [TextArea] public string textoSinHablarConGuardia = "YO: El guardia de la puerta me bloquea el paso. Debería hablar con él.";

    [Header("Bloqueo Parada de Bus (Requiere Tarjeta Bus)")]
    public bool requiereTarjetaBus = false;
    public string nombreTarjetaBus = "Tarjeta del Bus";
    [TextArea] public string textoSinTarjetaBus = "YO: Necesito la Tarjeta del Bus para subir.";

    [Header("Lógica de la Madre")]
    public bool esLaMadre = false;
    public string nombreObjetoAEntregarMadre = "Llaves de la Casa";

    [Header("Lógica del Guardia de Monlau")]
    public bool esElGuardia = false;
    [TextArea] public string textoGuardiaAutorizado = "GUARDIA: Ya te he registrado. Puedes pasar a la Sala del Campeón.";

    [Header("Lógica del Profesor Javier (Combate Final)")]
    public bool esElProfesorJavier = false;
    private static bool javierHablado = false; // Controla si ya le has hablado para desbloquear el combate

    // VARIABLES DE ESTADO (ESTÁTICAS)
    public static bool tieneLlaveDeCasa = false;
    public static bool cartaYaRecogida = false;
    public static bool madreSabeDeLaCarta = false;
    public static bool guardiaPermitePaso = false;

    [TextArea] public string textoMadreDespuesDeCarta = "MAMÁ: ¡A ver!... ¡Ooh, debes ir a Monlau a enfrentarte al campeón! ¡Qué suerte! Muchísima suerte, cariño. Coge la tarjeta del bus de tu cuarto, está en el ordenador.";
    [TextArea] public string textoMadreRecordatorioTarjeta = "MAMÁ: ¡Muchísima suerte en tu viaje, cariño! Recuerda coger la tarjeta del bus que está al lado del ordenador en tu cuarto.";

    [Header("Lógica del Buzón / Cama / Cofres (Entrega Objeto)")]
    public bool entregaObjetoAlInteractuar = false;
    public string nombreObjetoAEntregar = "Carta de Monlau";
    private bool objetoYaEntregado = false;
    [TextArea] public string textoYaEntregado = "Ya cogí el objeto, no queda nada más.";

    [Header("Objeto con dos estados (opcional)")]
    public GameObject objetoEstadoAlternativo;
    public bool estadoAlternativoActivo = false;
    [TextArea] public string textoMirarAlternativo = "";
    [TextArea] public string textoInteractuarAlternativo = "";

    // Variable estática compartida entre escenas para el punto de aparición
    public static string puntoEntradaDestino = "";

    // Método para reiniciar el progreso al iniciar la prueba
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetearEstadoAlEmpezar()
    {
        tieneLlaveDeCasa = false;
        cartaYaRecogida = false;
        madreSabeDeLaCarta = false;
        guardiaPermitePaso = false;
        javierHablado = false;
        puntoEntradaDestino = "";
        Debug.Log("🔄 Progreso de la partida reiniciado para la prueba.");
    }

    public void Interactuar(string accion)
    {
        Debug.Log("🔴 Interactuar llamado en " + nombreObjeto + ". Acción recibida: " + accion);

        switch (accion)
        {
            case "MIRAR":
                MostrarTexto(
                    estadoAlternativoActivo && !string.IsNullOrEmpty(textoMirarAlternativo)
                    ? textoMirarAlternativo
                    : textoMirar
                );
                break;

            case "HABLAR":
                if (esLaMadre)
                {
                    if (!tieneLlaveDeCasa)
                    {
                        tieneLlaveDeCasa = true;

                        if (Inventario.Instance != null)
                        {
                            Inventario.Instance.AgregarObjeto(nombreObjetoAEntregarMadre);
                        }

                        MostrarTexto(textoHablar);
                    }
                    else if (cartaYaRecogida && !madreSabeDeLaCarta)
                    {
                        madreSabeDeLaCarta = true;
                        MostrarTexto(textoMadreDespuesDeCarta);
                    }
                    else if (madreSabeDeLaCarta)
                    {
                        MostrarTexto(textoMadreRecordatorioTarjeta);
                    }
                    else
                    {
                        MostrarTexto("MAMÁ: Recuerda mirar el buzón fuera antes de irte.");
                    }
                }
                else if (esElGuardia)
                {
                    if (!guardiaPermitePaso)
                    {
                        guardiaPermitePaso = true;
                        MostrarTexto(textoHablar); // Mensaje de registro/bienvenida
                    }
                    else
                    {
                        MostrarTexto(textoGuardiaAutorizado); // Recordatorio de que puedes pasar
                    }
                }
                else if (esElProfesorJavier)
                {
                    // Al hablarle por primera vez, marcamos que ya le hemos hablado y mostramos su diálogo
                    javierHablado = true;
                    MostrarTexto(textoHablar);
                }
                else
                {
                    MostrarTexto(textoHablar);
                }
                break;

            case "INTERACTUAR":
                // Si es el profesor Javier y ya le hemos hablado previamente, lanzamos el combate al interactuar
                if (esElProfesorJavier)
                {
                    if (!javierHablado)
                    {
                        MostrarTexto("YO: Debería hablar con él antes de retarle.");
                        return;
                    }

                    MostrarTexto(textoInteractuar); // Muestra el texto de aceptar el reto

                    // Activa la secuencia de transición "VS" y salto al combate
                    GestorInicioCombate gestorCombate = FindFirstObjectByType<GestorInicioCombate>();
                    if (gestorCombate != null)
                    {
                        gestorCombate.IniciarSecuenciaTransicion();
                    }
                    else
                    {
                        Debug.LogWarning("No se encontró el GestorInicioCombate en la escena.");
                    }
                    return;
                }

                // 1. Recoger objetos (Cama, Buzón, etc.)
                if (entregaObjetoAlInteractuar)
                {
                    if (requiereHablarConMadrePostCarta && (!cartaYaRecogida || !madreSabeDeLaCarta))
                    {
                        MostrarTexto(textoInteractuar);
                        return;
                    }

                    if (!cartaYaRecogida && nombreObjetoAEntregar == "Carta de Monlau")
                    {
                        cartaYaRecogida = true;

                        if (Inventario.Instance != null)
                        {
                            Inventario.Instance.AgregarObjeto(nombreObjetoAEntregar);
                        }

                        MostrarTexto(textoInteractuar);
                    }
                    else if (!objetoYaEntregado)
                    {
                        objetoYaEntregado = true;

                        if (Inventario.Instance != null)
                        {
                            Inventario.Instance.AgregarObjeto(nombreObjetoAEntregar);
                        }

                        MostrarTexto("YO: ¡Aquí estaba la Tarjeta del Bus! Me la guardo en la mochila.");
                    }
                    else
                    {
                        MostrarTexto(textoYaEntregado);
                    }
                    return;
                }

                // =======================================================
                // COMPROBACIONES Y BLOQUEOS PARA CAMBIO DE ESCENA
                // =======================================================

                // A. Bloqueo Puerta Sala del Campeón (Exige hablar con el guardia primero)
                if (requiereHablarConGuardia && !guardiaPermitePaso)
                {
                    MostrarTexto(textoSinHablarConGuardia);
                    return;
                }

                // B. Bloqueo Escaleras Habitación
                if (requiereHablarConMadrePostCarta && (!cartaYaRecogida || !madreSabeDeLaCarta))
                {
                    MostrarTexto(textoSinHablarConMadre);
                    return;
                }

                // C. Bloqueo Puerta Entrada desde la Calle
                if (requiereCartaParaEntrar && !cartaYaRecogida)
                {
                    MostrarTexto(textoSinCarta);
                    return;
                }

                // D. Bloqueo Autobús
                if (requiereTarjetaBus)
                {
                    bool tieneTarjeta = Inventario.Instance != null && Inventario.Instance.TieneObjeto(nombreTarjetaBus);

                    if (!tieneTarjeta)
                    {
                        MostrarTexto(textoSinTarjetaBus);
                        return;
                    }
                }

                // E. Bloqueo Puerta Salida de Casa
                if (requiereLlave && !tieneLlaveDeCasa)
                {
                    MostrarTexto(textoSinLlave);
                    return;
                }

                // =======================================================
                // CAMBIO DE ESCENA
                // =======================================================
                if (!string.IsNullOrEmpty(escenaDestino))
                {
                    puntoEntradaDestino = nombrePuntoEntrada;
                    SceneManager.LoadScene(escenaDestino);
                    return;
                }

                // Lógica de estados alternativos
                if (objetoEstadoAlternativo != null)
                {
                    estadoAlternativoActivo = !estadoAlternativoActivo;
                    objetoEstadoAlternativo.SetActive(estadoAlternativoActivo);

                    MostrarTexto(
                        estadoAlternativoActivo && !string.IsNullOrEmpty(textoInteractuarAlternativo)
                        ? textoInteractuarAlternativo
                        : textoInteractuar
                    );
                }
                else
                {
                    MostrarTexto(textoInteractuar);
                }
                break;
        }
    }

    private void MostrarTexto(string mensaje)
    {
        if (SistemaDialogo.Instancia != null)
        {
            SistemaDialogo.Instancia.MostrarTexto(mensaje);
        }
        else
        {
            Debug.LogWarning("No se encuentra SistemaDialogo en la escena.");
        }
    }
}