using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BallMovement : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10;
    [SerializeField] private float speedIncrease = 0.25f;
    [SerializeField] private Text playerScore;
    [SerializeField] private Text AIScore;
    [SerializeField] private Text printWhoWon;

    private int hitCounter;
    private Rigidbody2D rb;

    public static bool gameIsStarted = false;
    public GameObject MainMenuUI;
    
    public static bool gameIsWon = false;
    public GameObject WinnerScreenUI;
    public bool hello = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        //removes start menu
        if(Input.GetKeyDown(KeyCode.Space)){
            if(!gameIsStarted){
                StartBall();
            }
        }
        int playerScoreValue = int.Parse(playerScore.text);
        int AIScoreValue = int.Parse(AIScore.text);

        if(playerScoreValue == 5){
            printWhoWon.text = "YOU WON!";
            StopGame();
        } else if (AIScoreValue == 5){
            printWhoWon.text = "YOU LOST!";
            StopGame();
        }

    }

    private void FixedUpdate(){
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, initialSpeed + (speedIncrease * hitCounter)); //clamp magnitude makes sure the ball speed never goes passed the set max
    }

    // start of each round, initializes balls position
    private void StartBall(){
        MainMenuUI.SetActive(false);
        gameIsStarted = true;
        rb.velocity = new Vector2(-1, 0) * (initialSpeed + speedIncrease * hitCounter);
    }

    // resets ball by placing in the center of field at the start of each round
    private void ResetBall(){
        rb.velocity = new Vector2(0,0);
        transform.position = new Vector2(0,0);
        hitCounter = 0;
        Invoke("StartBall", 2f);
    }

    private void PlayerBounce(Transform myObject){
        hitCounter++;

        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position; //paddle the ball has just hit
        float xDirection, yDirection;

        if(transform.position.x > 0){
            xDirection = -1; //makes ball move the the left
        } else {
            xDirection = 1; //makes ball move to the right
        }

        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        // ensures the ball will not continuoslly move in a straight line if it hits the center of a paddle 
        if(yDirection == 0){
            yDirection = 0.25f;
        }

        rb.velocity = new Vector2(xDirection, yDirection) * (initialSpeed + (speedIncrease * hitCounter)); 
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.name == "Player" || collision.gameObject.name == "AI"){
            PlayerBounce(collision.transform);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(transform.position.x > 0){
            ResetBall();
            playerScore.text = (int.Parse(playerScore.text) + 1).ToString();
        } else if(transform.position.x < 0){
            AIScore.text = (int.Parse(AIScore.text) + 1).ToString();
            ResetBall();
        }
    }

    private void StopGame(){
        Time.timeScale = 0f;
        WinnerScreenUI.SetActive(true);
        
        if(Input.GetKeyDown(KeyCode.Space)){
            WinnerScreenUI.SetActive(false);
            Time.timeScale = 1f;
            playerScore.text = "0";
            AIScore.text = "0";
            ResetBall();
        }
    }
}
