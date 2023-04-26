using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] Vector3 _amountToShake;

    public void ShakeCamera()
    {
        StartCoroutine(ShakeCameraRoutine());
    }

    private IEnumerator ShakeCameraRoutine()
    {
        transform.rotation = Quaternion.Euler(_amountToShake);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(-_amountToShake);
        yield return new WaitForSeconds(0.1f);
        transform.rotation = Quaternion.Euler(Vector3.zero);

    }
}
