using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe du bonus.
/// Permet le changement de sprite de celui-ci.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// #tp3
/// </summary>
public class Bonus : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // Données du joueur
    private SpriteRenderer _sr; // SpriteRenderer du bonus
    [SerializeField] Sprite _spBonusOff; // Sprite du bonus désactivé
    [SerializeField] Sprite _spBonusOn; // Sprite du bonus activé
    [SerializeField] AudioClip _sonBonus; // Son du bonus
    [SerializeField] GameObject _lumiereBonus; // Lumière du bonus
    [SerializeField] GameObject _particulesBonus; // Lumière du bonus

    /// <summary>
    /// Est appelé au début de l'exécution du script.
    /// </summary>
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>(); // Va chercher le SpriteRenderer du bonus
        _donneesPerso.activerLesBonus.AddListener(ActivationBonus);
    }

    /// <summary>
    /// Est appelé à chaque frame.
    /// </summary>
    void Update()
    {
        if (_donneesPerso.aActiverBonus) {_sr.sprite = _spBonusOn; _lumiereBonus.SetActive(true); _particulesBonus.SetActive(true);}
        else if (_donneesPerso.aActiverBonus == false) {_sr.sprite = _spBonusOff; _lumiereBonus.SetActive(false); _particulesBonus.SetActive(false);}
    }

    /// <summary>
    /// Active les bonus.
    /// </summary>
    void ActivationBonus()
    {
        _donneesPerso.aActiverBonus = true;
    }

    /// <summary>
    /// Est appelé quand un autre collider entre dans le collider du bonus.
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (_donneesPerso.aActiverBonus)
            {
                _donneesPerso.bonus++;
                GestAudio.instance.GetComponent<AudioSource>().volume = 1f;
                GestAudio.instance.JouerEffetSonore(_sonBonus); // Joue le son du bonus #tp4
                Destroy(gameObject); // Détruit le bonus
            }
        }
    }
}
