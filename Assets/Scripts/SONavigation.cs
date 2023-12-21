using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu (fileName = "Navigation", menuName = "TIM/Navigation")]

/// <summary>
/// #tp3
/// ScriptableObject de la navigation
/// Permet le changement de scène
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput 
/// </summary>
public class SONavigation : ScriptableObject
{
    [SerializeField] SOPerso _donneesPerso; //champ sérialisé pour les données du joueur

    /// <summary>
    /// Permet de passer à la scène suivante
    /// </summary>
    public void EntrerBoutique()
    {
        _donneesPerso.InitialiserBoutique(); //permet d'initialiser les données du joueur pour la boutique
        _donneesPerso.ViderInventaire(); //vide l'inventaire
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase, false); //désactive toutes les pistes musicales sauf celle de la boutique
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenA, false); 
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenB, false); 
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenC, true); 
        AllerSceneSuivante(); //va à la scène suivante
    }

    /// <summary>
    /// Permet de passer à la scène précédente
    /// </summary>
    public void SortirBoutique()
    {
        _donneesPerso.niveau++; //augmente le niveau du joueur
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueBase, true); //désactive toutes les pistes musicales sauf celle de base
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenA, false); 
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenB, false); 
        GestAudio.instance.ChangerEtatLecturePiste(TypePiste.musiqueEvenC, false); 
        _donneesPerso.InitialiserJeu(); //permet d'initialiser les données du joueur pour le jeu
        AllerScenePrecedente(); //va à la scène précédente
    }

    /// <summary>
    /// Va à la scène suivante
    /// </summary>
    public void AllerSceneSuivante()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    /// <summary>
    /// Va à la scène précédente
    /// </summary>
    public void AllerScenePrecedente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
    }

    /// <summary>
    /// Va à la dernière scène
    /// </summary>
    public void AllerSceneFin()
    {
        SceneManager.LoadScene("Generique");
    }

    /// <summary>
    /// Va à la première scène
    /// </summary>
    public void AllerSceneDebut()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Va à une scène spécifique
    /// </summary>
    /// <param name="nomScene">Le nom de la scène (attention aux fautes et majuscules!)</param>
    public void AllerSceneSpecifique(string nomScene)
    {
        SceneManager.LoadScene(nomScene);
    }

    /// <summary>
    /// Quitte le jeu
    /// </summary>
    public void QuitterJeu()
    {
        Application.Quit();
    }
}
