using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp3
/// Classe qui permet de gérer le contenu d'une salle.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class Salle : MonoBehaviour
{
    static Vector2Int _taille = new Vector2Int(32,18); //taille de la salle
    static public Vector2Int taille => _taille; //getter de la taille
    [SerializeField] public Transform _cleRepere; //référence pour la position de la clé
    [SerializeField] public Transform _porteRepere; //référence pour la position de la porte
    [SerializeField] public Transform _activateurRepere; //référence pour la position de l'activateur
    [SerializeField] public Transform _persoRepere; //référence pour la position du perso
    [SerializeField] public bool _drawGizmo = true; //dessine le gizmo de la salle

    /// <summary>
    /// Est appelé à chaque frame, même quand l'application est en pause.
    /// </summary>
    private void OnDrawGizmos()
    {
        if(_drawGizmo) Gizmos.DrawWireCube(transform.position, new Vector3(_taille.x, _taille.y, 0)); //dessine la salle 
    }

    /// <summary>
    /// Va chercher la position de la porte et la retourne.
    /// </summary>
    /// <param name="modele">Le prefab de la porte</param>
    /// <returns></returns>
    public Vector2Int ChercherPosPorte(GameObject modele)
    {
        Vector3 pos = _porteRepere.position;
        GameObject objet = Instantiate(modele, pos, Quaternion.identity, transform.parent);
        objet.name = modele.name;
        return Vector2Int.FloorToInt(pos);
    }

    /// <summary>
    /// Va chercher la position de la clé et la retourne.
    /// </summary>
    /// <param name="modele">Le prefab de la clé</param>
    /// <returns></returns>
    public Vector2Int ChercherPosCle(GameObject modele)
    {
        Vector3 pos = _cleRepere.position;
        GameObject objet = Instantiate(modele, pos, Quaternion.identity, transform.parent);
        objet.name = modele.name;
        return Vector2Int.FloorToInt(pos);
    }
    
    /// <summary>
    /// Va chercher la position de l'activateur et la retourne.
    /// </summary>
    /// <param name="modele">Le prefab de l'activateur</param>
    /// <returns></returns>
    public Vector2Int ChercherPosActivateur(GameObject modele)
    {
        Vector3 pos = _activateurRepere.position;
        GameObject objet = Instantiate(modele, pos, Quaternion.identity, transform.parent);
        objet.name = modele.name;
        return Vector2Int.FloorToInt(pos);
    }

    /// <summary>
    /// Va chercher la position du perso et la retourne.
    /// </summary>
    /// <param name="modele">Le gameObject du perso</param>
    /// <returns></returns>
    public Vector2Int ChercherPosPerso(GameObject modele)
    {
        Vector3 pos = _persoRepere.position;
        modele.transform.position = pos;
        return Vector2Int.FloorToInt(pos);
    }
}
