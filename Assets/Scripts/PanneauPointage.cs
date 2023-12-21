using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// #tp4
/// Classe qui permet de gérer le panneau de pointage.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class PanneauPointage : MonoBehaviour
{
    [SerializeField] SOPerso _donneesPerso; // données du personnage
    [SerializeField] TextMeshProUGUI _nbNiveaux; // nombre de niveaux comolétés
    [SerializeField] TextMeshProUGUI _nbAmes; // nombre d'âmes collectées
    [SerializeField] TextMeshProUGUI _nbBonus; // nombre de bonus collectés
    [SerializeField] TextMeshProUGUI _scoreNiveaux; // score selon le nombre de niveaux complétés
    [SerializeField] TextMeshProUGUI _scoreAmes; // score selon le nombre d'âmes collectées
    [SerializeField] TextMeshProUGUI _scoreBonus; // score selon le nombre de bonus collectés
    [SerializeField] TextMeshProUGUI _scoreTotal; // score total
    public int scoreTotal { get { return _donneesPerso.niveau*_multiNiveau + _donneesPerso.argent*_multiAmes + _donneesPerso.bonus*_multiBonus; } } // score total
    [SerializeField] int _multiNiveau = 1000; // multiplicateur du score selon le nombre de niveaux complétés
    [SerializeField] int _multiAmes = 100; // multiplicateur du score selon le nombre d'âmes collectées
    [SerializeField] int _multiBonus = 10; // multiplicateur du score selon le nombre de bonus collectés

    /// <summary>
    /// Lors de l'éxécution du script, initialise les informations du panneau de pointage et les affiche.
    /// </summary>
    public void Start()
    {
        _nbNiveaux.text = _donneesPerso.niveau.ToString()+ " niveau" + ((_donneesPerso.niveau >=2)?"x":"");
        _nbAmes.text = _donneesPerso.argent.ToString();
        _nbBonus.text = _donneesPerso.bonus.ToString();
        _scoreNiveaux.text = (_donneesPerso.niveau*_multiNiveau).ToString();
        _scoreAmes.text = (_donneesPerso.argent*_multiAmes).ToString();
        _scoreBonus.text = (_donneesPerso.bonus*_multiBonus).ToString();
        _scoreTotal.text = scoreTotal.ToString();
    }

    // void AugmenterScore()
    // {
    //     scoreTotal 
    // }
}
