using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorMenu : MonoBehaviour
{
public void NuevaPartida()
{
SceneManager.LoadScene("Escena_Note");
}

public void Salir()
{
    SceneManager.LoadScene("Intro_1");
}

}