using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe des colliders pour les murs.
/// Utilisé pour les colliders du perso.
/// Réutilisable pour des ennemis à l'avenir.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class CollidersWJ : MonoBehaviour
{
    private bool _touche = false; // booléen qui indique si le collider touche un mur

    public bool touche { get => _touche; set => _touche = value; } // accesseur du booléen

    /// <summary>
    /// Sent each frame where another object is within a trigger collider
    /// attached to this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tilemap")
        {
            touche = true; // si le collider touche un mur, le booléen est vrai
        }      
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit2D(Collider2D other)
    {
        touche = false; // si le collider ne touche plus un mur, le booléen est faux
    }
}
