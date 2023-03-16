using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Controls the animation of the main menu items: the title card and the how to play page

public class animation : MonoBehaviour{
    enum Direction{right, left}; 
    enum Phase{on, off}; 

    //title card animation 
    public GameObject titlecard; 
    public float rotate_speed; 
    private float current_target = 350f;  

    //background animation 
    public string _tag; 
    private GameObject [] animated_obj; 
    private const float percentage = 0.33f; 

    //how to play values
    private int msg_part = 0; 
    private int char_idx = 0;
    private string [] MSG = {"WELCOME TO THE GAME: TILT!", "THE OBJECTIVE OF THIS GAME IS TO MOVE\nTHE BEAM TO BALANCE IT FOR AS LONG AS\nPOSSIBLE. GOOD LUCK!"};

    //how to play objects 
    public Camera cam; 
    public GameObject canvas_instructions, screenshot, pointer; 
    public TMP_Text _textbox, _soundeffect; 
    private Phase tmp_phase = Phase.on; 

    //animate movement of seasaw
    public GameObject seasaw; 
    public float move_speed;
    private Direction current_dir = Direction.right; 
    private float cam_width; 

    //rotate towards a certain angle 
    float RotateTowards(float target, GameObject obj){
        float angle = Mathf.MoveTowardsAngle(obj.transform.eulerAngles.z, target, 
                      rotate_speed * Time.deltaTime);
        obj.transform.eulerAngles = new Vector3(0, 0, angle);

        return angle; 
    }

    // Start is called before the first frame update
    void Start(){
       //stagger showing the message
       InvokeRepeating("OutputTime", 0.15f, 0.15f);
       //soundeffect 
       InvokeRepeating("SoundEffect", 1.0f, 1.0f);

       //scaling
       animated_obj = GameObject.FindGameObjectsWithTag(_tag); 
       foreach(GameObject obj in animated_obj){
            //only 33% of orginial size
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * percentage, 
                                       obj.transform.localScale.y * percentage, 0f);
       }

       cam_width = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight, cam.nearClipPlane)).x * -1f; 
    }

    void OutputTime(){
        //display messages 
        if(canvas_instructions.activeSelf && msg_part < MSG.Length){
            //character array 
            char [] tmp = MSG[msg_part].ToCharArray();

            //display message  
            string msg_tmp = ""; 
            for(int i = 0; i < char_idx; i++){
                msg_tmp += tmp[i].ToString(); 
            }

            _textbox.text = msg_tmp;

            //stop at the end of the parts
            //at the end of the message change to the next part
            if(char_idx == tmp.Length){
                char_idx = 0; 
                msg_part += 1;  
            }else{
                char_idx += 1; 
            }
        }else if(!canvas_instructions.activeSelf){
            msg_part = 0;
            char_idx = 0;  
        }
    }

    void SoundEffect(){
        //click sound effect 
        if(canvas_instructions.activeSelf){
            switch(tmp_phase){
                case Phase.on:
                    _soundeffect.text = "CLICK!"; 
                    tmp_phase = Phase.off; 
                    break; 
                case Phase.off:
                    _soundeffect.text = "";
                    tmp_phase = Phase.on; 
                    break; 
            }
        }
    }

    // Update is called once per frame
    void Update(){
        //rotate the title card back and forth
        if(RotateTowards(current_target, titlecard) == 350f){
            current_target = 10f; 
        }else if(RotateTowards(current_target, titlecard) == 10f){
            current_target = 350f; 
        }
        RotateTowards(current_target, titlecard); 

        //when the instructions are not selected by user, reset animation 
        if(!canvas_instructions.activeSelf){
            msg_part = 0; 
        }

        //move seasaw back and forth 
        if(canvas_instructions.activeSelf){
            if(seasaw.transform.position.x > screenshot.transform.position.x + (cam_width * 8)){
                current_dir = Direction.left; 
            }else if(seasaw.transform.position.x < screenshot.transform.position.x - (cam_width * 8)){
                current_dir = Direction.right;
            }

            switch(current_dir){
                case Direction.right:
                    seasaw.transform.position += Vector3.right * move_speed * Time.deltaTime;

                    //change pointer location 
                    pointer.transform.position = new Vector3(screenshot.transform.position.x + (cam_width * 30), 
                        pointer.transform.position.y, pointer.transform.position.z); 
                    pointer.transform.eulerAngles = new Vector3(0, 0, 0f);

                    _soundeffect.transform.position = new Vector3(screenshot.transform.position.x + (cam_width * 28), 
                        _soundeffect.transform.position.y, _soundeffect.transform.position.z); 
                    _soundeffect.transform.eulerAngles = new Vector3(0, 0, 20f);

                    break; 
                case Direction.left:
                    seasaw.transform.position += Vector3.left * move_speed * Time.deltaTime;

                    //change pointer location
                    pointer.transform.position = new Vector3(screenshot.transform.position.x - (cam_width * 30), 
                        pointer.transform.position.y, pointer.transform.position.z); 
                    pointer.transform.eulerAngles = new Vector3(0, 0, -90f);

                    _soundeffect.transform.position = new Vector3(screenshot.transform.position.x - (cam_width * 28)
                        , _soundeffect.transform.position.y, _soundeffect.transform.position.z); 
                    _soundeffect.transform.eulerAngles = new Vector3(0, 0, -20f);
                    break; 
            }
        }
    }
}
