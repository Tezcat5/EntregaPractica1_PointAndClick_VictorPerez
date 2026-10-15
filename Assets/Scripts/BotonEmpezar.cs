using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonEmpezar : MonoBehaviour
{
public void Empezar()
{
SceneManager.LoadScene("Menu_Principal");
}
}