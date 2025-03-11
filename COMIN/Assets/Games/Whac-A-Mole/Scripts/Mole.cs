using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    Vector2 hidePosition;
    Vector2 showPosition = Vector2.zero;

    [SerializeField] float showHideTime = 0.5f;
    [SerializeField] float stayTime = 1f;

    [SerializeField] Sprite mole;
    [SerializeField] Sprite moleHit;
    [SerializeField] Sprite moleHat;
    [SerializeField] Sprite moleHatBroken;
    [SerializeField] Sprite moleHatHit;
    [SerializeField] Sprite moleBomb;


    [SerializeField] SpriteRenderer spriteRenderer;

    bool hittable = true;
    public enum MoleType { Standard, HardHat, Bomb };
    MoleType moleType;

    float hardRate = 0.25f;
    int lives;

    float bombRate = 0f;

    [SerializeField] BoxCollider2D boxCollider;
    Vector2 boxOffset;
    Vector2 boxSize;
    Vector2 boxOffsetHidden;
    Vector2 boxSizeHidden;

    private void Awake()
    {
        hidePosition = spriteRenderer.transform.position;
        //spriteRenderer = GetComponent<SpriteRenderer>();
        boxOffset = boxCollider.offset;
        boxSize = boxCollider.size;
        boxOffsetHidden = new Vector2(boxOffset.x, 0f);
        boxSizeHidden = new Vector2(boxSize.x, 0f);

    }
    private void Start()
    {
        SetLevel(0);
        Create();
        StartCoroutine(ShowHide(hidePosition, showPosition));
    }

    void Create()
    {
        float random = Random.Range(0f, 1f);
        if (random < bombRate)
        {
            moleType = MoleType.Bomb;
            spriteRenderer.sprite = moleBomb;
        }
        else
        {
            random = Random.Range(0f, 1f);
            if (random < hardRate)
            {
                moleType = MoleType.HardHat;
                spriteRenderer.sprite = moleHat;
                lives = 2;
            }
            else
            {
                moleType = MoleType.Standard;
                spriteRenderer.sprite = mole;
                lives = 1;
            }
        }

        hittable = true;
    }
    IEnumerator ShowHide(Vector2 start, Vector2 end)
    {
        spriteRenderer.transform.localPosition = start;
        float elapsedTime = 0f;
        while (elapsedTime < showHideTime)
        {
            spriteRenderer.transform.localPosition = Vector2.Lerp(start, end, elapsedTime / showHideTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.transform.localPosition = end;
        yield return new WaitForSeconds(stayTime);

        elapsedTime = 0f;
        while (elapsedTime < showHideTime)
        {
            spriteRenderer.transform.localPosition = Vector2.Lerp(end, start, elapsedTime / showHideTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.transform.localPosition = start;
    }

    private void OnMouseDown()
    {
        if (hittable)
        {
            switch (moleType)
            {
                case MoleType.Standard:
                    spriteRenderer.sprite = moleHit;
                    StopAllCoroutines();
                    StartCoroutine(QuickHide());
                    hittable = false;
                    break;
                case MoleType.HardHat:
                    if (lives == 2)
                    {
                        spriteRenderer.sprite = moleHatBroken;
                        lives--;
                    }
                    else
                    {
                        spriteRenderer.sprite = moleHatHit;
                        StopAllCoroutines();
                        StartCoroutine(QuickHide());
                        hittable = false;
                    }
                    break;
                case MoleType.Bomb:
                    break;
            }


        }
    }

    IEnumerator QuickHide()
    {
        yield return new WaitForSeconds(0.25f);
        if (!hittable)
        {
            Hide();
        }
    }

    void Hide()
    {
        spriteRenderer.transform.localPosition = hidePosition;
        boxCollider.offset = boxOffsetHidden;
        boxCollider.size = boxSizeHidden;
    }

    void SetLevel(int level)
    {
        //En el nivel 10 empieza a siempre 0.25
        bombRate = Mathf.Min(level * 0.025f, 0.25f);
        //va inclementando los topos con casco hasta llegar al 100%
        hardRate = Mathf.Min(level * 0.025f, 1f);
        //El tiempo de estar fuera lo vamos aumentando poco a poco
        //Nivel 15 1 -(15*0.1f) = 1-1.5 => 0.01f
        float stayMin = Mathf.Clamp(1 - level * 0.1f, 0.01f, 1f);
        //Nivel 15 2 -(15*0.1f) = 2- 1.5 => 0.5
        float stayMax = Mathf.Clamp(2 - level * 0.1f, 0.01f, 2f);
        stayTime = Random.Range(stayMin, stayMax);

    }



}
