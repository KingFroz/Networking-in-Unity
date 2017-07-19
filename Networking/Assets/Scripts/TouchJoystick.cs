
using UnityEngine;
using UnityEngine.UI;

public class TouchJoystick : MonoBehaviour {

    private int fingerID;

    private Image bgImage;
    private Image knobImage;

    public bool alwaysVisible = false;

    public Joystick joystick;

	void Start () {
        fingerID = 0;

        if (joystick != null) {
            bgImage = joystick.GetComponent<Image>();
            knobImage = joystick.transform.GetChild(0).GetComponent<Image>();

            bgImage.enabled = false;
            knobImage.enabled = false;
        }
	}
	
	void FixedUpdate () {
        if (Input.touchCount > 0)
        {
            Touch[] myTouches = Input.touches;

            for (int i = 0; i < Input.touchCount; i++) {
                if (myTouches[i].phase == TouchPhase.Began)
                {
                    if (myTouches[i].position.x < Screen.width / 2)
                    {
                        fingerID = myTouches[i].fingerId;

                        if (joystick.GetFixed() == false) {
                            var currentPosition = bgImage.rectTransform.position;
                            currentPosition.x = myTouches[i].position.x + bgImage.rectTransform.sizeDelta.x / 2;
                            currentPosition.y = myTouches[i].position.y - bgImage.rectTransform.sizeDelta.y / 2;

                            currentPosition.x = Mathf.Clamp(currentPosition.x, 0 + bgImage.rectTransform.sizeDelta.x, Screen.width / 2);
                            currentPosition.y = Mathf.Clamp(currentPosition.y, 0, Screen.height - bgImage.rectTransform.sizeDelta.y);

                            bgImage.rectTransform.position = currentPosition;

                            bgImage.enabled = true;
                            bgImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                        } else {
                            bgImage.enabled = true;
                            bgImage.rectTransform.GetChild(0).GetComponent<Image>().enabled = true;
                        }
                    }
                }

                if (myTouches[i].phase == TouchPhase.Ended)
                {
                    if (myTouches[i].fingerId == fingerID)
                    {
                        bgImage.enabled = alwaysVisible;
                        knobImage.enabled = alwaysVisible;
                    }
                }
            }
        }
    }
}
