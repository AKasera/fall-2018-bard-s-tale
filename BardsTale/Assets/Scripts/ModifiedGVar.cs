using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifiedGVar : MonoBehaviour {

	// Keybindings data
	public Canvas ui;
	//Hotbar-related fields
	private Image[] onArray;
	private Text[] valArray;
	private GameObject hotbar;
	private bool hidden;

	//Slider fields
	public Slider TSlide;
	public Slider VSlide;
	//Momentum Bar
	public Image momentumBar;

	//Health fields
	private Sprite fullHeart;
	private Sprite emptyHeart;
	private Image[] hearts;
	private const int maxHearts = 3;
	private int currHeart;

	// Use this for initialization
	void Start () {
		initHearts();
		initSlides(); //set sliders and momentum bar
		initHotbar();
	}

	// Update is called once per frame
	void Update () {
		updateHotbar();//Changes sprites to red or green accordingly

		//The following are just for testing
		if (Input.GetKeyDown(KeyCode.E)) {
			changeMomentum(.1f);
		}

		if(Input.GetKeyDown(KeyCode.W)) {
			changeMomentum(-.1f);
		}

		if(Input.GetKeyDown(KeyCode.Z)) {
			damage();
		}

		if(Input.GetKeyDown(KeyCode.X)) {
			heal();
		}
	}

	//Initialization methods
	private void initHotbar() {
		hidden=false;
		hotbar = ui.transform.Find("Hotbar").gameObject;
		GameObject[] hotkeys = new GameObject[6];
		hotkeys[0] = hotbar.transform.Find("A").gameObject;
		hotkeys[1] = hotbar.transform.Find("S").gameObject;
		hotkeys[2] = hotbar.transform.Find("D").gameObject;
		hotkeys[3] = hotbar.transform.Find("F").gameObject;
		hotkeys[4] = hotbar.transform.Find("G").gameObject;
		hotkeys[5] = hotbar.transform.Find("H").gameObject;

		onArray = new Image[6];
		valArray = new Text[6];

		for(int i = 0; i< 6; i++) {
			onArray[i]=hotkeys[i].GetComponentInChildren<Image>();
			valArray[i]=hotkeys[i].GetComponentInChildren<Text>();
		}
	}

	private void initHearts() {
		currHeart = maxHearts;
		fullHeart = Resources.Load <Sprite> ("Sprites/Full_Heart");
		emptyHeart = Resources.Load <Sprite> ("Sprites/Empty_Heart");
		GameObject healthbar = ui.transform.Find("Healthbar").gameObject;
		hearts = new Image[maxHearts];
		for(int i = 0; i < maxHearts; ++i) {
			hearts[i] = healthbar.transform.Find("Heart "+(i+1)).GetComponent<Image>();
		}
	}

	private void initSlides() {
		momentumBar.fillAmount = 0.5f;
		TSlide = ui.transform.Find("Tempo/Tempo Slider").gameObject.GetComponent<Slider>();
		VSlide = ui.transform.Find("Volume/Volume Slider").gameObject.GetComponent<Slider>();
	}

	private void updateHotbar() {
		//Hotbar is pressed down
		if(Input.GetKeyDown(KeyCode.A)) {
			onArray[0].color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.S)) {
			onArray[1].color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.D)) {
			onArray[2].color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.F)) {
			onArray[3].color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.G)) {
			onArray[4].color = Color.green;
		}
		if(Input.GetKeyDown(KeyCode.H)) {
			onArray[5].color = Color.green;
		}

		//Hotbar is released
		if(Input.GetKeyUp(KeyCode.A)) {
			onArray[0].color = Color.red;
		}
		if(Input.GetKeyUp(KeyCode.S)) {
			onArray[1].color = Color.red;
		}
		if(Input.GetKeyUp(KeyCode.D)) {
			onArray[2].color = Color.red;
		}
		if(Input.GetKeyUp(KeyCode.F)) {
			onArray[3].color = Color.red;
		}
		if(Input.GetKeyUp(KeyCode.G)) {
			onArray[4].color = Color.red;
		}
		if(Input.GetKeyUp(KeyCode.H)) {
			onArray[5].color = Color.red;
		}
		//Use tab to hide the hotbar!
		if(Input.GetKeyDown(KeyCode.Tab)) {
			if(!hidden) {
				hotbar.transform.Translate(new Vector3(0f,0f,1500f));
				hidden = !hidden;
			}
			else {
				hotbar.transform.Translate(new Vector3(0f,0f,-1500f));
				hidden = !hidden;
			}
		}
	}

	private void setHealth(int m) {
		if(m<0)
			m=0;
		if(m>maxHearts)
			m=maxHearts;
		int i=0;
		for(; i< m; ++i)
			hearts[i].sprite =fullHeart;
		for(; i<maxHearts; ++i)
			hearts[i].sprite = emptyHeart;
		currHeart=m;
	}

	//Public methods which the Bard class can Use
	public void changeNote(int i, string s) {
		valArray[i].text = s;
	}

	public void damage() {
		damage(1);
	}

	public void damage(int d) {
		setHealth(currHeart-d);
	}
	public void heal() {
		heal(1);
	}
	public void heal(int h) {
		setHealth(currHeart+h);
	}

	public void fullHeal() {
		setHealth(maxHearts);
	}

	public void setMomentum(float x) {
		momentumBar.fillAmount = x;
	}

	public void changeMomentum(float x) {
		momentumBar.fillAmount +=x;
	}

	public float getTempo() {
		return TSlide.value;
	}

	public float getVolume() {
		return VSlide.value;
	}
}