using UnityEngine;
using UnityEngine.SceneManagement;

public class Twins : MonoBehaviour
{
    [SerializeField] private float AFS = 90;
    [SerializeField] private Spawn spawn;
    [SerializeField] private GameObject[] circles;  // Corregí el espacio entre 'GameObject[]'
    [SerializeField] private GameObject gameOverScreen;  // UI de Game Over

    private bool gameOver = false;  // Se inicializa en 'false' para evitar valores indefinidos

    void Start()
    {
        gameOver = false; // Asegura que el juego comience en estado activo
        gameOverScreen.SetActive(false);  // Asegura que la pantalla de "Game Over" esté oculta al inicio
    }

    void Update()
    {
        if (!gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0); // Reinicia la escena
            }
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            AFS = -AFS; // Invierte la dirección de rotación al hacer clic
        }

        transform.Rotate(new Vector3(0, 0, AFS * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obs") && !gameOver)
        {
            gameOver = true;  // Cambia el estado a "game over"
            spawn.stop = true; // Detiene el spawn de objetos (asegúrate de que 'stop' sea una variable pública en la clase Spawn)
            
            Debug.Log("Game Over");

            // Desactiva los objetos en el array circles
            foreach (GameObject circle in circles)
            {
                circle.SetActive(false);
            }

            // Muestra la pantalla de Game Over
            if (gameOverScreen != null)
            {
                gameOverScreen.SetActive(true);
            }

            // Pausa el juego
            Time.timeScale = 0;  // Detiene el tiempo del juego
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;  // Reanuda el juego
        SceneManager.LoadScene(0); // Reinicia la escena
    }
}
