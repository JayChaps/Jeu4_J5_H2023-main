using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Classe de l'activateur.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class Activateur : MonoBehaviour
{
    [SerializeField] SOPerso _donnees; // Données du joueur
    private Animator _anim; // Animator de l'activateur


    /// <summary>
    /// Est appelé au début de l'exécution du script.
    /// </summary>
    void Start()
    {
        _anim = GetComponent<Animator>(); // Récupère l'animator de l'activateur
    }

    /// <summary>
    /// Est appelé à chaque frame.
    /// </summary>
    void Update()
    {
        if (_donnees.aActiverBonus) _anim.SetBool("estActif", true);
        else if (_donnees.aActiverBonus == false) _anim.SetBool("estActif", false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Perso>())
        {
            _donnees.activerLesBonus.Invoke();
        }
    }
}