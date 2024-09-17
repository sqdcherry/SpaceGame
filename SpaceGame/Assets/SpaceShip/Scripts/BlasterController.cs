using UnityEngine;

public class BlasterController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    public void Shoot()
    {
        Debug.Log("Shoot");
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(1f, 1f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
}
