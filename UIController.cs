using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController instance;
	private void Awake() //awake is called at object activation, before start()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject); //make object persistent accross scenes
		}
		else
		{
			Destroy(gameObject); //there's already an instance, do not create a new one on load
		}
	}

	public Slider healthBar;
	public Image fadeScreen;
	public float fadeSpeed = 2.0f;

	
	[Header("Collectibles HUD")]
	public TMP_Text ammoText;
	public TMP_Text minesText;
	public TMP_Text missilesText;
	public Image doubleJumpIcon;
	public Image missileIcon;

	private bool fadingIn, fadingOut;

	void Update()
	{
		if (fadingOut)
		{
			fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1.0f, fadeSpeed * Time.deltaTime));
			if(fadeScreen.color.a == 1.0f) fadingOut = false;
		}
		else if (fadingIn)
		{
			fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0.0f, fadeSpeed * Time.deltaTime));
			if (fadeScreen.color.a == 0.0f) fadingIn = false;
		}

	}

	public void UpdateHealth(int currentHealth,int maxHealth)
	{
		healthBar.maxValue = maxHealth;
		healthBar.value = currentHealth;
	}


	public void UpdateAmmo(int bullets)
	{
		if (ammoText !=null) 
			ammoText.text = "Ammo: "+bullets.ToString();
	}

	
	public void UpdateMines(int mines)
	{
		if (minesText != null) 
			minesText.text = "Mines : "+ mines.ToString();
	}

	
	public void UpdateMissiles(int missiles)
	{
		if (missilesText != null) 
			missilesText.text ="Missiles: "+  missiles.ToString();
	}

	
	public void UpdateAbilities(bool doubleJumpUnlocked,bool smartMissilesUnlocked)
	{
		if (doubleJumpIcon != null)
			doubleJumpIcon.color = new Color(1,1, 1,doubleJumpUnlocked ? 1.0f : .25f);
		
		if (missileIcon != null) 
			missileIcon.color= new Color(1,1, 1,smartMissilesUnlocked ? 1.0f : .25f);
	}

	public void fadeOut() //to black
	{
		fadingIn = false;
		fadingOut = true;

	}
	public void fadeIn() //from black
	{
		fadingIn = true;
		fadingOut = false;

	}

}
