using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (fileName = "Perso", menuName = "TIM/Perso")]

/// <summary>
/// #tp3 & #tp4
/// ScriptableObject du personnage.
/// Contient les données en lien avec le joueur.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput 
/// </summary>
public class SOPerso : ScriptableObject
{
    [SerializeField][Range(1,5)] int _niveauIni = 1; // Niveau initial du joueur 
    [SerializeField][Range(0,500)] int _argentIni = 0; // Argent initial du joueur 
    [SerializeField][Range(1,5)] int _niveau = 1; // Niveau actuel du joueur 
    [SerializeField][Range(0,500)] int _argent = 0; // Argent actuel du joueur 
    [SerializeField][Range(0,10)] int _bonus = 0; // Nombre de bonus amassés par le joueur 
    [SerializeField][Range(1,10)] int _degats = 1; // Dégats du joueur
    [SerializeField][Range(1,10)] int _bonusIni = 0; // Nombre de bonus initial du joueur
    [SerializeField][Range(1,10)] int _degatsIni = 1; // Dégats initial du joueur
    bool _aLaCle = false; // Détermine si le joueur possède la clé ou non 
    public bool aLaCle { get => _aLaCle; set => _aLaCle = value; } // Accesseur de la clé 
    bool _aActiverBonus = false; // Détermine si le joueur a activé l'activateur des bonus ou non 
    public bool aActiverBonus { get => _aActiverBonus; set => _aActiverBonus = value; } // Accesseur du bonus 
    [SerializeField] bool _possedeSandales = false; // Détermine si le joueur possède les sandales ou non
    public bool possedeSandales { get => _possedeSandales; set => _possedeSandales = value; } // Accesseur des sandales

    [SerializeField] int _secondes = 90; //le nombre de secondes du chrono #tp4
    [SerializeField] int _secondesInitiales = 90; //le nombre de secondes du chrono #tp4
    public int secondes { get => _secondes; set => _secondes = value; } // Accesseur des secondes #tp4
    public int secondesInitiales { get => _secondesInitiales; set => _secondesInitiales = value; } // Accesseur des secondes initiales #tp4

    [SerializeField][Range(0,100)] float _vie = 50f; // Vie du joueur #synthese
    [SerializeField] float _vieMax = 50f; // Vie max du joueur #synthese
    public float vie { get => _vie; set => _vie = value; } // Accesseur de la vie #synthese
    public float vieMax { get => _vieMax; set => _vieMax = value; } // Accesseur de la vie max #synthese

    public int niveau // Accesseur du niveau 
    { 
        get => _niveau; 
        set 
        {
            _niveau = Mathf.Clamp(value, 1, int.MaxValue); 
            _evenementMiseAJour.Invoke();
        }
    }
    
    public int argent // Accesseur de l'argent 
    { 
        get => _argent; 
        set
        {
            _argent = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke();
        } 
    }
    
    public int bonus // Accesseur du bonus 
    { 
        get => _bonus; 
        set
        {
            _bonus = Mathf.Clamp(value, 0, int.MaxValue);
            _evenementMiseAJour.Invoke();
        } 
    }

    public int degats // Accesseur des dégats 
    { 
        get => _degats; 
        set
        {
            _degats = Mathf.Clamp(value, 1, int.MaxValue);
            _evenementMiseAJour.Invoke();
        } 
    }

    UnityEvent _evenementMiseAJour = new UnityEvent();
    public UnityEvent evenementMiseAJour => _evenementMiseAJour;

    UnityEvent _activerLesBonus = new UnityEvent(); // Event pour activer les bonus
    public UnityEvent activerLesBonus => _activerLesBonus; // Accesseur de l'event


    [SerializeField] List<SOObjet> _inventaire = new List<SOObjet>(); // Listes des objets que posède le joueur
    
    /// <summary>
    /// Réinitialise les données de dégats et de sandales lorsqu'on entre dans la boutique
    /// </summary>
    public void InitialiserBoutique() 
    {
        _degats = _degatsIni;
        _possedeSandales = false;
    }

    /// <summary>
    /// Réinitialise les données de l'argent et du bonus lorsqu'on entre dans le jeu
    /// </summary>
    public void InitialiserJeu() 
    {
        // _argent = _argentIni;
        _bonus = _bonusIni;
    }

    /// <summary>
    /// Réinitialise toutes les données du joueur à leur valeurs initiales
    /// </summary>
    public void ToutReinitialiser() 
    {
        _niveau = _niveauIni;
        _argent = _argentIni;
        _bonus = _bonusIni;
        _degats = _degatsIni;
        _possedeSandales = false;
        _inventaire.Clear();
    }
    
    /// <summary>
    /// Vide l'inventaire du joueur
    /// </summary>
    public void ViderInventaire()
    {
        _inventaire.Clear();
    }

    /// <summary>
    /// Permet d'acheter un objet dans la boutique
    /// </summary>
    /// <param name="donneesObjet"></param>
    public void Acheter(SOObjet donneesObjet) 
    {
        argent -= donneesObjet.prix; // Retire l'argent au joueur
        _evenementMiseAJour.Invoke();
        if(donneesObjet.permetDeCourir) possedeSandales = true; // Si l'objet permet de courir, le joueur possède les sandales
        if(donneesObjet.augmenteLesDegats) _degats++; // Si l'objet augmente les dégats, on augmente l'augmentation des dégats
        _inventaire.Add(donneesObjet); // Ajoute l'objet à la liste de l'inventaire
        AfficherInventaire();
    }

    /// <summary>
    /// Affiche les objets que possède le joueur dans la console
    /// </summary>
    void AfficherInventaire()
    {
        string inventaire = "";
        foreach (SOObjet objet in _inventaire)
        {
            if (inventaire != "") inventaire += ", ";
            inventaire += objet.nom;
        }
        Debug.Log("Inventaire du perso: "+inventaire);
    }

    /// <summary>
    /// Lorsque l'on modifie les valeurs, on invoque l'événement de mise à jour
    /// </summary>
    void OnValidate()
    {
        _evenementMiseAJour.Invoke();
    }

}
