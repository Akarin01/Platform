using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform launchTransform;

    public void Launch()
    {
        GameObject projectile =  Instantiate(ProjectilePrefab, launchTransform.position, Quaternion.identity);

        int direction = transform.localScale.x > 0 ? 1 : -1;
        Vector3 originScale = projectile.transform.localScale;
        projectile.transform.localScale = new Vector3(
            originScale.x * direction,
            originScale.y,
            originScale.z
            );
    }
}
