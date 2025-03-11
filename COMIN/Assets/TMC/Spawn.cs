using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject obsPrefab;
    List<GameObject> obslist;
     [SerializeField] float spawnTime =1;
      [SerializeField] Transform[] spawnPos;
     float timer;

public bool stop;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        obslist= new List<GameObject>();
        timer= spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!stop){
             timer-= Time.deltaTime;
        if(timer<0){
            spawn();
        }
        }
       
    }
    void spawn(){
        int rndPos = Random.Range(0, spawnPos.Length);
GameObject newObs=GetFirstObsNoActive();
newObs.transform.position= spawnPos[rndPos].position;
newObs.SetActive(true);
timer = spawnTime;
    }
    GameObject GetFirstObsNoActive(){
        for(int i = 0; i < obslist.Count; i++){
            if(!obslist[i].activeInHierarchy){
                return obslist[i];
            }
        }
        return Instanceobs();
    }
    GameObject Instanceobs(){
        GameObject newObs= Instantiate(obsPrefab);
     newObs.name=newObs.name + "("+ obslist.Count +")";
     newObs.transform.parent= gameObject.transform;
     newObs.SetActive(false);
     obslist.Add(newObs);
     return newObs;
    }
}
