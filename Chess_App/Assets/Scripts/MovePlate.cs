using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;
    public GameObject Reference { get => reference; set => reference = value; }

    int matrixX;
    int matrixY;

    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            fight(reference, cp);
            
            if (cp.GetComponent<Chessman>().CurrentHealth <= 0)
            {
                if (cp.name == "white_king") controller.GetComponent<Game>().Winner("black");
                else if (cp.name == "black_king") controller.GetComponent<Game>().Winner("white");

                Destroy(cp);

                controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().XBoard, reference.GetComponent<Chessman>().YBoard);

                reference.GetComponent<Chessman>().XBoard = matrixX;
                reference.GetComponent<Chessman>().YBoard = matrixY;
                reference.GetComponent<Chessman>().SetCoords();

                controller.GetComponent<Game>().SetPosition(reference);
            }
        }
        else
        {
            controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<Chessman>().XBoard, reference.GetComponent<Chessman>().YBoard);

            reference.GetComponent<Chessman>().XBoard = matrixX;
            reference.GetComponent<Chessman>().YBoard = matrixY;
            reference.GetComponent<Chessman>().SetCoords();

            controller.GetComponent<Game>().SetPosition(reference);
        }
        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<Chessman>().DestroyMovePlates();
    }
    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }
    public void fight(GameObject attackingPiece, GameObject damagedPiece)
    {
        damagedPiece.GetComponent<Chessman>().CurrentHealth -= attackingPiece.GetComponent<Chessman>().Damage;
        int currentHealth = damagedPiece.GetComponent<Chessman>().CurrentHealth;
        int maxHealth = damagedPiece.GetComponent<Chessman>().MaxHealth;
        damagedPiece.GetComponent<Chessman>().healthBar.GetComponent<HealthBar>().setHealth(currentHealth,maxHealth);
    }

}
