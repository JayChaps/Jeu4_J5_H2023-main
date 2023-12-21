using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

/// <summary>
/// Classe qui controle les déplacements du personnage
/// Auteurs du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class Perso : MonoBehaviour
{
    [Header("Champs composants")]
    private Rigidbody2D _rb; //rigidbody du personnage
    private SpriteRenderer _sr; //sprite renderer du personnage

    [Header("Champs déplacements")]
    [SerializeField] float _forceSaut = 75f; //force du saut du personnage
    [SerializeField] float _vitesse = 5f; //vitesse de déplacement horizontal du personnage
    [SerializeField] float _vitesseIni = 5f; //vitesse initiale du personnage
    [SerializeField] float _multiplicateurVitesse = 1.5f; //multiplicateur de vitesse du personnage

    [Space(5)]
    [SerializeField] int _nbFramesMax = 10; //nombre de frames maximum à prendre en considération lors du saut
    [SerializeField] int _nbFramesRestants = 0; //nombre de frames restantes à prendre en considération lors du saut
    
    [Space(5)]
    private float _distanceDebutSol = 0.5f; //distance entre le personnage et le sol
    private float _floatRayonSol = 0.6f; //longueur du rayon dans lequel vérifier le sol
    private Vector2 _longueurRayonSol; // = new Vector2(0f, 0.6f); //longueur du rayon dans lequel vérifier le sol
    [SerializeField] LayerMask _layerMask; //layer du sol

    private float _axeV; //l'axe vertical (joystick ou clavier)
    private float _axeH; //l'axe horizontal (joystick ou clavier)
    [Space(5)]
    private float _veloY; //vitesse verticale du personnage

    private bool _veutSauter = false; //bool qui détermine si le joueur veut sauter
    private bool _estAuSol = true; //bool qui détermine si le joueur est au sol

    // #tp3
    [Header("Champs TP3")]
    [SerializeField] ParticleSystem _particules; //particules #tp3
    ParticleSystem.EmissionModule _emission; //module d'émission des particules #tp3
    [SerializeField] SOPerso _donnees; //données du joueur #tp3
    [SerializeField] SONavigation _navigation; //système de navigation entre les scènes #tp3

    // #tp4
    [Header("Champs TP4")]
    private Animator _anim; //animator du personnage #tp4
    [Header("Champs Sons")]
    [SerializeField] AudioClip _sonSaut; //son du saut #tp4
    [SerializeField] AudioClip _sonPas; //son du pas #tp4
    [SerializeField] AudioClip _sonGlisse; //son de la glisse #synthese
    [SerializeField] AudioClip _sonDegat; //son des dégats #synthese

    // #synthese
    [Header("Champs synthèse")]
    [SerializeField] GameObject _colHaut; //collider du haut #synthese
    [SerializeField] GameObject _colBas; //collider du bas #synthese
    [SerializeField] bool _estAuMur = false; //bool qui détermine si le joueur est au mur #synthese
    [SerializeField] bool _estColleAuMur = false; //bool qui détermine si le joueur est collé au mur #synthese
    [SerializeField] bool _glisseDuMur = false; //bool qui détermine si le joueur glisse du mur #synthese
    [SerializeField] bool _peutRebondir = false; //bool qui détermine si le joueur peut sauter du mur #synthese
    [SerializeField] bool _estRetourne = false; //bool qui détermine si le joueur est retourné #synthese
    [SerializeField] float _puissanceRebond = 1.3f; //force du rebond #synthese

    [Space(5)]
    [SerializeField] GameObject _prefabFondu; //trail en fondu qui suit le perso lors de la course #synthese
    [SerializeField] GameObject _particulesGlisse; //particules qui sortent du perso lorsqu'il glisse du mur #synthese
    Coroutine _routineTrail;
    Coroutine _routineAgrippeMur;
    Coroutine _routineRebondMural;
    [SerializeField] CinemachineVirtualCamera _cam; //caméra #synthese
    CinemachineBasicMultiChannelPerlin _bruitCam;

    [Space(5)]
    [SerializeField] private BarreDeVie _barreDeVie; //barre de vie #synthese
    [SerializeField] float _nbFramesAvantRegen = 0f; //secondes avant de regagner de la vie #synthese


    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); //va chercher le rigidbody du perso pour lui parler
        _sr = GetComponent<SpriteRenderer>(); //va chercher le sprite renderer du perso pour lui parler
        _anim = GetComponent<Animator>(); //va chercher l'animator du perso pour lui parler #tp4
        _vitesse = _vitesseIni; //initialise la vitesse du personnage #tp3
        _donnees.aLaCle = false; //le joueur n'a plus la clé #tp3
        _donnees.aActiverBonus = false; //désactive les bonus #tp3
        _donnees.vie = 100f; //initialise la vie du personnage #synthese
        _emission = _particules.emission; //va chercher l'emission des particules #tp3
        _donnees.secondes = _donnees.secondesInitiales; //initialise le chrono #tp4
        if(_donnees.niveau == 1) _donnees.ToutReinitialiser(); else _donnees.InitialiserJeu();
        _bruitCam = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>(); //initialise le shake de la caméra #tp4
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        MiseAJourVie();

        //déplacement du code par le prof!
        _veutSauter = Input.GetButton("Jump"); //si le joueur appuie sur le bouton de saut, _veutSauter devient "true"
        _axeV = Input.GetAxis("Vertical"); //retourne un float entre -1 et 1 selon la direction verticale du joueur
        _axeH = Input.GetAxis("Horizontal"); //retourne un float entre -1 et 1 selon la direction horizontale du joueur
        //fin déplacement du code par le prof!

        // if(_axeH > 0) _sr.flipX = false; //si le joueur regarde à droite, ne flip pas le sprite horizontalement
        // if(_axeH < 0) _sr.flipX = true; //si le joueur regarde à gauche, flip le sprite horizontalement
        if(_axeH > 0) _estRetourne = false; //si le joueur regarde à droite, ne flip pas le sprite horizontalement
        if(_axeH < 0) _estRetourne = true; //si le joueur regarde à gauche, flip le sprite horizontalement
        if(!_estRetourne) transform.localScale = new Vector3(2, transform.localScale.y, transform.localScale.z); //si le joueur regarde à droite, ne flip pas le sprite horizontalement
        if(_estRetourne) transform.localScale = new Vector3(-2, transform.localScale.y, transform.localScale.z); //si le joueur regarde à gauche, flip le sprite horizontalement

        _veloY = (float)decimal.Round((decimal)_rb.velocity.y, 2); //arrondi la vélocité verticale du joueur à 2 décimales #tp4
        
        _anim.SetFloat("Horizontal", Mathf.Abs(_axeH)); //envoie les valeurs de l'axe horizontal à l'animator #tp4
        _anim.SetFloat("Vertical", _veloY); //envoie les valeurs de la vélocité verticale à l'animator #tp4
        _anim.SetBool("estEnMouvement", (_axeH != 0 || _veloY != 0)); //#tp4
        _anim.SetBool("estEnSaut", (_veloY > 0)); //#tp4
        _anim.SetBool("estEnChute", (_veloY < 0)); //#tp4
        _anim.SetBool("estAuSol", _estAuSol); //#tp4
        _anim.SetBool("estAuMur", _estAuMur); //#synthese

        // if(_sr.flipX == false && _axeH != 0) _particules.transform.eulerAngles = new Vector3(_particules.transform.eulerAngles.x, 0, _particules.transform.eulerAngles.z); //#tp4
        // if(_sr.flipX == true && _axeH != 0) _particules.transform.eulerAngles = new Vector3(_particules.transform.eulerAngles.x, 180, _particules.transform.eulerAngles.z); //flip les particules horizontalement #tp3
        if(!_estRetourne && _axeH != 0) _particules.transform.eulerAngles = new Vector3(_particules.transform.eulerAngles.x, 0, _particules.transform.eulerAngles.z); //#tp4
        if(_estRetourne && _axeH != 0) _particules.transform.eulerAngles = new Vector3(_particules.transform.eulerAngles.x, 180, _particules.transform.eulerAngles.z); //flip les particules horizontalement #tp3
        if (_axeH == 0 || _estAuSol == false) _emission.enabled = false; //si le joueur ne bouge pas ou s'il est en l'air, désactive les particules #tp4
        else _emission.enabled = true; //#tp4

        if(!_estAuMur)
        {
            ArreterCoroutine(_routineAgrippeMur);
            _routineAgrippeMur = null; 
        }

        if(_estAuSol)
        {
            ArreterCoroutine(_routineRebondMural);
            _routineRebondMural = null;
            

            if(_donnees.possedeSandales) //si le joueur possède les sandales et appuie sur le bouton de débug (left-shift), augmente la vitesse
            {
                if(Input.GetButton("Debug Multiplier")) 
                {
                    // Debug.Log("Status : " +_routineTrail);
                    if(_routineTrail == null)
                    {
                        _vitesse = _vitesseIni * _multiplicateurVitesse;
                        _anim.speed = _multiplicateurVitesse;
                        _routineTrail = StartCoroutine(CoroutineTrail());
                    }
                }
                else //sinon, remet la vitesse à la normale
                {
                    _vitesse = _vitesseIni;
                    _anim.speed = 1f;
                    // Debug.Log("Premier stop : "+_routineTrail);
                    ArreterCoroutine(_routineTrail);
                    _routineTrail = null; //patch prof!
                }
            }
            else //sinon, remet la vitesse à la normale
            {
                _vitesse = _vitesseIni;
                _anim.speed = 1f;
                // Debug.Log("Deuxième stop : "+_routineTrail);
                ArreterCoroutine(_routineTrail);
                _routineTrail = null; //patch prof!
            } 
        }

        VerifierSautMural();

        // if (Input.GetKey(KeyCode.D)) Debug.Log("D est enfoncé");
        // if (Input.GetKey(KeyCode.A)) Debug.Log("A est enfoncé");

        if (_glisseDuMur)
        {
            _bruitCam.m_AmplitudeGain = Mathf.Lerp(_bruitCam.m_AmplitudeGain, 1f, 0.005f);
            _particulesGlisse.SetActive(true);
        } 
        else 
        { 
            _bruitCam.m_AmplitudeGain = 0f; 
            if (!_estAuMur) _particulesGlisse.SetActive(false); 
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Debug.Log("Coroutine Agrippe Mur : " + _routineAgrippeMur + " | Coroutine Rebond Mural : " + _routineRebondMural + " | Coroutine Trail : " + _routineTrail);

        _nbFramesAvantRegen = Mathf.Clamp(_nbFramesAvantRegen, 0, 300); //clamp le nombre de frames avant la régénération entre 0 et 300
        _nbFramesAvantRegen--; //décrémente le nombre de frames avant la régénération
        if (_nbFramesAvantRegen <= 0) _donnees.vie += 0.01f;
        _donnees.vie = Mathf.Clamp(_donnees.vie, 0, _donnees.vieMax); //clamp la vie entre 0 et sa valeur maximale
        if (_donnees.vie <= 0) _navigation.AllerSceneSpecifique("Generique");

        // _veutSauter = Input.GetButton("Jump"); //si le joueur appuie sur le bouton de saut, _veutSauter devient "true"
        // _axeV = Input.GetAxis("Vertical"); //retourne un float entre -1 et 1 selon la direction verticale du joueur
        // _axeH = Input.GetAxis("Horizontal"); //retourne un float entre -1 et 1 selon la direction horizontale du joueur

        VerifierCourse();
        VerifierSol(); //appel de la méthode pour vérifier si le joueur est au sol

        if (_estAuSol) //si le joueur est au sol
        {
            if (_veutSauter) //si le joueur veut sauter
            {
                float fractionDeForce = (float)_nbFramesRestants / _nbFramesMax; //calcule la fraction de force en fonction du nombre de frames restantes
                float puissance = _forceSaut * fractionDeForce; //calcule la puissance du saut en multipliant la force par la fraction
                _rb.AddForce(Vector2.up * puissance); //ajoute de la force vers le haut au perso égale à la puissance
                _nbFramesRestants--; //réduit le nombre de frames restantes à chaque frame
                if (_nbFramesRestants < 0) _nbFramesRestants = 0; //s'assure que le nombre de frames restantes ne descend pas sous 0
            }
            else _nbFramesRestants = _nbFramesMax; //si le joueur est au sol, mais ne veut pas sauter, reset le nombre de frames restantes
        }
        else //sinon
        {
            bool peutSauterPlus = (_nbFramesRestants > 0); //si il reste des frames de saut, le joueur peut sauter plus
            if (_veutSauter && peutSauterPlus) //si le joueur veut et peut sauter d'avantage
            {
                float fractionDeForce = (float)_nbFramesRestants / _nbFramesMax; //calcule la fraction de force en fonction du nombre de frames restantes
                float puissance = _forceSaut * fractionDeForce; //calcule la puissance du saut en multipliant la force par la fraction
                _rb.AddForce(Vector2.up * puissance); //ajoute de la force vers le haut au perso égale à la puissance
                _nbFramesRestants--; //réduit le nombre de frames restantes à chaque frame
                if (_nbFramesRestants < 0) _nbFramesRestants = 0; //s'assure que le nombre de frames restantes ne descend pas sous 0
            }
            else _nbFramesRestants = 0; //sinon, le nombre de frames restantes est égale à 0
        }

        if (Input.GetKeyDown(KeyCode.Escape)) _navigation.QuitterJeu();
    }

    private void VerifierCourse()
    {
        if (_routineAgrippeMur != null || _routineRebondMural != null) //si le joueur va faire / est en train de faire un saut mural
        {
            return; //empêche le joueur de courir
        }
        _rb.velocity = new Vector2(_vitesse * _axeH, _rb.velocity.y); //change la vélocité selon la direction horizontale du joueur 
    }

    void VerifierSautMural()
    {
        bool colHautTouche = _colHaut.GetComponent<CollidersWJ>().touche;
        bool colBasTouche = _colBas.GetComponent<CollidersWJ>().touche;
        _estAuMur = colHautTouche && colBasTouche && _estAuSol == false; //vérifie si le joueur est au mur
        bool vaVersGauche = (_axeH < 0) || (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)); //vérifie si le joueur va vers la gauche
        bool vaVersDroite = (_axeH > 0) || (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)); //vérifie si le joueur va vers la droite

        if (_estAuMur && !_peutRebondir && ((!_estRetourne && vaVersDroite) || (_estRetourne && vaVersGauche))) //si le joueur est au mur et qu'il va vers le mur
        {
            if (_routineAgrippeMur == null)
            {
                _routineAgrippeMur = StartCoroutine(CoroutineAgrippeMur());
            }
        }
        else if (!_estAuMur || _estAuSol || (!vaVersGauche && !vaVersDroite))
        {
            _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, 3, 0.5f);
            ArreterCoroutine(_routineAgrippeMur);
            _routineAgrippeMur = null; //patch prof!
        }

        if (_estAuMur || _estColleAuMur)
        {
            _peutRebondir = true;
            
            if ((vaVersGauche && _veutSauter) || (vaVersDroite && _veutSauter)) //si le joueur va vers le mur et qu'il veut sauter (patch prof pour permettre flèches et manette)
            //if ((vaVersGauche && Input.GetKeyDown(KeyCode.Space)) || (vaVersDroite && Input.GetKeyDown(KeyCode.Space)))
            {
                ArreterCoroutine(_routineAgrippeMur);
                _routineAgrippeMur = null; //patch prof!
                _routineRebondMural = StartCoroutine(CoroutineRebondMural());
            }
        }

        if (!_estAuMur)
        {
            _glisseDuMur = false;
            _estColleAuMur = false;
            _peutRebondir = false;
            ArreterCoroutine(_routineAgrippeMur);
            _routineAgrippeMur = null; //patch prof!
            ArreterCoroutine(_routineRebondMural);
            //pas de patch ici: sinon ça empêche le rebond mural
        }
    }

    IEnumerator CoroutineAgrippeMur()
    {
        _rb.gravityScale = 0;
        _rb.velocity = Vector2.zero;
        _estColleAuMur = true;
        _peutRebondir = true;
        _glisseDuMur = false;
        
        yield return new WaitForSeconds(1f);
        
        _particulesGlisse.SetActive(true);
        JouerSon(_sonGlisse);

        yield return new WaitForSeconds(1f);

        _estColleAuMur = false;
        _glisseDuMur = true;
        _rb.gravityScale = Mathf.Lerp(_rb.gravityScale, 3, 0.02f);

        yield return null;
    }

    IEnumerator CoroutineRebondMural()
    {
        _rb.gravityScale = 3;
        _estColleAuMur = false;

        float forceRebond = Mathf.Sqrt(2 * _puissanceRebond * Physics2D.gravity.magnitude);

        JouerSon(_sonSaut);
        _rb.AddForce(new Vector2((forceRebond * (-transform.localScale.x / 2)), forceRebond), ForceMode2D.Impulse);

        _peutRebondir = false;
        _glisseDuMur = false;

        yield return new WaitForSeconds(0.1f);
        ArreterCoroutine(_routineRebondMural);
        _routineRebondMural = null;
        yield return null;
    }

    /// <summary>
    /// Coroutine qui permet de faire la trail du perso.
    /// </summary>
    /// <returns></returns>
    IEnumerator CoroutineTrail()
    {
        while (true) 
        {
            Instantiate(_prefabFondu, transform.position, Quaternion.identity, transform.parent);
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Permet de vérifier si le perso se trouve au sol.
    /// </summary>
    private void VerifierSol()
    {
        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0, _distanceDebutSol); //retourne le point de départ du cercle de détection
        Vector2 direction = Vector2.down; //indique que la direction vers laquelle le cercle ira est vers le bas
        RaycastHit2D col = Physics2D.Raycast(pointDepart, Vector2.down, _floatRayonSol, _layerMask); //crée un cercle de détection de collision 
        _estAuSol = (col == true); //si une collision quelconque est détectée, retourne l'info dans _estAuSol
    }

    void ArreterCoroutine(Coroutine routine)
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null; //cette ligne est inutile, car la variable locale routine n'est pas la véritable variable globale (ex. _routineTrail)
        }
    }

    /// <summary>
    /// Permet de dessiner des gizmos sur la scène
    /// </summary>
    void OnDrawGizmos()
    {
        _longueurRayonSol = new Vector2(0f, _floatRayonSol);
        if(!Application.isPlaying) VerifierSol(); //si l'editeur Unity n'est pas en mode play, appelle la méthode VerifierSol

        if(_estAuSol) Gizmos.color = Color.green; //si le joueur est au sol, le gizmo est vert
        else if(!_estAuSol) Gizmos.color = Color.red; //sinon, le gizmo est rouge

        Vector2 pointDepart = (Vector2)transform.position - new Vector2(0, _distanceDebutSol); //retourne le point de départ du cercle du gizmo
        Gizmos.DrawRay(pointDepart, Vector3.down*_longueurRayonSol); //dessine un gizmo linéaire à partir du point centre de la longueur du rayon
    }

    /// <summary>
    /// Permet de faire jouer un son
    /// #tp4
    /// </summary>
    /// <param name="son">Le clip audio</param>
    public void JouerSon(AudioClip son)
    {
        GestAudio.instance.JouerEffetSonore(son);
    }

    /// <summary>
    /// Met à jour la vie du personnage, ainsi que la rotation horizontale de la barre de vie
    /// </summary>
    void MiseAJourVie()
    {
        _barreDeVie.MiseAJourVie(_donnees.vie, _donnees.vieMax);
        GameObject parentBarre = _barreDeVie.transform.parent.gameObject;
        if (_estRetourne) parentBarre.transform.localScale = new Vector3(-1, 1, 1);
        else if (!_estRetourne) parentBarre.transform.localScale = new Vector3(1, 1, 1);
    }

    /// <summary>
    /// Appelé lorsque le personnage entre en collision avec un autre objet
    /// </summary>
    /// <param name="other">L'autre objet.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ennemi"))
        {
            _donnees.vie -= 5;
            _nbFramesAvantRegen = 300f;
            MiseAJourVie();
            JouerSon(_sonDegat);
        }
    }
}