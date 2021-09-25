using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    private Player player;

    private const float TIME_TO_DESTROY = 10F;

    [SerializeField]
    private int maxHP = 1;

    [SerializeField]
    private int scoreAdd = 10;

    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        Destroy(gameObject, TIME_TO_DESTROY);
        player = FindObjectOfType<Player>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        int collidedObjectLayer = collision.gameObject.layer;

        if (collidedObjectLayer.Equals(Utils.BulletLayer))
        {
            player.onBulletStored(collision.rigidbody);

            currentHP -= 1;

            if (currentHP <= 0)
            {               
                if (player != null && player.onPlayerScoreChanged != null)
                {
                    player.onPlayerScoreChanged(scoreAdd);       
                }
                Destroy(gameObject);
            }
        }
        else if (collidedObjectLayer.Equals(Utils.PlayerLayer) ||
            collidedObjectLayer.Equals(Utils.KillVolumeLayer))
        {

            if (player != null)
            {
                if(player.OnPlayerHit != null)
                {
                    player.OnPlayerHit();
                }                              
            }
            Destroy(gameObject);
        }
    }
}