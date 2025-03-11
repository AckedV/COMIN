using UnityEngine;
using UnityEngine.SceneManagement;
public class Stack : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] arrayBlocks; // Objetos a posicionar
    private Vector3 spawnPositions;




    [SerializeField] private bool changeSpeedMultiplier;

    GameObject[] pieces;

    GameObject currentPiece;
    int pieceIndex;
    Vector3 topPiecePosition;

    Vector3 camOffset;
    Camera cam;

    Vector3 currentPieceSize;
    Vector3 pieceSize;
    [SerializeField] float speedInit = 2.5f;
    [SerializeField] float speedMax = 5;
    float speed;
    float moveTimer = 0.0f;
    bool isAxisX;

    bool isGameOver;

    float distMove;

    Color currentColor;
    float colorHue;
    float colorSaturation;
    bool addSaturation;
    float lastColorHue = 1;

    void Start()
    {
        pieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;
        }
        pieceIndex = 0;
        currentPiece = pieces[pieceIndex];

        cam = Camera.main;
        camOffset = cam.transform.position - transform.GetChild(0).position;

        pieceSize = transform.GetChild(0).localScale;
        currentPieceSize = pieceSize;
        speed = speedInit;
        //isAxisX = true;
        //moveTimer = 90;
        SpawnPiece();

        colorHue = Random.Range(0f, 1f);
        colorSaturation = 0.5f;
        addSaturation = false;
        //UpdateColor();
        foreach(Transform pieces in transform)
        {
            //pieces.GetComponent<MeshRenderer>().material.color = currentColor;
        }

    }

    void Update()
    {
        string pieceInfo = "Nombre de la pieza: " + currentPiece.ToString() +
                           " Tamaño de la pieza: " + currentPieceSize.ToString();
                           
        Debug.Log(pieceInfo);
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {

            MovePiece();
            if (Input.GetMouseButtonDown(0))
            {
                if (PlacePiece())
                {
                    SpawnPiece();
                }
                else
                {
                    currentPiece.AddComponent<Rigidbody>();
                    Debug.Log("Game over");
                    isGameOver = true;
                }
            }
        }

    }
    void SpawnPiece()
    {
        topPiecePosition = currentPiece.transform.localPosition;
        pieceIndex--;
        if (pieceIndex < 0)
        {
            pieceIndex = transform.childCount - 1;

        }
        currentPiece = pieces[pieceIndex];
        currentPiece.transform.localScale = currentPieceSize;
        float posY = topPiecePosition.y + currentPieceSize.y;
        currentPiece.transform.localPosition = new Vector3(topPiecePosition.x, posY-0.65f, topPiecePosition.z);
        cam.transform.position = camOffset + currentPiece.transform.localPosition;
        isAxisX = !isAxisX;
        moveTimer = 90;


        if (changeSpeedMultiplier)
        {
            float speedMultiplier;
            if (currentPieceSize.x < currentPieceSize.z)
            {
                speedMultiplier = pieceSize.x / currentPieceSize.z;
            }
            else
            {
                speedMultiplier = pieceSize.z / currentPieceSize.z;
            }
            speed = speedInit + speedMultiplier * 0.5f;
            if (speed > speedMax)
            {
                speed = speedMax;
            }
        }
        if (isAxisX)
        {
            if(currentPieceSize.x <= 0.25f)
            {
                distMove = 0.25f;
            }
            else
            {
                distMove = currentPieceSize.x;
            }

        }
        else
        {
            if(currentPieceSize.z <= 0.25f)
            {
                distMove = 0.25f;
            }
            else
            {
                distMove = currentPieceSize.z;
            }
        }

        //UpdateColor();
       // currentPiece.GetComponent<MeshRenderer>().material.color = currentColor;
    
    }
    void MovePiece()
    {
        moveTimer += Time.deltaTime * speed;
        if (isAxisX)
        {
            float newPosX = topPiecePosition.x + (Mathf.Sin(moveTimer) * distMove);
            currentPiece.transform.localPosition = new Vector3(newPosX, currentPiece.transform.localPosition.y, currentPiece.transform.localPosition.z);
        }
        else
        {
            float newPosZ = topPiecePosition.z + (Mathf.Sin(moveTimer) * distMove);
            currentPiece.transform.localPosition = new Vector3(currentPiece.transform.localPosition.x, currentPiece.transform.localPosition.y, newPosZ);
        }
    }
    bool PlacePiece()
    {
        Vector3 currentPiecePosition = currentPiece.transform.localPosition;
        if (isAxisX)
        {
            float deltaX = topPiecePosition.x - currentPiecePosition.x;
            if (Mathf.Abs(deltaX) <= (currentPieceSize.x * 0.1f) && currentPieceSize.z > 0.1f)
            {
                currentPiece.transform.localPosition = new Vector3(topPiecePosition.x, currentPiecePosition.y, topPiecePosition.z);
            }
            else
            {
                currentPieceSize.x -= Mathf.Abs(deltaX);
                if (currentPieceSize.x <= 0.1f)
                {
                    return false;

                }
                float middle = topPiecePosition.x + currentPiecePosition.x / 2;
                currentPiece.transform.localPosition = new Vector3(middle - topPiecePosition.x / 2, currentPiecePosition.y, currentPiecePosition.z);
                currentPiece.transform.localScale = currentPieceSize;
                //CreateCutPiece(deltaX);
                SpawnObjects();
            }

        }
        else
        {
            float deltaZ = topPiecePosition.z - currentPiecePosition.z;
            if (Mathf.Abs(deltaZ) <= (currentPieceSize.z * 0.1f) && currentPieceSize.z > 0.1f)
            {
                currentPiece.transform.localPosition = new Vector3(topPiecePosition.x, currentPiecePosition.y, topPiecePosition.z);
            }
            else
            {
                currentPieceSize.z -= Mathf.Abs(deltaZ);
                if (currentPieceSize.z <= 0.01)
                {
                    return false;
                }
                float middle = topPiecePosition.z + currentPiecePosition.z / 2f - topPiecePosition.z / 2f;
                currentPiece.transform.localPosition = new Vector3(currentPiecePosition.x, currentPiecePosition.y, middle);
                currentPiece.transform.localScale = currentPieceSize;
                //CreateCutPiece(deltaZ);
                SpawnObjects();
            }

        }
        return true;
    }
    public void SpawnObjects()
    {
        for (int i = 0; arrayBlocks.Length > i; i++)
        {
            arrayBlocks[i].transform.localScale = currentPieceSize;
            arrayBlocks[i].transform.position = new Vector3(currentPiece.transform.position.x, currentPiece.transform.position.y, currentPiece.transform.position.z);
            
        }
    }
    void CreateCutPiece(float deltaSize)
    {

        Vector3 currentPiecePosition = currentPiece.transform.localPosition;
        Vector3 cutPiecePos = currentPiecePosition;
        Vector3 cutPieceSize = currentPieceSize;

        if (isAxisX)
        {
            float posX;
            if (deltaSize > 0)
            {
                posX = currentPiecePosition.x - currentPieceSize.x / 2;
            }
            else
            {
                posX = currentPiecePosition.x + currentPieceSize.x / 2;
            }
            posX -= deltaSize / 2;
            cutPiecePos.x = posX;
            cutPieceSize.x = Mathf.Abs(deltaSize);

        }
        else
        {
            float posZ;
            if (deltaSize > 0)
            {
                posZ = currentPiecePosition.z - currentPieceSize.z / 2;
            }
            else
            {
                posZ = currentPiecePosition.z + currentPieceSize.z / 2;
            }
            posZ -= deltaSize / 2;
            cutPiecePos.z = posZ;
            cutPieceSize.z = Mathf.Abs(deltaSize);
        }




        GameObject cutPiece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cutPiece.transform.localPosition = cutPiecePos;
        cutPiece.transform.localScale = cutPieceSize;
        cutPiece.AddComponent<Rigidbody>();
        cutPiece.GetComponent<MeshRenderer>().material.color = currentColor;
    }

    void UpdateColor()
    {
        currentColor = Color.HSVToRGB(colorHue, colorSaturation,1);
        if(!addSaturation && colorSaturation >= 1 || addSaturation && colorSaturation < 0.6 && lastColorHue == colorHue)
        {
            colorHue += 0.05f;
            if(colorHue > 1)
            {
                colorHue = 0.05f;
            }
        }
        else
        {
            lastColorHue = colorHue;
            if(addSaturation)
            {
                colorSaturation += 0.1f;
                if(colorSaturation >= 1)
                {
                    addSaturation = false;
                }

            }
            else
            {
                colorSaturation -= 0.1f;
                if(colorSaturation < 0.6f)
                {
                    addSaturation = true;
                }
            }
        }
    }
}
