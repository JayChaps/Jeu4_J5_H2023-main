using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe de l'ame.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class Ame : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; //données du joueur #tp3
    [SerializeField] AudioClip _sonAme; //son de l'ame #tp4
    [SerializeField] GameObject _parent; //parent de l'ame #synthese
    [SerializeField] GameObject _prefabPlusUn; //prefab du +1 #synthese

    /// <summary>
    /// Est appelé lorsque le collider entre en collision avec un autre collider.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _donneesPerso.argent++; //ajoute 1 à l'argent du joueur #tp3
            GestAudio.instance.GetComponent<AudioSource>().volume = 0.2f; 
            GestAudio.instance.JouerEffetSonore(_sonAme); //joue le son de l'ame #tp4
            Instantiate(_prefabPlusUn, transform.position, Quaternion.identity, transform.parent); //crée le +1 #tp3
            Destroy(_parent.gameObject); //détruit l'ame #tp3
        }
    }
}