
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / 200f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
