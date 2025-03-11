using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using System.Numerics;
public class Tile : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI textUI;
    [SerializeField] Image background;
    int number;
Board board;
UnityEngine.Vector3 correctPosition;
public bool IsCorrected{ private set; get; } = false;
  RectTransform rt;
    private void Awake()
    {
        rt=GetComponent<RectTransform>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void setup(Board board, int titleNumber){
  this.board= board;
    number = titleNumber;
    textUI.text = number.ToString();
    if(number == 0){
        background.enabled= false;
        textUI.enabled= false;
 
    }
    
 }
 public void SetCorrectPosition(){
    correctPosition = rt.localPosition;
 }
 public void OnPointerClick(PointerEventData eventData)
    {
  board.IsMoveTile(this);
    }
    public void OnMoveTo(UnityEngine.Vector3 end){
        StartCoroutine("MoveTo", end);

         
    }
    IEnumerator MoveTo(UnityEngine.Vector3 end){
        float elapsedtime = 0;
        float totalTime = 0.1f;
        UnityEngine.Vector3 start =rt.localPosition;
        while(elapsedtime < totalTime){
            elapsedtime += Time.deltaTime;
            rt.localPosition = UnityEngine.Vector3.Lerp(start, end, elapsedtime/ totalTime);
            yield return null;
        }
        IsCorrected= correctPosition == rt.localPosition ? true: false;
        board.CheckCompleted();
    }
}
