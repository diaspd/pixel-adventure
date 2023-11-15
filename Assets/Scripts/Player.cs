using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// class of definition of an object
public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float speed;
    public float jumpForce;
    public int life;
    public int cherry;

    [Header("Components")]
    public Rigidbody2D rig;
    public Animator anim;
    public SpriteRenderer sprite;

    [Header("UI")]
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI lifeText;
    public GameObject gameOver;

    private Vector2 direction;

    private bool isGrounded;
    private bool recovery;

    // Start is called before the first frame update
    void Start()
    {
        lifeText.text = life.ToString();
        Time.timeScale = 1;

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        Jump();
        PlayAnimations();
    }

    void FixedUpdate()
    {
        Movement();
    }
 
    void Movement()
    {
        rig.velocity = new Vector2(direction.x * speed, rig.velocity.y);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded == true)
        {
            anim.SetInteger("transition", 2);
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void Death()
    {
        if (life <= 0)
        {
            gameOver.SetActive(true);
            Time.timeScale = 0;
        }
    }

    void PlayAnimations()
    {
        if(direction.x > 0 && isGrounded)
        {
            if(isGrounded)
            {
              anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = Vector2.zero;
        }

        if(direction.x < 0)
        {
            if(isGrounded)
            {
              anim.SetInteger("transition", 1);
            }
            transform.eulerAngles = new Vector2(0, 180);
        }

        if (direction.x == 0 && isGrounded)
        {
            anim.SetInteger("transition", 0);
        }
    }

    public void Hit()
    {   
        if(recovery == false)
        {
          StartCoroutine(Flick());   
        }
    }

    IEnumerator Flick()
    {
        recovery = true;
        
        life--;
        Death();
        lifeText.text = life.ToString();

        sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.2f);
        sprite.color = new Color(1, 1, 1, 1);

        recovery = false;
    }

    public void IcreaseScore()
    {
        cherry++;
        cherryText.text = cherry.ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isGrounded = true;
        }
    }
}
