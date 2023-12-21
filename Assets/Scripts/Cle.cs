using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de la clé.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Cle : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; //données du joueur #tp3
    [SerializeField] AudioClip _sonCle; //son de la clé #tp4
    [SerializeField] GameObject _parentCle; //parent de la clé #synthese

    /// <summary>
    /// Est appelé quand un autre collider entre dans le collider de la clé.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _donneesPerso.aLaCle = true;
            GestAudio.instance.GetComponent<AudioSource>().volume = 1f; 
            GestAudio.instance.JouerEffetSonore(_sonCle); //joue le son de la clé #tp4
            GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenA, true); 
            Destroy(_parentCle); //détruit le parent de la clé #synthese
        }
    }
}
