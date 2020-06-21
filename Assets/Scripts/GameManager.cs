using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    const int xPlayer = 1;
    const int oPlayer = 2;

    public int[] gameState;

    int currentPlayer = xPlayer;
    int xMoveCount = 0;
    int oMoveCount = 0;

    public Button[] tiles = new Button[9];

    public GameObject winScreen;


    // Start is called before the first frame update
    void Start()
    {
        gameState = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Menu logic

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // In-game logic
    public void PlayTile(int tile)
    {
        // Checkeamos que ya no hayan usado ese spot
        if (gameState[tile] != 0)
        {
            return;
        }
        if (currentPlayer == xPlayer)
        {
            xMoveCount++;
        }
        else
        {
            oMoveCount++;
        }
        gameState[tile] = currentPlayer;
        tiles[tile].GetComponentInChildren<Text>().text = currentPlayer == xPlayer ? "X" : "O";
        bool won = PlayerWon(tile);
        if (!won)
        {
            currentPlayer = currentPlayer == xPlayer ? oPlayer : xPlayer;
            if (xMoveCount + oMoveCount == 9)
            {
                winScreen.GetComponentInChildren<Text>().text = "Draw!";
                winScreen.SetActive(true);
            }
        }
        else
        {
            winScreen.GetComponentInChildren<Text>().text = "Player " + (currentPlayer == xPlayer ? "X" : "O") + " wins!";
            winScreen.SetActive(true);
        }
    }

    public bool PlayerWon(int tile)
    {
        // Checkeamos que hayan suficientes movimientos para ganar
        if (currentPlayer == xPlayer && xMoveCount < 3)
        {
            return false;
        }

        // Checkeamos que hayan suficientes movimientos para ganar
        if (currentPlayer == oPlayer && oMoveCount < 3)
        {
            return false;
        }

        if ( // Checkeamos si gano por columna
            gameState[tile] == gameState[(tile + 3) % 9] &&
            gameState[tile] == gameState[(tile + 6) % 9]
        )
        {
            return true;
        }

        int row = tile < 3 ? 0 : (tile < 6 ? 1 : 2);
        if ( // Checkeamos si gano por fila
            gameState[row * 3] == gameState[(row * 3) + 1] &&
            gameState[row * 3] == gameState[(row * 3) + 2]
        )
        {
            return true;
        }

        if ( // Checkeamos las diagonales/esquinas
            ((tile % 2) == 0 && tile != 4) &&
            gameState[tile] == gameState[4] && gameState[tile] == gameState[8 - tile]
        )
        {
            return true;
        }
        return false;
    }

    public void ResetGame()
    {
        gameState = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < 9; i++)
        {
            tiles[i].GetComponentInChildren<Text>().text = "";
        }
        currentPlayer = xPlayer;
        xMoveCount = 0;
        oMoveCount = 0;
        if (winScreen.activeSelf)
        {
            winScreen.SetActive(false);
        }
    }
}
