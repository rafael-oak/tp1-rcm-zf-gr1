using UnityEngine;

public class GameOverTrigger : MonoBehaviour
   
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            object GameManager = null;
           

            // Son de GameOver
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.Play();
            }

            Debug.Log("💀 Game Over : un animal affamé a été dépassé !");
        }
    }
}
