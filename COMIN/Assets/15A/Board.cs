using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    Vector2Int puzzleSize = new Vector2Int(4, 4);
List<Tile> tileList = new List<Tile>();
float tileDistance;
Vector3 emptyTilePosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        tileDistance= GetComponent<GridLayoutGroup>().cellSize.x+GetComponent<GridLayoutGroup>().spacing.x;
      LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
        yield return new WaitForEndOfFrame();
        foreach(Tile t in tileList){
            t.SetCorrectPosition();
        }
        
        SpawnTiles();
        StartCoroutine(ShuffleTiles());
    }

    // Update is called once per frame
   void SpawnTiles(){
  for(int y = 0; y < puzzleSize.y; y++){

    for(int x = 0; x < puzzleSize.x; x++){

        //Instantiate(titlePrefab, transform);
        GameObject newTile=Instantiate(tilePrefab, transform);
        Tile tile = newTile.GetComponent<Tile>();
        int tileNumber = y* puzzleSize.x+x+1;
        if(tileNumber == puzzleSize.x * puzzleSize.y){
            tileNumber=0;
        }
        tile.setup(this, tileNumber);
        tileList.Add(tile);
    }
  }

    }
    IEnumerator ShuffleTiles()
    {
        float elapsedtime = 0;
        float totalTime = 1;
        while(elapsedtime< totalTime){
            elapsedtime += Time.deltaTime;
            int rndIndex = Random.Range(0, puzzleSize.x * puzzleSize.y);
            tileList[rndIndex].transform.SetAsLastSibling();
            yield return null;
        }
       emptyTilePosition = tileList[tileList.Count - 1].GetComponent<RectTransform>().localPosition;
    }
    public void IsMoveTile(Tile tile)
    {
        if(Vector3.Distance(emptyTilePosition, tile.GetComponent<RectTransform>().localPosition)== tileDistance)
        {
            Vector3 goalPosition= emptyTilePosition;
            emptyTilePosition= tile.GetComponent<RectTransform>().localPosition;
            tile.OnMoveTo(goalPosition);
        }
    }
    public void CheckCompleted()
    {
        List<Tile>tiles= tileList.FindAll(x=>x.IsCorrected == true);
        Debug.Log("Correct Count:"+ tiles.Count);
        if(tiles.Count ==puzzleSize.x*puzzleSize.y-1){
            Debug.Log("Completed");
        }
    }
}
