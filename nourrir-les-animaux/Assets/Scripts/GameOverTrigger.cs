using UnityEngine;

public class GameOverTrigger : MonoBehaviour
   
{
    public bool gameOver = false;

    Animator playeranim;

    public AudioClip endGame;

    public AudioSource cameraAudioSource;

    private void Start()
    {
        playeranim = GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            // object GameManager = null;
            gameOver = true;

            gererGameOver();

            Debug.Log("💀 Game Over : un animal affamé a été touché!");
        }
    }



    public void gererGameOver()
    {
        playeranim.SetBool("Death_b", true);
        playeranim.SetInteger("DeathType_int", 1);

        // Son de GameOver
        if (cameraAudioSource != null && endGame != null)
        {
            cameraAudioSource.Stop(); // Coupe la musique en cours
            cameraAudioSource.clip = endGame;
            cameraAudioSource.Play(); // Joue le son de fin
        }
        else
        {
            Debug.LogWarning("🎵 AudioSource ou AudioClip manquant !");
        }
    }
}
