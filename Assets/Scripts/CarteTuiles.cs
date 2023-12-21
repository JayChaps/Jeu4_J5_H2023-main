using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Permet de déterminer la probabilité de chaque CarteTuile.
/// Permet de changer l'alpha des CarteTuiles.
/// Permet de transferer toutes les CarteTuiles sur la Tilemap du niveau.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class CarteTuiles : MonoBehaviour
{
    [SerializeField][Range(0,100)] private float _probabilite = 100; // Slider permettant de déterminer la probabilité de chaque CarteTuile
    [SerializeField] public float probabilite => _probabilite;
    private Tilemap tm;
    private float aleaProb;

    /// <summary>
    /// Awake est appelée lors du changement de l'instance du script
    /// </summary>
    void Awake()
    {
        tm = GetComponent<Tilemap>();
        BoundsInt bounds = tm.cellBounds;
        aleaProb = Random.Range(0,100);
        Niveau niveau = GetComponentInParent<Niveau>();
        Vector3Int decalage = Vector3Int.FloorToInt(transform.position);
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TraiterUneTuile(tm, niveau, pos, decalage);
            }
        }
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tm"></param>
    /// <param name="niveau"></param>
    /// <param name="pos"></param>
    /// <param name="decalage"></param>
    void TraiterUneTuile(Tilemap tm, Niveau niveau, Vector3Int pos, Vector3Int decalage)
    {
        TileBase tuile = tm.GetTile(pos);

        if (probabilite >= aleaProb && tuile != null)
        {
            niveau.tilemap.SetTile(pos + decalage, tuile);
        }
        else
        {
            tm.SetTile(pos, null);
        }
    }

    void ChercherTilemap()
    {
        tm = GetComponent<Tilemap>();
    }

    /// <summary>
    /// Permet de dessiner des gizmos sur la scène
    /// </summary>
    void OnDrawGizmos()
    {
        if(tm == null) ChercherTilemap();
        float alphaTuile = probabilite/100f; // Détermine l'alpha de la tuile selon sa probabilité.
        if(!Application.isPlaying) tm.color = new Color (1, 1, 1, alphaTuile); // Change l'alpha de la tuile si l'application n'est pas en mode lecture.
        else if(Application.isPlaying) tm.color = new Color (1, 1, 1, 1);
    }
}