using UnityEngine;
using UnityEngine.UI;

public class RadialFill : MonoBehaviour
{
    [SerializeField] private Image radialImage;
    [SerializeField] private float fillDuration = 10f;  // Tiempo para llenar
    [SerializeField] private float emptyDuration = 5f;  // Tiempo para vaciar

    private float timer = 0f;
    private bool isFilling = true; // Controla si se está llenando o vaciando

    void Update()
    {
        // Detecta si se hace clic en cualquier parte de la pantalla
        if (Input.GetMouseButtonDown(0))
        {
            isFilling = false; // Cambia a vaciar
        }

        if (isFilling)
        {
            timer += Time.deltaTime;
            radialImage.fillAmount = timer / fillDuration;

            if (timer >= fillDuration)
            {
                timer = fillDuration;
            }
        }
        else
        {
            timer -= Time.deltaTime;
            radialImage.fillAmount = timer / emptyDuration;

            if (timer <= 0f)
            {
                timer = 0f;
                isFilling = true; // Vuelve a llenarse automáticamente
            }
        }
    }
}
    