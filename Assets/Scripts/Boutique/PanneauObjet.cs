using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class PanneauObjet : MonoBehaviour
{
    [Header("LES DONNÉES")]
    [SerializeField] SOObjet _donneesObj;
    public SOObjet donneesObj => _donneesObj;

    [Header("LES CONTENEURS")]
    [SerializeField] TextMeshProUGUI _champNom;
    [SerializeField] TextMeshProUGUI _champPrix;
    [SerializeField] TextMeshProUGUI _champDescription;
    [SerializeField] Image _image;
    [SerializeField] CanvasGroup _canvasGroup;

    void Start()
    {
        MettreAJourInfos();
        Boutique.instance.donneesPerso.evenementMiseAJour.AddListener(MettreAJourInfos);
    }

    private void MettreAJourInfos()
    {
        _champNom.text = _donneesObj.nom;
        _champPrix.text = _donneesObj.prix + " $";
        _champDescription.text = _donneesObj.description;
        _image.sprite = _donneesObj.sprite;
        GererDispo();
    }

    void GererDispo()
    {
        bool aAssezArgent = Boutique.instance.donneesPerso.argent >= _donneesObj.prix;
        if (aAssezArgent)
        {
            RendreDisponible();
            if(_donneesObj.permetDeCourir && Boutique.instance.donneesPerso.possedeSandales)
            {
                RendreIndisponible();
            }
        }
        else
        {
            RendreIndisponible();
        }
    }

    void RendreDisponible()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.alpha = 1;
    }

    void RendreIndisponible()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.alpha = 0.5f;
    }

    public void Acheter()
    {
        Boutique.instance.donneesPerso.Acheter(_donneesObj);
        MettreAJourInfos();
    }
}