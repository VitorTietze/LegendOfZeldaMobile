using System.Collections;
using UnityEngine;

public class AnimatedSpritePlus : MonoBehaviour
{
    private Sprite[] sprites;

    public Sprite[] idleLeft;
    public Sprite[] idleRight;
    public Sprite[] idleDown;
    public Sprite[] idleUp;
    public float idleFramerate = 6f;
    private float idleFrametime => 1f / idleFramerate;

    public Sprite[] runningLeft;
    public Sprite[] runningRight;
    public Sprite[] runningDown;
    public Sprite[] runningUp;
    public float runFramerate = 6f;
    private float runFrametime => 1f / runFramerate;

    public Sprite[] attackingLeft;
    public Sprite[] attackingRight;
    public Sprite[] attackingDown;
    public Sprite[] attackingUp;
    public float attackFramerate = 6f;
    private float attackFrametime => 1f / attackFramerate;

    public float updateInterval = 0.1f;

    /* [HideInInspector]  */public int horizontal = 0;
    /* [HideInInspector]  */public int vertical = 1;
    /* [HideInInspector]  */public bool running = false;
    /* [HideInInspector]  */public bool attacking = false;

    private int horizontalBuffer;
    private int verticalBuffer;
    private bool runningBuffer;
    private bool attackingBuffer;

    private SpriteRenderer spriteRenderer;
    private Coroutine animationCoroutine;
    private Coroutine updateCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        updateCoroutine = StartCoroutine(UpdateStates());
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
    private float currentFrametime;
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

            if (running){
                currentFrametime = runFrametime;
            } else if (attacking){
                currentFrametime = attackFrametime;
            } else {
                currentFrametime = idleFrametime;
            }

            yield return new WaitForSeconds(currentFrametime);
        }
    }

    private IEnumerator UpdateStates()
    {
        while (true)
        {
            if (runningBuffer != running || horizontalBuffer != horizontal ||
                verticalBuffer != vertical || attackingBuffer != attacking)
            {
                if (attacking){
                    if (vertical == 1){
                        sprites = attackingUp;
                    } else if (vertical == -1){
                        sprites = attackingDown;
                    } else if (horizontal == -1){
                        sprites = attackingLeft;
                    } else if (horizontal == 1){
                        sprites = attackingRight;
                    }
                } else {
                    if (vertical == 1){
                        sprites = running ? runningUp : idleUp;
                    } else if (vertical == -1){
                        sprites = running ? runningDown : idleDown;
                    } else if (horizontal == -1){
                        sprites = running ? runningLeft : idleLeft;
                    } else if (horizontal == 1){
                        sprites = running ? runningRight : idleRight;
                    }
                }

                horizontalBuffer = horizontal;
                verticalBuffer = vertical;
                runningBuffer = running;
                attackingBuffer = attacking;
                frame = 0;
            }

            yield return new WaitForSeconds(updateInterval);
        }
    }
}
