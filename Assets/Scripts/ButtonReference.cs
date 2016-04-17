using UnityEngine;
using System.Collections;

public class ButtonReference {
	public static KeyCode AButtonKeyCode(int controllerNumber) {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		switch(controllerNumber){
		case 1:
			return KeyCode.Joystick1Button16;
		case 2:
			return KeyCode.Joystick2Button16;
		case 3:
			return KeyCode.Joystick3Button16;
		case 4:
			return KeyCode.Joystick4Button16;
		default:
			break;
		}
		#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
		switch(controllerNumber){
		case 1:
		return KeyCode.Joystick1Button0;
		case 2:
		return KeyCode.Joystick2Button0;
		case 3:
		return KeyCode.Joystick3Button0;
		case 4:
		return KeyCode.Joystick4Button0;
		default:
		break;
		}
		#endif
		Debug.LogError("Unknown controller button A attempted access. Controller number: "+controllerNumber);
		return KeyCode.Z;	// HACK lawl
	}

	public static KeyCode BButtonKeyCode(int controllerNumber) {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		switch(controllerNumber){
		case 1:
			return KeyCode.Joystick1Button17;
		case 2:
			return KeyCode.Joystick2Button17;
		case 3:
			return KeyCode.Joystick3Button17;
		case 4:
			return KeyCode.Joystick4Button17;
		default:
			break;
		}
		#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
		switch(controllerNumber){
		case 1:
		return KeyCode.Joystick1Button1;
		case 2:
		return KeyCode.Joystick2Button1;
		case 3:
		return KeyCode.Joystick3Button1;
		case 4:
		return KeyCode.Joystick4Button1;
		default:
		break;
		}
		#endif
		Debug.LogError("Unknown controller button B attempted access. Controller number: "+controllerNumber);
		return KeyCode.Z;	// HACK lawl
	}

	public static KeyCode XButtonKeyCode(int controllerNumber) {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		switch(controllerNumber){
		case 1:
			return KeyCode.Joystick1Button18;
		case 2:
			return KeyCode.Joystick2Button18;
		case 3:
			return KeyCode.Joystick3Button18;
		case 4:
			return KeyCode.Joystick4Button18;
		default:
			break;
		}
		#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
		switch(controllerNumber){
		case 1:
		return KeyCode.Joystick1Button2;
		case 2:
		return KeyCode.Joystick2Button2;
		case 3:
		return KeyCode.Joystick3Button2;
		case 4:
		return KeyCode.Joystick4Button2;
		default:
		break;
		}
		#endif
		Debug.LogError("Unknown controller button X attempted access. Controller number: "+controllerNumber);
		return KeyCode.Z;	// HACK lawl
	}

	public static KeyCode YButtonKeyCode(int controllerNumber) {
		#if UNITY_EDITOR || UNITY_STANDALONE_OSX
		switch(controllerNumber){
		case 1:
			return KeyCode.Joystick1Button19;
		case 2:
			return KeyCode.Joystick2Button19;
		case 3:
			return KeyCode.Joystick3Button19;
		case 4:
			return KeyCode.Joystick4Button19;
		default:
			Debug.LogError("Unknown controller button Y attempted access. Controller number: "+controllerNumber);
			break;
		}
		#elif UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
		switch(controllerNumber){
		case 1:
		return KeyCode.Joystick1Button3;
		case 2:
		return KeyCode.Joystick2Button3;
		case 3:
		return KeyCode.Joystick3Button3;
		case 4:
		return KeyCode.Joystick4Button3;
		default:
		break;
		}
		#endif
		return KeyCode.Z;	// HACK lawl
	}
}
