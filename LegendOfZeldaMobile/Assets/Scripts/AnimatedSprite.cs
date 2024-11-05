using System.Collections;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    private Sprite[] sprites;
    public Sprite[] idleLeft;
    public Sprite[] idleRight;
    public Sprite[] runningLeft;
    public Sprite[] runningRight;
    public float framerate = 6f;
    private float frametime => 1f / framerate;
    public float updateInterval = 0.1f;

    public int horizontal = -1;
    public bool running = false;
    public bool singleCycle; // USES IDLE LEFT FROM INSPECTOR
    private int horizontalBuffer;
    private bool runningBuffer;

    private SpriteRenderer spriteRenderer;
    private Coroutine animationCoroutine;
    private Coroutine updateCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (singleCycle){
            sprites = idleLeft;
        } else {
            updateCoroutine = StartCoroutine(UpdateStates());
        }
        animationCoroutine = StartCoroutine(Animate());
    }

    private void OnDisable()
    {
        if (updateCoroutine != null)
        {
            StopCoroutine(updateCoroutine);
            updateCoroutine = null;
        }

        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private int frame;
    private IEnumerator Animate()
    {
        while (true)
        {
            if (sprites.Length > 0){
                if (frame >= sprites.Length){
                    frame = 0;
                }

                if (spriteRenderer.sprite != sprites[frame]){
                    spriteRenderer.sprite = sprites[frame];
                }

                frame++;
            }

            yield return new WaitForSeconds(frametime);
        }
    }

    private IEnumerator UpdateStates()
    {
        while (true)
        {
            if (runningBuffer != running || horizontalBuffer != horizontal){
                if (horizontal == -1){
                    sprites = running ? runningLeft : idleLeft;
                } else if (horizontal == 1) {
                    sprites = running ? runningRight : idleRight;
                }
                horizontalBuffer = horizontal;
                runningBuffer = running;
                frame = 0;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
