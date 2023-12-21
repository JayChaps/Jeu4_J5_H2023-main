using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese
/// Classe du viseur du personnage.
/// Permet de faire suivre le viseur au personnage et de faire tirer des projectiles.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Viseur : MonoBehaviour
{
    private Vector3 _posSouris; // Position de la souris
    [SerializeField] private GameObject _prefabProjectile; // Prefab du projectile
    [SerializeField] private GameObject _prefabProjecTriple; // Prefab du projectile de triple tir
    [SerializeField] private GameObject _perso; // GameObject du personnage
    [SerializeField] float _incrementation = 20f; // Incrémentation de l'angle du triple tir
    [SerializeField] private SOPerso _donneesPerso; // Données du personnage
    [SerializeField] private GameObject _repereViseur; // Repère du viseur

    // Update is called once per frame
    void Update()
    {
        transform.position = _perso.transform.position; // Fait suivre le viseur au personnage
        
        _posSouris = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Va chercher la position de la souris
        Vector2 v = transform.position - _posSouris; // Calcule la direction entre le viseur et la souris
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg; // Calcule l'angle entre le viseur et la souris
        float angleFix = angle + 180; // Ajoute 180 degrés à l'angle
        transform.rotation = Quaternion.Euler(0, 0, angleFix); // Fait tourner le viseur vers la souris

        if (Input.GetButtonDown("Fire1")) // Si le bouton gauche de la souris est enfoncé
        {
            Instantiate(_prefabProjectile, _repereViseur.transform.position, Quaternion.Euler(0, 0, angleFix), transform.parent); // Crée un projectile
        }

        if (Input.GetButtonDown("Fire2")) // Si le bouton droit de la souris est enfoncé
        {
            if (_donneesPerso.bonus >= 1) // Si le personnage a un bonus de triple tir
            {
                Instantiate(_prefabProjecTriple, _perso.transform.position, Quaternion.Euler(0, 0, angleFix), transform.parent); // Crée un projectile
                Instantiate(_prefabProjecTriple, _perso.transform.position, Quaternion.Euler(0, 0, angleFix+_incrementation), transform.parent); // Crée un projectile
                Instantiate(_prefabProjecTriple, _perso.transform.position, Quaternion.Euler(0, 0, angleFix-_incrementation), transform.parent); // Crée un projectile
                _donneesPerso.bonus -= 1; // Enlève un bonus de triple tir
            }
        }

    }
}
