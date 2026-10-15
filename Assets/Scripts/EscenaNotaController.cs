using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EscenaNotaController : MonoBehaviour
{
    public TMP_Text textoRecuerdo;
    public GameObject botonContinuar;

    [TextArea(5, 12)]
    public string mensaje =
        "Has recorrido pueblos, superado desafíos y derrotado a los líderes de gimnasio.\n\n" +
        "Cada combate te ha preparado para este momento.\n\n" +
        "Ahora solo queda una última prueba: tu próximo destino es Monlau.\n\n" +
        "Allí te espera el campeón. Descansa, prepara tu equipo y recuerda todo lo que has aprendido.\n\n" +
        "Tu aventura aún no ha terminado.";

    public float velocidadEscritura = 0.04f;

    private bool textoTerminado = false;

    void Start()
    {
        botonContinuar.SetActive(false);
        textoRecuerdo.text = "";

        StartCoroutine(EscribirTexto());
    }

    IEnumerator EscribirTexto()
    {
        foreach (char letra in mensaje)
        {
            textoRecuerdo.text += letra;
            yield return new WaitForSeconds(velocidadEscritura);
        }

        textoTerminado = true;
        botonContinuar.SetActive(true);
    }

    public void Continuar()
    {
        if (!textoTerminado) return;

        SceneManager.LoadScene("Casa_Arriba");
    }
}