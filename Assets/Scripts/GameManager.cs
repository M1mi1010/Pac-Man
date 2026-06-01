using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour //Anything that inherits from MonoBehaviour can be added as a component to a game object in the editor
{
    // I'll need variables for common references (ghosts, pacman, pellets, score and lives)

    public Ghost[] ghosts;
    public Pacman pacman;
    public int ghostMultiplier { get; private set; } = 1;

    public Transform pellets; // Transform to enable me to loop through the children in the parent
    public int score {  get; private set; } // Public getter, private setter so players can see the score but not overwritten
    public int lives { get; private set; }

    public Text scoreText;
    public int highestScore { get; private set; }
    public Text highestScoreText;
    public Text gameOver;


    private void Start()
    {
        NewGame();
    }

    private void NewGame()
    {
        // Set score and lives back to default and reset the state of objects
        SetScore(0);
        SetLives(3);
        NewRound();
        this.gameOver.text = "";
    }

    private void NewRound()
    {
        //Loop through all the pellets and turn them on:

        foreach(Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true); // When eaten the pellet will be set to false
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void Update()
    {
        if(this.lives <=0 && Input.anyKeyDown)
        {
            NewGame();
        }
        scoreText.text = score.ToString();
        if (this.score > highestScore)
        {
            highestScoreText.text = highestScore.ToString();
            highestScore = this.score + 10;
        }
    }

    private void GameOver()
    {
        //Turn off all objects
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
        this.gameOver.text = "Game   Over";
        
        highestScore = this.score;

    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
    }
    public void PacmanEaten()
    {
        // The pacman should be turned off
        this.pacman.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
/*            ResetState();
*/        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {

        //Turn off pellet and increase the score

        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        UnityEngine.Debug.Log("IS the PelletEaten function being called?");
        if(!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false); //Stop ghost from eating you
            Invoke(nameof(NewRound), 3.0f);
            UnityEngine.Debug.Log("Is the normal pellet being eaten but not destroyed?");
        }

    }

    public void PowerPelletEaten(PowerPel pellet)
    {
        //Loop through all the ghosts
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        //Change ghost state
        PelletEaten(pellet);
        CancelInvoke(); //Allows you to consume another power pellet and gain a multiplier before the duration of the previous one has ended
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        UnityEngine.Debug.Log("Is the power pellet being eaten by calling the power pellet function?");

    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf) //Are there any active game objects
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
