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
    public string nombrePuntoEntrada = ""; // Identificador del punto donde aparecerá el jugador

    [Header("Bloqueo Puerta Exterior (Requiere Carta)")]
    public bool requiereCartaParaEntrar = false;
    [TextArea] public string textoSinCarta = "YO: No debo volver a entrar todavía, primero tengo que revisar el buzón.";

    [Header("Bloqueo Parada de Bus (Requiere Tarjeta Bus)")]
    public bool requiereTarjetaBus = false;
    public string nombreTarjetaBus = "Tarjeta del Bus";
    [TextArea] public string textoSinTarjetaBus = "YO: Necesito la Tarjeta del Bus para subir al autobús. Tendré que buscarla en casa.";

    [Header("Lógica de la Madre")]
    public bool esLaMadre = false;
    public string nombreObjetoAEntregarMadre = "Llaves de la Casa";
    private static bool tieneLlaveDeCasa = false; // Compartido entre escenas
    private static bool madreSabeDeLaCarta = false; // Estado para el nuevo diálogo

    [TextArea] public string textoMadreDespuesDeCarta = "MAMÁ: ¡A ver qué pone en la carta!... ¡Ooh, se ve que debes ir a Monlau para enfrentarte al campeón Pokémon! Pues muchísima suerte cariño. Recuerda pillar la tarjeta del bus que está en tu cuarto, al lado del ordenador.";
    [TextArea] public string textoMadreRecordatorioTarjeta = "MAMÁ: Recuerda coger la tarjeta del bus de tu habitación antes de marcharte.";

    [Header("Lógica del Buzón / Cofres / Objetos (Entrega Objeto)")]
    public bool entregaObjetoAlInteractuar = false;
    public string nombreObjetoAEntregar = "Carta de Monlau";
    private static bool cartaYaRecogida = false; // Compartido entre escenas
    private bool objetoYaEntregado = false;
    [TextArea] public string textoYaEntregado = "El buzón está vacío. Ya cogí la carta.";

    [Header("Objeto con dos estados (opcional)")]
    public GameObject objetoEstadoAlternativo;
    public bool estadoAlternativoActivo = false;
    [TextArea] public string textoMirarAlternativo = "";
    [TextArea] public string textoInteractuarAlternativo = "";

    // Variable estática para recordar por qué punto entramos en la nueva escena
    public static string puntoEntradaDestino = "";

    public void Interactuar(string accion)
    {
        Debug.Log(
            "🔴 Interactuar llamado en " + nombreObjeto +
            ". Acción recibida: " + accion
        );

        switch (accion)
        {
            case "MIRAR":
                MostrarTexto(
                    estadoAlternativoActivo &&
                    !string.IsNullOrEmpty(textoMirarAlternativo)
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
                else
                {
                    MostrarTexto(textoHablar);
                }
                break;

            case "INTERACTUAR":
                // 1. Lógica del Buzón / Cofres
                if (entregaObjetoAlInteractuar)
                {
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

                        MostrarTexto(textoInteractuar);
                    }
                    else
                    {
                        MostrarTexto(textoYaEntregado);
                    }
                    return;
                }

                // 2. Comprobación Parada de Autobús
                if (requiereTarjetaBus)
                {
                    bool tieneTarjeta = Inventario.Instance != null && Inventario.Instance.TieneObjeto(nombreTarjetaBus);

                    if (!tieneTarjeta)
                    {
                        MostrarTexto(textoSinTarjetaBus);
                        return;
                    }
                }

                // 3. Puerta exterior que exige coger la carta para volver a entrar
                if (requiereCartaParaEntrar && !cartaYaRecogida)
                {
                    MostrarTexto(textoSinCarta);
                    return;
                }

                // 4. Puerta interior que exige llave
                if (requiereLlave && !tieneLlaveDeCasa)
                {
                    MostrarTexto(textoSinLlave);
                    return;
                }

                // 5. Cambio de escena si pasa las comprobaciones
                if (!string.IsNullOrEmpty(escenaDestino))
                {
                    // Guardamos el punto por el que entraremos a la nueva escena
                    puntoEntradaDestino = nombrePuntoEntrada;
                    SceneManager.LoadScene(escenaDestino);
                    return;
                }

                // 6. Estados alternativos
                if (objetoEstadoAlternativo != null)
                {
                    estadoAlternativoActivo = !estadoAlternativoActivo;

                    objetoEstadoAlternativo.SetActive(
                        estadoAlternativoActivo
                    );

                    MostrarTexto(
                        estadoAlternativoActivo &&
                        !string.IsNullOrEmpty(textoInteractuarAlternativo)
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
            Debug.LogWarning(
                "No se encuentra SistemaDialogo en la escena."
            );
        }
    }
}