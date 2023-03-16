using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scaler : MonoBehaviour{

    //camera coordinates 
    public Camera cam;
    private Vector2 left_point, right_point;

    //seasaw
    public GameObject plane_seasaw, bottom_block;  
    public Button home_button, reset_button; 

    //falling objects
    private string [] _tags = {"Gummy bear", "Popsicle", "Bubble tea"}; 
    private int [] multiplier = {140, 135, 110}; 
    List<GameObject[]> obj_list = new List<GameObject[]>();

    // Start is called before the first frame update
    void Start(){
        //fix landscape mode
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        //get coordinates the top left of camera
        left_point = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane));
        
        //get coordinates the bottom right of the camera
        right_point = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane));

        //set reset button to the right button corner and home button to bottom left corner
        reset_button.transform.position = new Vector3(cam.pixelWidth - 500, right_point.y + 30, 0f);
        home_button.transform.position = new Vector3(500, right_point.y + 80, 0f);  

        //66% of the screen for the seasaw plank 
        float plane_width_prev = plane_seasaw.transform.localScale.x; 
        float plane_width = ((right_point.x * 2)/3) * 2; 
        float plane_height = plane_seasaw.transform.localScale.y * (plane_width /plane_width_prev/2); 
        plane_seasaw.transform.localScale = new Vector3(plane_width, plane_height, 0); 

        //2% of the screen for the seasaw block
        float block_size = (right_point.x * 2) * 0.02f; 
        bottom_block.transform.localScale = new Vector3(block_size, block_size, 0);

        //initialize all types of sprites in indivuial arrays 
        for(int i = 0; i < _tags.Length; i++){
            obj_list.Add(GameObject.FindGameObjectsWithTag(_tags[i])); 
        }

        //resize falling objects
        for(int i = 0; i < _tags.Length; i++){
            foreach(GameObject obj in obj_list[i]){
                float temp = obj.transform.localScale.y * ((plane_seasaw.transform.localScale.x/100 * multiplier[i])/plane_seasaw.transform.localScale.x);
                obj.transform.localScale = new Vector3(temp, temp, 0);
            }
        }
    }
}
