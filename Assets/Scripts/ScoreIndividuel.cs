using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #tp4
/// Classe de la ligne rouge qui contient les informations d'un des 3 meilleurs scores.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class ScoreIndividuel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _position; //champ text du position du score
    [SerializeField] TextMeshProUGUI _nom; //champ text du nom du joueur
    [SerializeField] TextMeshProUGUI _score; //champ text du score du joueur

    /// <summary>
    /// Initialise les informations du score.
    /// </summary>
    /// <param name="pos">La position du score dans la liste</param>
    /// <param name="nom">Le nom du joueur</param>
    /// <param name="score">Le score du joueur</param>
    public void Initialiser(int pos, string nom, float score)
    {
        _position.text = pos.ToString(); //converti le int en string et l'affiche
        _nom.text = nom; //affiche le nom
        _score.text = score.ToString(); //converti le float en string et l'affiche
    }
}
