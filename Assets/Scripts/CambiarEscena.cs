using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public string nombreEscena = "Casa_Abajo";

    public void IrAEscena()
    {
        SceneManager.LoadScene(nombreEscena);
    }
}