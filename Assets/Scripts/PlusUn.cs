using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese
/// Classe du petit +1 qui apparait lorsqu'un joueur ramasse un joyaux.
/// Sert à détruire le gameobject.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class PlusUn : MonoBehaviour
{
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        transform.parent = null; // enlève le parent du gameobject
    }

    /// <summary>
    /// Détruit le gameobject.
    /// Est appelé par l'animation de fin de vie du gameobject.
    /// </summary>
    void Detruire()
    {
        Destroy(gameObject);
    }
}
