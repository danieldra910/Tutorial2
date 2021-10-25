using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    public AudioSource musicSource;



   private Rigidbody2D rd2d;
   public float speed;
   public Text score;
   public Text winLose;
   public Text lives;
   private int livesValue;

   private int scoreValue;
   
    private bool facingRight=true;

   private bool isOnGround;
   public Transform groundcheck;
   public float checkRadius;
   public LayerMask allGround;

   Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim=GetComponent<Animator>();

        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop=true;

        winLose.text="";
        scoreValue=0;
        livesValue=3;

        rd2d=GetComponent<Rigidbody2D>();

        score.text="Score: "+scoreValue.ToString();
        lives.text="Lives: "+livesValue.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        isOnGround=Physics2D.OverlapCircle(groundcheck.position, checkRadius,allGround);
        rd2d.AddForce(new Vector2(hozMovement*speed,verMovement*speed));

        if(facingRight==false&&hozMovement>0)
        {
            Flip();
        }
        else if(facingRight ==true&&hozMovement<0)
        {
            Flip();
        }

        if(verMovement >0)
        {
            anim.SetInteger("State",2);
        }
        

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    void Flip()
    {
        facingRight=!facingRight;
        Vector2 Scaler=transform.localScale;
        Scaler.x = Scaler.x*-1;
        transform.localScale=Scaler;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag=="Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0,4),ForceMode2D.Impulse);
            }
            anim.SetInteger("State",0);
            if(Input.GetKey(KeyCode.D)||Input.GetKey(KeyCode.A))
            {
                anim.SetInteger("State",1);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag=="Coin")
        {
            scoreValue+=1;
            score.text="Score: "+scoreValue.ToString();
            Destroy(collision.collider.gameObject);

            if(scoreValue==4)
            {
                transform.position = new Vector2(55.0f, 0.0f); 
                livesValue=3;
                lives.text="Lives: "+livesValue.ToString();
            }
            win();
        }
        if(collision.collider.tag=="enemy")
        {
            livesValue-=1;
            lives.text="Lives: "+livesValue.ToString();
            Destroy(collision.collider.gameObject);
            lose();
        }
    }

    private void win()
    {
        if(scoreValue==8)
        {
            musicSource.Stop();
            musicSource.loop=false;
            musicSource.clip=musicClipTwo;
            musicSource.Play();

            winLose.text="You Win Game made by Daniel Arias";
            speed=0;
        }
    }
    private void lose()
    {
        if(livesValue<=0)
        {
            anim.SetInteger("State",3);
            musicSource.Stop();
            musicSource.clip=musicClipThree;
            musicSource.Play();
            winLose.text="You Lose game made by Daniel Arias";
            speed=0;
        }
    }

    
}
