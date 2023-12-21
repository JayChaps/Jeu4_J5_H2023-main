using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

/// <summary>
/// Classe sur le Niveau. Elle permet de générer les niveaux de
/// manière aléatoire selon le nombre de salles fournies 
/// et la taille demandée.
/// </summary>
public class Niveau : MonoBehaviour
{
    [SerializeField] Vector2Int _taille = new Vector2Int(3,3);  // La taille du niveau en nombre de salles horizontales et verticales.
    [SerializeField] Salle[] _tSallesModeles;                   // Tableau pour les modèles des salles qui vont être instanciées pour créer le niveau.

    [SerializeField] Tilemap _tilemap;                          // Le tilemap du niveau.
    public Tilemap tilemap => _tilemap;                         // Propriété qui permet d'accéder au champ de la tilemap depuis d'autres scripts.


    [SerializeField] TileBase _tuileModele;                     // Tuile utilisée pour les bordures.

    // #tp3
    public List<Vector2Int> _listePosSurReperes = new List<Vector2Int>();
    public List<Vector2Int> _listePosLibres = new List<Vector2Int>();

    [SerializeField] GameObject _clePrefab;                     // Le prefab de la clé.
    [SerializeField] GameObject _portePrefab;                   // Le prefab de la porte.
    [SerializeField] GameObject _activateurPrefab;              // Le prefab de l'activateur.
    [SerializeField] GameObject _joyauxPrefab;                  // Le prefab du joyau.
    [SerializeField] int _nbJoyauxParSalle = 10;                // Le nombre de joyaux par salle.
    [SerializeField] GameObject _bonusPrefab;                   // Le prefab du bonus.
    [SerializeField] int _nbBonusParSalle = 1;                  // Le nombre de bonus par salle.
    private int _increment = 0;                                 // Incrémenteur pour le nom des salles.

    // #tp4
    [SerializeField] GameObject _confineurCam;                  // Le confineur du niveau.
    [SerializeField] SOPerso _donneesPerso;                     // Les données du personnage.
    [SerializeField] TextMeshProUGUI _ameTMP;                   // Le texte du nombre d'âmes.
    [SerializeField] TextMeshProUGUI _bonusTMP;                 // Le texte du nombre de bonus.
    [SerializeField] TextMeshProUGUI _tempsTMP;                 // Le texte du chrono.
    [SerializeField] SONavigation _navigation;                  // Les données de navigation.
    [SerializeField] TextMeshProUGUI _degatsTMP;                // Le texte des dégâts.
    [SerializeField] Image _vitesseIMG;                         // L'image de la vitesse.
    [SerializeField] TextMeshProUGUI _niveauTMP;                // Le texte du niveau.

    // #synthese
    [SerializeField] GameObject[] _tCamerasGlobales;            // Tableau des caméras globales.
    [SerializeField] GameObject _goPerso;                       // Le gameObject du personnage.
    public GameObject goPerso { get => _goPerso; set => _goPerso = value; }
    private static Niveau _instance;
    public static Niveau instance { get => _instance; set => _instance = value; }

