using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera mainCamera;
    Vector3 direction;
    void Start()
    {
        // Kameranı alırıq
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera != null)
        {
            Vector3 direction = mainCamera.transform.position - transform.position;

            // Y oxunda hərəkət etməməsi üçün sıfırlayırıq
            direction.x = 0;
            direction.z = 0;

            // Rotasiyanı təyin edirik
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Kameranın yuxarı-aşağı dönməsini istəyirik (Y rotasını) - əksinə 180 dərəcə rotasiya edirik
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x + 180f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
