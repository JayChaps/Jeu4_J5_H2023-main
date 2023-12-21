using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

/// <summary>
/// #synthese
/// Classe de l'ennemi qui marche et qui détecte le joueur.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class EnnemiMarche : MonoBehaviour
{
    [SerializeField] public float _vitesse = 3f; // Vitesse de l'ennemi
    [SerializeField] public float _distanceDetection = 10f; // Distance de détection du joueur
    [SerializeField] AudioClip _sonDegat; // Son de dégat de l'ennemi
    [SerializeField] private BarreDeVie _barreDeVie; //barre de vie #synthese

    private Rigidbody2D _rb; // Rigidbody de l'ennemi
    private SpriteRenderer _sr; // Sprite renderer de l'ennemi
    private Animator _anim; // Animator de l'ennemi

    private Coroutine _routineMouvementEnnemi; // Coroutine du mouvement de l'ennemi

    private GameObject _joueur; // GameObject du joueur
    [SerializeField] float _orientation; // Orientation de l'ennemi

    [SerializeField] private int _vie = 10; // Vie de l'ennemi
    [SerializeField] private int _vieMax = 10; // Vie max de l'ennemi
    [SerializeField] SOPerso _donneesPerso; // Données du personnage #synthese
    bool _vaADroite = false; // Booléen qui indique si l'ennemi va à droite ou à gauche


    private void Start()
    {
        _joueur = Niveau.instance.goPerso; // Récupère le gameobject du joueur
        _rb = GetComponent<Rigidbody2D>(); // Récupère le rigidbody de l'ennemi
        _sr = GetComponent<SpriteRenderer>(); // Récupère le sprite renderer de l'ennemi
        if (GetComponent<Animator>() != null) 
        {
            _anim = GetComponent<Animator>(); // Récupère l'animator de l'ennemi si il en a un
        }
        _routineMouvementEnnemi = StartCoroutine(CoroutineMouvementEnnemi()); // Lance la coroutine du mouvement de l'ennemi
        _orientation = transform.localScale.x; // Récupère l'orientation de l'ennemi
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        MiseAJourVie(); // Met à jour la vie de l'ennemi
    }

    /// <summary>
    /// Coroutine du mouvement de l'ennemi.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoroutineMouvementEnnemi()
    {
        while (true) // Fait tourner le comportement de l'ennemi en boucle 
        {
            if (JoueurProche()) // Si le joueur est à portée de détection
            {
                if(_anim != null) _anim.SetBool("marche", true); // Lance l'animation de marche si l'ennemi en a une
                
                if (transform.position.x > PosJoueur().x) // Si le joueur est à gauche, va à gauche
                {
                    _rb.velocity = new Vector2(-_vitesse, _rb.velocity.y); // Fait avancer l'ennemi
                    transform.localScale = new Vector2(_orientation, transform.localScale.y); // Change l'orientation de l'ennemi
                    _vaADroite = false; // Indique que l'ennemi va à gauche
                }
                else // Si le joueur est à droite, va à droite
                {
                    _rb.velocity = new Vector2(_vitesse, _rb.velocity.y); // Fait avancer l'ennemi
                    transform.localScale = new Vector2(-_orientation, transform.localScale.y); // Change l'orientation de l'ennemi
                    _vaADroite = true; // Indique que l'ennemi va à droite
                }
            }
            else // Si le joueur n'est pas à portée de détection, ne bouge pas
            {
                _rb.velocity = new Vector2(0f, _rb.velocity.y); // Arrête l'ennemi
                if(_anim != null) _anim.SetBool("marche", false); // Lance l'animation d'arrêt si l'ennemi en a une
            }

            yield return null;
        }
    }

    /// <summary>
    /// Vérifie si le joueur est à portée de détection.
    /// </summary>
    /// <returns>True ou False</returns>
    private bool JoueurProche()
    {
        if (_joueur != null)
        {
            float distance = Vector2.Distance(transform.position, _joueur.transform.position); // Calcule la distance entre l'ennemi et le joueur
            return distance <= _distanceDetection; // Retourne true si le joueur est à portée de détection
        }
        return false; // Retourne false si le joueur n'est pas à portée de détection
    }

    /// <summary>
    /// Récupère la position du joueur.
    /// </summary>
    /// <returns>Vector2</returns>
    private Vector2 PosJoueur()
    {
        if (_joueur != null)
        {
            return _joueur.transform.position; // Retourne la position du joueur
        }
        return Vector2.zero; // Retourne Vector2.zero si le joueur n'est pas trouvé
    }

    /// <summary>
    /// Dessine la distance de détection de l'ennemi.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _distanceDetection);
    }

    /// <summary>
    /// Met à jour la vie du personnage, ainsi que la rotation horizontale de la barre de vie
    /// </summary>
    void MiseAJourVie()
    {
        _barreDeVie.MiseAJourVie(_vie, _vieMax);
        GameObject parentBarre = _barreDeVie.transform.parent.gameObject;
        if (_vaADroite) parentBarre.transform.localScale = new Vector3(-1, 1, 1);
        else if (!_vaADroite) parentBarre.transform.localScale = new Vector3(1, 1, 1);

        if (_vie == 0) Destroy(gameObject);
    }

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            _vie -= _donneesPerso.degats; // Retire de la vie à l'ennemi
            GestAudio.instance.JouerEffetSonore(_sonDegat); // Joue le son de dégât
            MiseAJourVie();
        }
    }
}
