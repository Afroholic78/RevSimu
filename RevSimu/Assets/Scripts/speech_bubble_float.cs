using UnityEngine;
using System.Collections;

public class speech_bubble_float : MonoBehaviour {

    private bool toggle = true;
    private float x = 0.01f;
    private float duration = 1.0f;
    public Vector3 startMarker;
    public Vector3 endDest;
    public Vector3 origin;
    public Vector3 leftDest;
    private float startTime;
    public Transform target;
    public float smooth = 1000.0F;
    public float speed = 0f;
    private float derpTime = 0;

	// Use this for initialization
	void Start () {
        startMarker = transform.position;
        endDest = transform.position + new Vector3(0.5f, 0, 0);
        leftDest = transform.position - new Vector3(0.5f, 0, 0);
        origin = transform.position;
	}

    void OnMouseDown()
    {
        Debug.Log("Test log");
    }

    void speedincrease()
    {
        speed += 0.01f;
    }
    void fade()
    {
        Color textureColor = renderer.material.color;
        if (toggle)
        {
            textureColor.a = duration - Mathf.PingPong(Time.time, duration) / duration;
            if (textureColor.a == 0)
            {
                toggle = false;
            }
        }
        else
        {
            textureColor.a = duration - Mathf.PingPong(Time.time + speed, duration) / duration;
            renderer.material.color = textureColor;
            Invoke("speedincrease", 2);
        }
    }

	// Update is called once per frame
	void Update () {
        //Destroy after 5 seconds CHANGE THIS DEPENDING ON HOW LONG CHOICE IS DISPLAYED
        //Destroy(gameObject, 5);

        Invoke("fade", 2);
	}
}
