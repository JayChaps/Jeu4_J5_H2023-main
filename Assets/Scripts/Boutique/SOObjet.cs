using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objet", menuName = "TIM/Objet boutique")]

/// <summary>
/// ScriptableObject des objets de la boutique.
/// Contient les données en lien avec les objets.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput 
/// #tp3
/// </summary>
public class SOObjet : ScriptableObject
{
    [Header("LES DONNÉES")]
    [SerializeField] string _nom = "Sandale de course"; //nom de l'objet
    [SerializeField][Tooltip("Image de l'icône à afficher")] Sprite _sprite; //sprite de l'objet
    [SerializeField][Range(0, 200)] int _prix = 30; //prix de l'objet
    [SerializeField][TextArea] string _description; //description de l'objet
    [SerializeField][Tooltip("Permet de courir?")] bool _permetDeCourir = false; //détermine si l'objet permet de courir ou non
    [SerializeField][Tooltip("Augmente les dégats?")] bool _augmenteLesDegats = false; //détermine si l'objet augmente les dégats ou non

    //setters/getters du nom, sprite, prix, description et bools
    public string nom { get => _nom; set => _nom = value; }
    public Sprite sprite { get => _sprite; set => _sprite = value; }
    public int prix { get => _prix; set => _prix = value; }
    public string description { get => _description; set => _description = value; }
    public bool permetDeCourir { get => _permetDeCourir; set => _permetDeCourir = value; }
    public bool augmenteLesDegats { get => _augmenteLesDegats; set => _augmenteLesDegats = value; }
}