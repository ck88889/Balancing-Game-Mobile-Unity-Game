using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class seasaw : MonoBehaviour{
    enum Direction{right, left}; 
    private Direction swipe; 

    //camera coordinates 
    public Camera cam; 
    private Vector2 left_point, right_point; 
    private Vector2 starting_position, direction; 
    
    private GameObject plank, bottom_block; 
    private float y_position; 
    public float speed; 

    // Start is called before the first frame update
    void Start(){
        plank = GameObject.Find("Plank");
        bottom_block = GameObject.Find("Triangle base");
        y_position = bottom_block.transform.position.y; 

        //get coordinates the top left of camera
        left_point = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        
        //get coordinates the bottom right of the camera
        right_point = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane)); 
    }

    // Update is called once per frame
    void Update(){
        //keep bottom_block block upright and in place vertically
        bottom_block.transform.rotation = Quaternion.identity;
        bottom_block.transform.position = new Vector3(bottom_block.transform.position.x, y_position, 0f);

        //calculate direction of user swipe  
        if (Input.touchCount > 0){
            //get touch object 
            Touch touch = Input.GetTouch(0);
            Vector2 touch_converted = cam.ScreenToWorldPoint(touch.position); 

            //click left side or right side of the seasaw to move in that direction
            if(touch_converted.x < plank.transform.position.x){
                swipe = Direction.left;
            }else{
                swipe = Direction.right;
            }

            //move the base of the seasaw
            switch(swipe){
                case Direction.right:
                    //only move within screen
                    if(transform.position.x < right_point.x){
                        //transform.Rotate(0f, 0f, degrees*-1, Space.Self);
                        transform.position += Vector3.right * speed * Time.deltaTime;
                    }
                    break; 
                case Direction.left:
                    //only move within screen
                    if(transform.position.x > left_point.x){
                        transform.position += Vector3.left * speed * Time.deltaTime; 
                    }
                    break; 
            }
        }
    }
}
