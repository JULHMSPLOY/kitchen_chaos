using UnityEngine;

public class PlayerWalkingParticles : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem particles;

    private void Update()
    {
        if (player.IsWalking())
        {
            if (!particles.isPlaying)
            {
                particles.Play();
            }
        }
        else
        {
            if (particles.isPlaying)
            {
                particles.Stop();
            }
        }
    }
}
