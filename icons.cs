using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

//Controls the behavior of all of the non-playable parts while the user is playing the game: the reset and home button, the timer, and current record display 

public class icons : MonoBehaviour{
	//camera coordinates 
    public Camera cam; 
    private Vector2 right_point;

	//reset button 
    public Button home_button, reset_button;

	//seasaw
    public GameObject plank, bottom;
	private Vector3 init_p, init_b;

	//canvas that holds all the falling objects 
	public GameObject [] canvas_arr; 

    //falling objects
    private string [] _tags = {"Gummy bear", "Popsicle", "Bubble tea"}; 
    List<GameObject[]> obj_list = new List<GameObject[]>();
	List<List<Vector3>> init_list = new List<List<Vector3>>();

	//timer
	public TMP_Text timer_display, current_record;
	public static int seconds = 0; 

	//create a perference for the player under 'current_record'
	private const string KEY = "Player Current Record";

	//main menu 
	public GameObject game, home; 

	void Start () {
		InvokeRepeating("OutputTime", 1f, 1f);

		//retrieve record time
		current_record.text = "Current Record: " + PlayerPrefs.GetInt(KEY);

		//get coordinates the bottom right of the camera
        right_point = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, 0, cam.nearClipPlane)); 

		//home button
		Button h_btn = home_button.GetComponent<Button>();
		h_btn.onClick.AddListener(Home);

		//reset game 
		Button r_btn = reset_button.GetComponent<Button>();
		r_btn.onClick.AddListener(ResetGame);

		//inital seasaw
		init_p = plank.transform.position; 
		init_b = bottom.transform.position; 

		//initialize all types of sprites in indivuial arrays
        for(int i = 0; i < _tags.Length; i++){
			//enable to disable 
			canvas_arr[i].SetActive(true); 
            obj_list.Add(GameObject.FindGameObjectsWithTag(_tags[i])); 
        } 

		//get all initial positions 
		for(int i = 0; i < _tags.Length; i++){
			List<Vector3> tmp_list = new List<Vector3>();
            foreach(GameObject obj in obj_list[i]){
				tmp_list.Add(obj.transform.position); 
            }
			init_list.Add(tmp_list); 

			//disable cavases 
			canvas_arr[i].SetActive(false); 
        }

		//activate chosen canvas 
		canvas_arr[main_menu.chosen_idx].SetActive(true); 
	}

	//Output is called every 1 second 
    void OutputTime() {
		seconds += 1;
		timer_display.text = "TIME: " + seconds;
	}

	//reset game and go back to main menu
	void Home(){
        ResetGame(); 
		home.SetActive(true);
		main_menu.isPlay = false; 
		game.SetActive(false);
	}

	public void ResetGame(){
		//reset seasaw
		bottom.transform.position = init_b;
		plank.GetComponent<Rigidbody2D>().velocity =  new Vector2(0f, 0f);
		plank.transform.rotation = Quaternion.identity;
		plank.transform.position = new Vector3(init_p.x, init_p.y, 0f); 

		//reset falling objects
		//enable to disable
		for(int i = 0; i < _tags.Length; i++){
			canvas_arr[i].SetActive(true);
			for(int x = 0; x < obj_list[i].Length; x++){
				//place them below
				obj_list[i][x].transform.position = init_list[i][x]; 
				if(x > 3){
					//disabled  them
					obj_list[i][x].SetActive(false); 
				}
			} 
			
			canvas_arr[i].SetActive(false); 
        }

		//activate chosen canvas 
		canvas_arr[main_menu.chosen_idx].SetActive(true); 

		//reset staggering of falling objects
		falling_objects.length_arr = obj_list[main_menu.chosen_idx].Length - 4; 

		//reset timer 
		seconds = 0;
		timer_display.text = "TIME: " + seconds;
	}

    // Update is called once per frame
    void Update(){
		//update high score
		if(seconds > PlayerPrefs.GetInt(KEY)){
			PlayerPrefs.SetInt(KEY, seconds);
		}
		current_record.text = "Current Record: " + PlayerPrefs.GetInt(KEY) + " SEC";

		//reset game after the plank falls off screen
        if(plank.transform.position.y < right_point.y - 10f){
			//reset game and go back to main menu
        	Home(); 
        }
    }
}
