using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;
    public int XBoard { get => xBoard; set => xBoard = value; }
    public int YBoard { get => yBoard; set => yBoard = value; }

    private string player;

    private int maxHealth;
    public HealthBar healthBar;
    private int currentHealth;
    private int damage;
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    public int Damage { get => damage; set => damage = value; }
    public int MaxHealth { get => maxHealth; set => maxHealth = value; }

    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;


    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; startHealthBar(3,2); break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; startHealthBar(3,3); break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; startHealthBar(3,2); break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; startHealthBar(4,3); break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; startHealthBar(2,2); break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; startHealthBar(1,1); break;

            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; startHealthBar(3,2); break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; startHealthBar(3,3); break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; startHealthBar(3,2); break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; startHealthBar(4,3); break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; startHealthBar(2,2); break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; startHealthBar(1,1); break;
        }
    }

    public void startHealthBar(int max, int damage)
    {
        maxHealth = max;
        currentHealth = maxHealth;
        healthBar.setHealth(max, max);

        this.damage = damage;
    }
    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);

    }
    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }
    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                LineMovePlate(-1, 0);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;

            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;

            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;

            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;

            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                LineMovePlate(-1, 0);
                break;

            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                break;

            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                break;

        }
    }
    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() && controller.GetComponent<Game>().CurrentPlayer == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }
    }
    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }
        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x,y);
        }
    }
    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }
    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
    }
    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if(sc.GetPosition(x,y) == null)
            {
                MovePlateSpawn(x, y);
            }
            if(sc.PositionOnBoard(x +1,y) && sc.GetPosition(x+1, y) != null && sc.GetPosition(x+1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }
            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }
    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f),Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.Reference = gameObject;
        mpScript.SetCoords(matrixX, matrixY);
    }
    private void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.Reference = gameObject;
        mpScript.SetCoords(matrixX, matrixY);
    }
    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);
            if(cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if(cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x,y);
            }
        }
    }
}
