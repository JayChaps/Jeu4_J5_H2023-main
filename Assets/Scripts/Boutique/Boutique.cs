using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// ##tp3
/// Classe qui contrôle la boutique.
/// Permet l'achat d'objets.
/// Auteurs du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class Boutique : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso;
    public SOPerso donneesPerso => _donneesPerso;

    [SerializeField] TextMeshProUGUI _champNiveau;
    [SerializeField] TextMeshProUGUI _champArgent;

    bool _estEnPlay = true;

    static Boutique _instance;
    static public Boutique instance => _instance;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        if (_instance != null) { Destroy(gameObject); return; }
        _instance = this;
        MettreAJourInfos();
        _donneesPerso.InitialiserBoutique();
        _donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos);
    }

    private void MettreAJourInfos()
    {
        _champArgent.text = _donneesPerso.argent + " $";
        _champNiveau.text = "Niveau " + _donneesPerso.niveau;
    }

    void OnApplicationQuit()
    {
        _donneesPerso.InitialiserJeu();
        Debug.Log("Quit!");
        _estEnPlay = false;
    }

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    void OnDestroy()
    {
        _donneesPerso.evenementMiseAJour.RemoveAllListeners();
        if (_estEnPlay) _donneesPerso.niveau++;
        Debug.Log("Destroy!");
    }
}