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

            if (collision.transform.localScale.x > 0.3f) // tamaño mínimo
            {
                float angle = 30f; // ángulo fijo de separación

                for (int i = -1; i <= 1; i += 2) // dos fragmentos
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
                        rb.useGravity = false; // 🚫 sin gravedad

                        // Vector bisectriz (dirección de la bala)
                        Vector3 dir = Quaternion.AngleAxis(i * angle, Vector3.forward) * targetVector;

                        // Asignamos velocidad suave en vez de un impulso loco
                        rb.linearVelocity = dir.normalized * 2f; // <-- prueba con 1.5f o 2f
                    }

                    Destroy(fragment, 8f); // desaparecen a los 8 segundos
                }
            }

            Destroy(collision.gameObject); // destruir asteroide original
            Destroy(gameObject);           // destruir la bala
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
