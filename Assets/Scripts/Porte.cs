using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp3 & #tp4
/// Classe de la porte.
/// Permet le changement de sprite de celle-ci.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class Porte : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; //données du joueur #tp3
    [SerializeField] SONavigation _nav; //données du joueur #tp3
    private SpriteRenderer _sr; //SpriteRenderer de la porte
    [SerializeField] Sprite _porteFerme; //sprite de la porte fermée
    [SerializeField] Sprite _porteOuvert; //sprite de la porte ouverte
    [SerializeField] AudioClip _sonPorte; //son de la porte #tp4
    [SerializeField] GameObject _particulePorteOuverte; 
    // [SerializeField] GameObject _lumierePorteOuverte;
    // [SerializeField] GameObject _carrePorteOuverte;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>(); // Va chercher le SpriteRenderer de la porte
    }

    /// <summary>
    /// Est appelé à chaque frame.
    /// </summary>
    void Update()
    {
        if (_donneesPerso.aLaCle) PorteOuverte(); //change le sprite de la porte en fonction de si le joueur possède la clé ou non
        else if (_donneesPerso.aLaCle == false) PorteFermer();
    }

    /// <summary>
    /// Est appelé quand un autre collider entre dans le collider de la porte.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_donneesPerso.aLaCle) //si le joueur a la clé
            {
                GestAudio.instance.GetComponent<AudioSource>().volume = 1f; //remet le volume à 100% #tp4
                GestAudio.instance.JouerEffetSonore(_sonPorte); //joue le son de la porte #tp4
                StartCoroutine(CoroutineChangerScene()); //permet de changer de scène après 2 secondes
                // Niveau.instance.goPerso.GetComponent<Rigidbody2D>().gravityScale = 0; //empêche le joueur de tomber dans le vide #tp4
            } else if (_donneesPerso.aLaCle == false) Debug.Log("Vous n'avez pas la clé");
        }
    }

    /// <summary>
    /// Change le sprite de la porte pour celui de la porte fermée.
    /// </summary>
    void PorteFermer()
    {
        _sr.sprite = _porteFerme; 
        _particulePorteOuverte.SetActive(false);
    }

    /// <summary>
    /// Change le sprite de la porte pour celui de la porte ouverte.
    /// </summary>
    void PorteOuverte()
    {
        _sr.sprite = _porteOuvert; 
        _particulePorteOuverte.SetActive(true);
    }

    /// <summary>
    /// Permet de passer à la scène suivante après 2 secondes.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineChangerScene()
    {
        yield return new WaitForSeconds(2f);
        _nav.EntrerBoutique(); // Passer à la scene de la boutique 
    }
}
