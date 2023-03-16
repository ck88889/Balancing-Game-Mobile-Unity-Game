using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random; 

public class falling_objects : MonoBehaviour{

    //camera coordinates 
    public Camera cam; 
    private Vector2 left_point, right_point; 

    //falling objects
    public string _tag; 
    private GameObject [] falling_obj;

    //stagger falling obj
    public GameObject main_page; 
    public static int length_arr; 

    //Generate a random decmial with 2 decimal points between to numbers 
    float GeneratePosition(float min, float max){
        Random rand = new Random();

        int min_ = (int)(min); 
        int max_ = (int)(max); 
        int rand_num = rand.Next((min_ * 100), max_ * 100); 

        return ((float)rand_num)/100f;
    }

    // Start is called before the first frame update
    void Start(){
        InvokeRepeating("OutputTime", 0.5f, 0.5f);

        //get coordinates the top left of camera
        left_point = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        
        //get coordinates the bottom right of the camera
        right_point = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane)); 

        //get all list of all the sprites 
        falling_obj = GameObject.FindGameObjectsWithTag(_tag);

        //stagger the number of objects falling by only startin with three
        length_arr = falling_obj.Length - 4; 
        for(int i = 0; i < falling_obj.Length; i++){
            if(i > 3){
                falling_obj[i].SetActive(false);
            }
        }
    }

    //Output is called every 1 second 
    void OutputTime() {
       //stagger falling obj
       try {
            if(length_arr > -1){
                falling_obj[length_arr].SetActive(true);
                length_arr -= 1; 
            }
       }catch{
            //in case of an error just release all the falling sprites
            for(int i = 0; i < falling_obj.Length; i++){
                falling_obj[i].SetActive(true);
            }
       }
    }

    // Update is called once per frame
    void Update(){
        //stagger falling obj
        foreach(GameObject obj in falling_obj){
            //only objects that are visible can fall
            if(obj.activeSelf){
                //return sprite to the top of the screen if it falls out of view
                if(obj.transform.position.y < right_point.y){
                    //reset position 
                    obj.transform.position = new Vector3(GeneratePosition(left_point.x, right_point.x),
                                            GeneratePosition(left_point.y + 1.0f , left_point.y * 4)); 
                    //freeze velocity so it does not acceralate too quickly
                    obj.GetComponent<Rigidbody2D>().velocity =  new Vector2(0f, 0f);
                }
            }
        }
    }
}
