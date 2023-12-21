using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese
/// Classe du gameobject de trail qui suit le joueur.
/// Sert à afficher le bon sprite lors de l'instanciation, et à détruire le gameobject.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Trail : MonoBehaviour
{
    private SpriteRenderer _sr; // sprite renderer du gameobject
    private GameObject _perso; // gameobject du personnage

    /// <summary>
    /// Awake est appelé lorsque le script est chargé.
    /// </summary>
    void Awake()
    {
        _perso = Niveau.instance.goPerso; // récupère le gameobject du personnage
        _sr = GetComponent<SpriteRenderer>(); // récupère le sprite renderer du gameobject
        _sr.sprite = _perso.GetComponent<SpriteRenderer>().sprite; // change le sprite du gameobject pour celui du personnage
        transform.localScale = _perso.transform.localScale; // change la taille du gameobject pour celle du personnage
        transform.parent = null; // enlève le parent du gameobject
    }

    /// <summary>
    /// Détruit le gameobject.
    /// Est appelé par l'animation de fin de vie du gameobject.
    /// </summary>
    public void Detruire()
    {
        Destroy(gameObject);
    }
}
