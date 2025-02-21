using UnityEngine;

public class Stack : MonoBehaviour
{
    GameObject[] pieces;
    GameObject currentPiece;
    int pieceIndex;
    Vector3 topPiecePosition;

    Vector3 camOffset;
    Camera cam;

    Vector3 currentPieceSize;
    Vector3 pieceSize;
    [SerializeField] float speedInit = 2.5f;
    float speed;
    float moveTimer = 0.0f;
    bool isAxisX;
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
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        { 
            SpawnPiece();
        }
        MovePiece();
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
        float posY = topPiecePosition.y + currentPieceSize.y;
        currentPiece.transform.localPosition = new Vector3(topPiecePosition.x, posY, topPiecePosition.z);
        cam.transform.position = camOffset+ currentPiece.transform.position;
    }
    void MovePiece()
    {
        moveTimer += Time.deltaTime * speed;
        if(isAxisX)
        {
            float newPosX = topPiecePosition.x+(Mathf.Sin(moveTimer) * currentPieceSize.x);
            currentPiece.transform.localPosition = new Vector3(newPosX, currentPiece.transform.localPosition.y, currentPiece.transform.localPosition.z);
        }
        else
        {
            float newPosZ = topPiecePosition.z+(Mathf.Sin(moveTimer)*currentPieceSize.z);
            currentPiece.transform.localPosition = new Vector3(currentPiece.transform.localPosition.x,currentPiece.transform.localPosition.y,newPosZ);
        }
    }
}
