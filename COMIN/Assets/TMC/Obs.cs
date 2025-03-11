using UnityEngine;

public class Obs : MonoBehaviour
{
    [SerializeField] private float SP; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
          transform.Translate(Vector3.left * SP * Time.deltaTime, Space.World);
    }
}
