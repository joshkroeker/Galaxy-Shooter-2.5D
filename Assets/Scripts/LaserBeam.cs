using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.Damage();
        }
    }
}
