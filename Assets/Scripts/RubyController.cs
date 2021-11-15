using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    public int maxHealth = 5;

    public GameObject projectilePrefab;

    public ParticleSystem healthEffect;
    public ParticleSystem hitEffect;

    public AudioClip throwSound;
    public AudioClip hitSound;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public TextMeshProUGUI countText;
    private int count;

    public TextMeshProUGUI scoreText;
    private int score;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);

    AudioSource audioSource;

    bool gameOver;

    public GameObject winStage1TextObject;
    public GameObject loseTextObject;
    public GameObject welcomeTextObject;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    public AudioSource musicSource;
    public AudioSource musicSource2;
    public AudioSource musicSource3;

    public Transform Target;

    // Start is called before the first frame update
    void Start()
    {
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
            musicSource.loop = true;
        }

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        healthEffect.Stop();
        hitEffect.Stop();

        score = 0;
        count = 4;
        countText.text = "Cogs: " + count.ToString();
        scoreText.text = "Robots Fixed: " + score.ToString() + "/6";

        winStage1TextObject.SetActive(false);
        loseTextObject.SetActive(false);
        welcomeTextObject.SetActive(true);

        gameOver = false;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
           
            if(count > 0)
            {
                Launch();
                count = count - 1;
                SetCountText();
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();

                if(gameOver == true)
                {
                    SceneManager.LoadScene("Scene2");
                }

                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            winStage1TextObject.SetActive(false);
            welcomeTextObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }
    }

        void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitSound);
            hitEffect.Play();

        }

        if (amount > 0)
        {
            healthEffect.Play();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            loseTextObject.SetActive(true);
            speed = 0;
            gameOver = true;
            transform.position = Target.position;
            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource3.clip = musicClipThree;
            musicSource3.Play();
        }

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);

    }

    public void ChangeScore(int amount)
    {
        score = score + amount;
        SetScoreText();

        if (score == 6)
        {
            winStage1TextObject.SetActive(true);
            gameOver = true;
            musicSource.clip = musicClipOne;
            musicSource.Stop();
            musicSource2.clip = musicClipTwo;
            musicSource2.Play();
            musicSource2.loop = true;
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");

        PlaySound(throwSound);
       
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ammo")
        {
            count = count + 4;
            SetCountText();
            Destroy(collision.collider.gameObject);
        }
    }


    void SetCountText()
    {
        countText.text = "Cogs: " + count.ToString();
    }

    void SetScoreText()
    {
        scoreText.text = "Robots Fixed: " + score.ToString() + "/6";
        /*
        if (lives == 0)
        {
            // display lose text
            loseTextObject.SetActive(true);
        }
        */

    }
}
