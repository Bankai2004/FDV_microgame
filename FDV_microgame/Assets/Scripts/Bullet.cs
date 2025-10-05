using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Bullet : MonoBehaviour
{

    public float speed = 10f;
    public float maxLifeTime = 3f;
    public Vector3 targetVector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, maxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed * targetVector * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            IncreaseScore();

            if (collision.transform.localScale.x > 0.3f)
            {
                float angle = 30f; 

                for (int i = -1; i <= 1; i += 2) 
                {
                    GameObject fragment = Instantiate(
                        collision.gameObject,
                        collision.transform.position,
                        Quaternion.identity
                    );

                    fragment.transform.localScale = collision.transform.localScale * 0.5f;

                    Rigidbody rb = fragment.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.useGravity = false; 

                        // Vector bisectriz (dirección de la bala)
                        Vector3 dir = Quaternion.AngleAxis(i * angle, Vector3.forward) * targetVector;

                        // Asignamos velocidad suave en vez de un impulso loco
                        rb.linearVelocity = dir.normalized * 2f; 
                    }

                    Destroy(fragment, 8f); 
                }
            }

            Destroy(collision.gameObject); 
            Destroy(gameObject);           
        }
    }


    private void IncreaseScore()
    {
        Player.SCORE++;
        Debug.Log(Player.SCORE);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<Text>().text = "Puntos: " + Player.SCORE;
    }
}
