using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] AudioClip _powerupClip;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private int _powerupID; // 0 = triple shot | 1 = speed | 2 = shields | 3 = ammo | 4 = health

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerupClip, transform.position);

            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.ActivateTripleShot();
                        break;
                    case 1:
                        player.ActivateSpeedBoost();
                        break;
                    case 2:
                        player.ActivateShields();
                        break;
                    case 3:
                        player.ReplenishAmmo();
                        break;
                    case 4:
                        player.ReplenishHealth();
                        break;
                    case 5:
                        player.ActivateWideSweep();
                        break;
                    case 6:
                        player.DeactivateShields();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;
                }
            }
            Destroy(gameObject);
        }
        else if(other.tag == "Laser")
        {
            Destroy(this.gameObject);
        }
    }
}
