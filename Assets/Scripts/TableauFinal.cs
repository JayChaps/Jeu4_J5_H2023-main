using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #tp4
/// Classe du tableau de score final.
/// Permet de sauvegarder le score du joueur.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class TableauFinal : MonoBehaviour
{
    [SerializeField] SOSauvegarde _sauvegarde; //scriptable object de sauvegarde
    [SerializeField] Highscores _highscores; //script de gestion des highscores
    [SerializeField] PanneauPointage _panneauPointage; //script de gestion du panneau de pointage
    [SerializeField] TextMeshProUGUI _champInput; //champ de texte pour le nom du joueur

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        if(_sauvegarde.listeNomScore == null) _highscores.Initialiser(_sauvegarde); //initialise le tableau de highscores
        _panneauPointage.Start();
    }

    /// <summary>
    /// Sauvegarde le score du joueur dans le scriptable object de sauvegarde.
    /// </summary>
    public void Sauvegarder()
    {
        _sauvegarde.AjouterNomScore(_champInput.text, _panneauPointage.scoreTotal); //ajoute le nom et le score du joueur dans le scriptable object de sauvegarde
    }
}
