using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Target : MonoBehaviour
{
    public delegate void OnTargetStored(Target target);
    public OnTargetStored onTargetStored;

    private Player player;

    public Rigidbody rb;

    [SerializeField]
    private int maxHP = 1;

    [SerializeField]
    private int scoreAdd = 10;

    public int currentHP;

    private void Start()
    {
        currentHP = maxHP;
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody>();
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
                onTargetStored(this);
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
            onTargetStored(this);
        }
    }
    private void OnEnable()
    {
        Invoke("StoreTarget", 4f);
    }
    private void OnDisable()
    {
        CancelInvoke("StoreTarget");
    }
    private void StoreTarget()
    {
        if(onTargetStored != null)
        {
            onTargetStored(this);
        }
    }
}