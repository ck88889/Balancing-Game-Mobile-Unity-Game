using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; 

public class main_menu : MonoBehaviour{
    public static bool isPlay = false;

    public Button play_button, question_button, inventory_button, exit_button1, exit_button2;
    public GameObject home, game, snapshot, center_title, how_to_play; 

    //character button animation
    //icon/character selection order all have same order 
    public Button [] character_selection; 
    public Image [] character_icons;
    public string [] _tag; 

    public GameObject [] chosen_character; 
    public static int chosen_idx = 0;  

    // Start is called before the first frame update
    void Start(){
        //start game 
		Button p_btn = play_button.GetComponent<Button>();
		p_btn.onClick.AddListener(PlayGame);

        //change character button 
		Button c_btn = inventory_button.GetComponent<Button>();
		c_btn.onClick.AddListener(() => OpenWindow(snapshot, center_title));

        //how to play button 
        Button q_btn = question_button.GetComponent<Button>(); 
        q_btn.onClick.AddListener(() => OpenWindow(how_to_play, center_title));

		//exit button 
		Button e1_btn = exit_button1.GetComponent<Button>();
		e1_btn.onClick.AddListener(ExitWindow);

        Button e2_btn = exit_button2.GetComponent<Button>();
		e2_btn.onClick.AddListener(ExitWindow);

        //character selection
        for(int i = 0; i < character_selection.Length; i++){
            Button tmp = character_selection[i].GetComponent<Button>(); 
            tmp.onClick.AddListener(CharacterChange); 

            //only show default icon  
            if(i != 0){
                character_icons[i].enabled = false;
            }
        }
    }

    //change tabs
	void OpenWindow(GameObject on, GameObject off){
		on.SetActive(true);
		off.SetActive(false);
	}

    void ExitWindow(){
        center_title.SetActive(true); 
        snapshot.SetActive(false);
        how_to_play.SetActive(false);
    }

    //change characters 
    void CharacterChange(){
        var selected = EventSystem.current.currentSelectedGameObject;
        for(int i = 0; i < character_icons.Length; i++){
            if(selected.tag == _tag[i]){
                character_icons[i].enabled = true; 
                chosen_idx = i; 
            }else{
                character_icons[i].enabled = false;
            }
        }

        OpenWindow(center_title, snapshot); 
    }

    void PlayGame(){
        if(isPlay){
            isPlay = false;
        }else{
            isPlay = true; 
            icons.seconds = 0; 
        }
    }

    // Update is called once per frame
    void Update(){
        if(isPlay){
            game.SetActive(true);
            for(int i = 0; i < chosen_character.Length; i++){
                if(i == chosen_idx){
                    chosen_character[i].SetActive(true); 
                }else{
                    chosen_character[i].SetActive(false); 
                }
            } 
            home.SetActive(false);
        }else{
            game.SetActive(false);
            home.SetActive(true);
        }
    }
}
