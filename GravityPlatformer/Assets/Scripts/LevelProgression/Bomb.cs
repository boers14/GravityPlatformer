using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private Sprite whiteBombSprite = null;

    private Sprite regularSprite = null;

    private SpriteRenderer spriteRenderer = null;

    [SerializeField]
    private float explodeTimer = 3f, explosionRange = 1, swapSpriteFactor = 4;

    private float swapBombSpriteTimer = 0, particleTimer = 0;

    private bool canExplode = false, startExploding = false, exploded = false;

    [SerializeField]
    private LayerMask hitLayerMask = 0;

    private new ParticleSystem particleSystem = null;

    [System.NonSerialized]
    public BombSpawner bombSpawner = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        regularSprite = spriteRenderer.sprite;
        swapBombSpriteTimer = explodeTimer / swapSpriteFactor;
        particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (!canExplode) { return; }

        if (Input.GetKeyUp(KeyCode.X))
        {
            startExploding = true;
        }

        if (startExploding)
        {
            explodeTimer -= Time.deltaTime;
            swapBombSpriteTimer -= Time.deltaTime;

            if (swapBombSpriteTimer <= 0)
            {
                swapBombSpriteTimer = explodeTimer / swapSpriteFactor;
                if (spriteRenderer.sprite == regularSprite)
                {
                    spriteRenderer.sprite = whiteBombSprite;
                } else
                {
                    spriteRenderer.sprite = regularSprite;
                }
            }

            if (explodeTimer <= 0 && !exploded)
            {
                Collider2D[] hitByExplosion = Physics2D.OverlapCircleAll(transform.position, explosionRange, hitLayerMask);
                foreach(Collider2D hit in hitByExplosion)
                {
                    Destroy(hit.gameObject);
                }
                spriteRenderer.enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                tag = "Untagged";
                exploded = true;

                ParticleSystem.MinMaxCurve emission = particleSystem.emission.GetBurst(0).count;
                ParticleSystem.ShapeModule shape = particleSystem.shape;
                emission.constant *= explosionRange;
                shape.scale = new Vector3(explosionRange, explosionRange, 1);

                particleTimer = particleSystem.main.startLifetime.constant;

                particleSystem.Play();
            }

            if (exploded)
            {
                particleTimer -= Time.deltaTime;

                if (particleTimer <= 0)
                {
                    bombSpawner.SpawnBomb();
                    Destroy(gameObject);
                }
            }
        }
    }

    public void SetToCanExplode()
    {
        canExplode = true;
        if (!Input.GetKey(KeyCode.X))
        {
            startExploding = true;
        }
    }
}