    /// <summary>
    /// Awake est appelée au chargement de l'instance du script.
    /// Appelle les méthodes pour générer le niveau.
    /// </summary>
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        GenererSalles();
        TrouverPosLibres();
        PlacerLesJoyaux();
        PlacerLesBonus();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        StartCoroutine(CoroutineCompteur()); //démarre la coroutine du chrono #tp4
    }
    
    /// <summary>
    /// Méthode qui génère les salles du niveau.
    /// </summary>
    private void GenererSalles()
    {
        Vector2Int tailleBordure = Salle.taille - Vector2Int.one; // Taille de la bordure de chaque salle.

        Vector2Int placementPorte = new Vector2Int(Random.Range(0, _taille.x), 0); // Détermine aléatoirement la position de la porte.
        Vector2Int placementCle = new Vector2Int(Random.Range(0, _taille.x), _taille.y-1); // Détermine aléatoirement la position de la clé.
        Vector2Int placementActivateur = new Vector2Int(Random.Range(0, _taille.x), (_taille.y-1)/2); // Détermine aléatoirement la position de l'activateur.
        Vector2Int placementPerso = new Vector2Int(Random.Range(0, _taille.x), 0); // Détermine aléatoirement la position du perso.
        
        for (int x = 0; x < _taille.x; x++) // Boucle sur l'axe des X.
        {
            for (int y = 0; y < _taille.y; y++) // Boucle sur l'axe des Y.
            {
                Vector2 pos = new Vector2(tailleBordure.x * x, tailleBordure.y * y);                        // On détermine la position de la salle.
                int aleaSalle = Random.Range(0, _tSallesModeles.Length);                                    // Choix aléatoire de la salle à instancier.
                Salle salle = Instantiate(_tSallesModeles[aleaSalle], pos, Quaternion.identity, transform); // Instanciation de la salle à sa position.
                salle.name = "Salle_" + x + "_" + y + "_(" + _increment + ")";                              // Nom de la salle selon sa position (X et Y).

                if (placementPorte == new Vector2Int(x,y))
                {
                    Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                    Vector2Int posRep = salle.ChercherPosPorte(_portePrefab) - decalage;
                    _listePosSurReperes.Add(posRep);
                }

                if (placementCle == new Vector2Int(x,y))
                {
                    Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                    Vector2Int posRep = salle.ChercherPosCle(_clePrefab) - decalage;
                    _listePosSurReperes.Add(posRep);
                }

                if (placementActivateur == new Vector2Int(x,y))
                {
                    Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                    Vector2Int posRep = salle.ChercherPosActivateur(_activateurPrefab) - decalage;
                    _listePosSurReperes.Add(posRep);
                }

                if (placementPerso == new Vector2Int(x,y))
                {
                    Vector2Int decalage = Vector2Int.CeilToInt(_tilemap.transform.position);
                    Vector2Int posRep = salle.ChercherPosPerso(goPerso) - decalage;
                    _listePosSurReperes.Add(posRep);
                }

                _increment++; // Augmente de 1 l'incrémenteur (pour nommer les salles).
            }
        }

        Vector2Int tailleNiveau = _taille * tailleBordure;   // La taille totale du niveau.
        Vector2Int min = Vector2Int.zero - Salle.taille / 2; // Taille minimale du niveau.
        Vector2Int max = min + tailleNiveau;                 // Taille maximale du niveau.

        _confineurCam.transform.localScale = new Vector3((max.x - min.x) + 1, -(max.y - min.y) - 1, 1); // #tp4 Changement du scale du confineur pour qu'il corresponde à la taille du niveau
        _confineurCam.transform.position = new Vector3(min.x, min.y, 0); // #tp4 Déplacement du confineur pour qu'il soit en bas à gauche du niveau
    
        for (int y = min.y; y <= max.y; y++) // Boucle sur l'axe des Y.
        {
            for (int x = min.x; x <= max.x; x++) // Boucle sur l'axe des X.
            {
                Vector3Int pos = new Vector3Int(x, y, 0);                         // Détermine la position de la tuile.
                if (x == min.x || x == max.x) tilemap.SetTile(pos, _tuileModele); // Si on est sur une extrémité, place la tuile.
                if (y == min.y || y == max.y) tilemap.SetTile(pos, _tuileModele); // Si on est sur une extrémité, place la tuile.
            }
        }
    }

    /// <summary>
    /// Cette fonction dresse une liste des espaces libres dans le niveau.
    /// Elle devrait être appelé après l'instantiation des objets uniques,
    /// afin que leurs emplacements ne soient pas considérés comme "vides".
    /// #tp3
    /// </summary>
    void TrouverPosLibres()
    {
        BoundsInt bornes = _tilemap.cellBounds;
        for (int y = bornes.yMin; y < bornes.yMax; y++)
        {
            for (int x = bornes.xMin; x < bornes.xMax; x++)
            {
                Vector2Int posTuile = new Vector2Int(x,y);
                TileBase tuile = _tilemap.GetTile((Vector3Int)posTuile);
                Vector2 posTilemap = _tilemap.transform.position;
                if (tuile == null) _listePosLibres.Add(posTuile);
            }
        }
        foreach (Vector2Int pos in _listePosSurReperes)
        {
            _listePosLibres.Remove(pos);
        }
    }

    /// <summary>
    /// Place les joyaux dans le niveau.
    /// #tp3
    /// </summary>
    void PlacerLesJoyaux() 
    {
        Transform conteneur = new GameObject("Joyaux").transform;
        conteneur.parent = transform;
        int nbJoyaux = _nbJoyauxParSalle * (_taille.x * _taille.y);
        for (int i = 0; i < nbJoyaux; i++)
        {
            Vector2Int pos = ObtenirPosLibre();

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            Instantiate(_joyauxPrefab, pos3, Quaternion.identity, conteneur);
        }
    }

    /// <summary>
    /// Place les bonus dans le niveau.
    /// #tp3
    /// </summary>
    void PlacerLesBonus()
    {
        Transform conteneur = new GameObject("Bonus").transform;
        conteneur.parent = transform;
        int nbBonus = _nbBonusParSalle * (_taille.x * _taille.y);
        for (int i = 0; i < nbBonus; i++)
        {
            Vector2Int pos = ObtenirPosLibre();

            Vector3 pos3 = (Vector3)(Vector2)pos + _tilemap.transform.position + _tilemap.tileAnchor;
            Instantiate(_bonusPrefab, pos3, Quaternion.identity, conteneur);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        MettreAJourHUD();
    }

    /// <summary>
    /// Cette fonction met à jour l'interface du jeu avec les infos du joueur.
    /// </summary>
    void MettreAJourHUD()
    {
        _ameTMP.text = _donneesPerso.argent.ToString(); //affiche le nombre d'âmes dans l'interface #tp4
        _bonusTMP.text = _donneesPerso.bonus.ToString(); //affiche le nombre de bonus dans l'interface #tp4
        _degatsTMP.text = "x"+_donneesPerso.degats.ToString(); //affiche le nombre de dégâts dans l'interface #tp4
        if (_donneesPerso.possedeSandales)
        {
            _vitesseIMG.color = Color.green; //si le joueur possède les sandales, la couleur de l'image de vitesse est verte #tp4
        } 
        else if (!_donneesPerso.possedeSandales)
        {
            _vitesseIMG.color = new Color (0.4f, 0, 0, 0.4f); //si le joueur ne possède pas les sandales, la couleur de l'image de vitesse est grise #tp4
        }
        _niveauTMP.text = "Niveau "+_donneesPerso.niveau.ToString(); //affiche le niveau dans l'interface #tp4
    }

    /// <summary>
    /// Diminue le compteur à chaque seconde
    /// #tp4
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineCompteur()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            _donneesPerso.secondes--;
            _tempsTMP.text = _donneesPerso.secondes.ToString();
            if(_donneesPerso.secondes==0) _navigation.AllerSceneFin(); //si il ne reste plus te temps, aller à la scène de fin
        }
    }

    /// <summary>
    /// Permet d'obtenir une position libre dans le niveau.
    /// #tp3
    /// </summary>
    Vector2Int ObtenirPosLibre() 
    {
        int indexPosLibre = Random.Range(0, _listePosLibres.Count);
        Vector2Int pos = _listePosLibres[indexPosLibre];
        _listePosLibres.RemoveAt(indexPosLibre);
        return pos;
    }
}