using UnityEngine;
using System.Collections;

public class ColorPicker : MonoBehaviour {
	public Color pantsColor, skinColor, shoesColor, tshirtColor, hairColor;
	public SpriteRenderer pantsSR, armsSR, headSR, hairSR, tshirtSR, shoesSR;

	// Use this for initialization
	void Start () {
		float randomValue = UnityEngine.Random.value;
		int skin = (int) Mathf.Lerp(0f, 4, randomValue);
		if(skin > 3) {
			skin = 3;
		}
		switch(skin){
			case(0):
				skinColor = new Color(253,248,179);
				break;
			case(1):
				skinColor = new Color(253,241,82);
				break;
			case(2):
				skinColor = new Color(217,157,0);
				break;
			case(3):
				skinColor = new Color(96,75,20);
				break;
		}

		headSR.color = skinColor;
		armsSR.color = skinColor;
		int pants = Random.Range(0,5);
		switch(pants){
				case 0:
					pantsColor = Color.white;
					break;
				case 1:
					pantsColor = Color.blue;
					break;
				case 2:
					pantsColor = Color.green;
					break;
				case 3:
					pantsColor = Color.red;
					break;
				case 4:
					pantsColor = Color.yellow;
					break;
			}
		pantsSR.color = pantsColor;
		int tshirt = Random.Range(0,4);
		switch(tshirt){
				case 0:
					tshirtColor = Color.blue;
					break;
				case 1:
					tshirtColor = Color.green;
					break;
				case 2:
					tshirtColor = Color.red;
					break;
				case 3:
					tshirtColor = Color.yellow;
					break;
			}
		tshirtSR.color = tshirtColor;
		int shoes = Random.Range(0,5);
		switch(shoes){
			case 0:
				shoesColor = Color.white;
				break;
			case 1:
				shoesColor = Color.blue;
				break;
			case 2:
				shoesColor = Color.green;
				break;
			case 3:
				shoesColor = Color.red;
				break;
			case 4:
				shoesColor = Color.yellow;
				break;
		}
		shoesSR.color = shoesColor;
		int hair = Random.Range(0,4);
		switch(hair){
			case(0):
				hairColor = Color.black;
				break;
			case(1):
				hairColor = Color.red;
				break;
			case(2):
				hairColor = Color.yellow;
				break;
			case(3):
				hairColor = new Color(246,117,0);
				break;
		}
		hairSR.color = hairColor;

	}

	public void Copy(ColorPicker other){
		hairSR.color = other.hairColor;
		armsSR.color = other.skinColor;
		headSR.color = other.skinColor;
		shoesSR.color = other.shoesColor;
		tshirtSR.color = other.tshirtColor;
		pantsSR.color = other.pantsColor;
	}

}
