using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private bool isAI;
    [SerializeField] private GameObject ball; //allows AI to move with the ball 

    private Rigidbody2D rb;
    private Vector2 playerMove;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //allows players to exit the game
        if (Input.GetKey("escape"))
        {
            EscapeGame();
        }

        if(isAI){
            AIControl();
        } else {
            PlayerControl();
        }

    }

    private void EscapeGame(){
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void PlayerControl(){
        playerMove = new Vector2(0, Input.GetAxisRaw("Vertical"));
    }

    private void AIControl(){
        if(ball.transform.position.y > transform.position.y + 0.75f){ //0.75 for so AI moves like a real player
            playerMove = new Vector2(0,1);
        } else if(ball.transform.position.y < transform.position.y - 0.75f){
            playerMove = new Vector2(0,-1);
        } else {
            playerMove = new Vector2(0,0);
        }
    }

    private void FixedUpdate(){ 
        //this allows for the movement to stay consistant regardless of the players FPS
        rb.velocity = playerMove * movementSpeed;
    }
}
