using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #synthese
/// Classe de la barre de vie.
/// Peut être utilisé pour n'importe quel gameobject.
/// </summary>
public class BarreDeVie : MonoBehaviour
{
    public void MiseAJourVie(float vie, float vieMax)
    {
        float pourcentageVie = vie / vieMax;
        Vector3 echelle = transform.localScale;
        echelle.x = pourcentageVie;
        transform.localScale = echelle;
    }
}
