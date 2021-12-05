// MiniGame made by Skyler Swagart following Pronay Peddiraju's tutorial for game basics

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class timeScript : MonoBehaviour {

	public Text counterText;
	public bool timeCounter = false;
	public float seconds, minutes;
	private float startTime;

	private GameObject MiniScene;

	// Use this for initialization
	void Start () {
		counterText = GetComponent<Text> () as Text;
		MiniScene = GameObject.FindGameObjectWithTag("MiniGame");
	}

	public void startTimer ()
    {
		timeCounter = true;
		startTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeCounter) {
			seconds = (int)((Time.timeSinceLevelLoad - startTime) % 60f);
			counterText.text = "Seconds" + ":" + seconds.ToString ("00");
		}
		if((seconds == 10) && (timeCounter == true))
        {
			timeCounter = false;
			counterText.color = Color.yellow;
			GameObject rubyObject = GameObject.Find("Ruby");
			RubyController controller = GameObject.Find("Ruby").GetComponent<RubyController>();
			controller.count++;
			controller.SetCountText();
			controller.musicSource.clip = controller.musicClipOne;
			controller.musicSource.Play();
			Destroy(MiniScene, 1);
		}
	}

	public void endGame() {
		timeCounter = false;
		counterText.color = Color.yellow;
		RubyController controller = GameObject.Find("Ruby").GetComponent<RubyController>();
		controller.count += 4;
		controller.SetCountText();
		controller.musicSource.clip = controller.musicClipOne;
		controller.musicSource.Play();
		Destroy(MiniScene, 1);
	}
}
