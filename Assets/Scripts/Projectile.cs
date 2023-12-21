using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// #synthese
/// Classe des projectiles.
/// Sert à détruire le projectile après un certain temps, et à le faire suivre le viseur.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _vitesse = 5f; // Vitesse du projectile
    private Rigidbody2D _rb; // Rigidbody du projectile
    Vector3 _directionViseur; // Direction du viseur
    [SerializeField] bool _estProjectileTriple;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Va chercher le Rigidbody du projectile
        _rb.AddForce(transform.right * _vitesse, ForceMode2D.Impulse); // Ajoute une force au projectile
        transform.parent = null; // Enlève le parent du projectile

        if (_estProjectileTriple) // Si le projectile est un projectile triple
        {
            StartCoroutine(CoroutinePoursuite()); // Lance la coroutine de poursuite
        }
    }

    /// <summary>
    /// Coroutine de poursuite du projectile triple.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutinePoursuite()
    {
        yield return new WaitForSeconds(0.5f);

        // Tant que le projectile existe, il va chercher l'ennemi le plus proche et va le suivre. 
        while (true)
        {
            GameObject[] tEnnemis = GameObject.FindGameObjectsWithTag("Ennemi"); // Tableau des ennemis
            GameObject ennemiPlusProche = null; // Ennemi le plus proche
            float distancePlusProche = Mathf.Infinity; // Distance de l'ennemi le plus proche
            foreach (GameObject ennemi in tEnnemis) // Pour chaque ennemi, il va chercher le plus proche et le suit
            {
                float distanceEnnemi = Vector2.Distance(transform.position, ennemi.transform.position);
                if (distanceEnnemi < distancePlusProche)
                {
                    ennemiPlusProche = ennemi;
                    distancePlusProche = distanceEnnemi;
                }
            }

            if (ennemiPlusProche != null) // Si il y a un ennemi, il le suit
            {
                Vector2 directionEnnemi = (ennemiPlusProche.transform.position - transform.position).normalized;
                float angleVersEnnemi = Mathf.Atan2(directionEnnemi.y, directionEnnemi.x) * Mathf.Rad2Deg;
                Quaternion rotationVersEnnemi = Quaternion.AngleAxis(angleVersEnnemi, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationVersEnnemi, _vitesse * Time.deltaTime);
            }
            yield return null;
        }     
    }

    /// <summary>
    /// Lorqu'un projectile entre en collision avec un autre objet, il est détruit.
    /// </summary>
    /// <param name="other">L'autre objet</param>
    void OnCollisionEnter2D(Collision2D collision) 
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Lorqu'un projectile sort du champ de la caméra, il est détruit.
    /// </summary>
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
