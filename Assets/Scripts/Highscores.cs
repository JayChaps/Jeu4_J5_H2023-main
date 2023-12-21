using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// #tp4
/// Classe qui permet de gérer les meilleurs scores.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Highscores : MonoBehaviour
{
    [SerializeField] SOSauvegarde _sauvegarde; //données de sauvagarde
    [SerializeField] GameObject _prefabLigne; //prefab de la ligne rouge qui contient les informations d'un des 3 meilleurs scores
    [SerializeField] GameObject _prefabNouvelleLigne; //prefab de la nouvelle ligne qui contient les informations de la partie qui vient de s'achever
    [SerializeField] int _nbScoresMax = 3; //nombre de meilleurs scores maximum à afficher
    List<GameObject> _listeLignes = new List<GameObject>(); //liste qui contient les 3 prefabs de lignes

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Initialiser(_sauvegarde);
    }


    /// <summary>
    /// Initialise les meilleurs scores.
    /// </summary>
    /// <param name="sauve">Les données de sauvegarde</param>
    public void Initialiser(SOSauvegarde sauve)
    {
        foreach (GameObject obj in _listeLignes)
        {
            Destroy(obj); //détruit les lignes rouges
        }

        _sauvegarde.listeNomScore.Sort((x, y) => y.score.CompareTo(x.score)); //classe la liste des scores
        for (int i = 0; i < _nbScoresMax; i++)
        {
            AjouterScore(sauve.listeNomScore[i].nom, sauve.listeNomScore[i].score); //ajoute le score à la liste
        }
        if (sauve.listeNomScore.Count > _nbScoresMax)
        {
            sauve.listeNomScore.RemoveAt(_nbScoresMax);
        }
    }

    /// <summary>
    /// Ajoute un score à la liste des meilleurs scores.
    /// </summary>
    /// <param name="nom">Le nom du joueur</param>
    /// <param name="score">Le score du joueur</param>
    public void AjouterScore(string nom, float score)
    {
        GameObject obj = Instantiate(_prefabLigne, transform); //instancie le prefab
        _listeLignes.Add(obj); //ajoute le prefab à la liste
        obj.GetComponent<ScoreIndividuel>().Initialiser(transform.childCount, nom, score);
    }
}
