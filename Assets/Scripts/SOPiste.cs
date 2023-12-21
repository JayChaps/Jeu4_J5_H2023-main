using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "TIM/Piste musicale", fileName = "DonneesPiste")]

/// <summary>
/// #tp4
/// ScriptableObject des pistes musicales.
/// Permet de gérer les pistes musicales.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class SOPiste : ScriptableObject
{
    [SerializeField] TypePiste _type; //le type de piste
    [SerializeField] AudioClip _clip; //le clip à jouer
    [SerializeField] bool _estActifParDefaut; //permet de choisir l'état initial
    [SerializeField] bool _estActif; //c'est l'état actuel
    AudioSource _source; //la source audio qui jouera le clip
    public AudioSource source => _source; //la source audio qui jouera le clip
    public TypePiste type => _type; //le type de piste
    public AudioClip clip => _clip; //le clip à jouer
    public bool estActif //setter/getter de l'état actuel
    { 
        get => _estActif; 
        set 
        {
            _estActif = value;
            AjusterVolume();
        } 
    } 

    /// <summary>
    /// Fonction qui initialise l'audio source
    /// </summary>
    /// <param name="source">L'audio source en question</param>
    public void Initialiser(AudioSource source)
    {
        _source = source; //va chercher la source
        _source.clip = _clip; //va chercher l'audio clip
        _source.loop = true; //permet de faire une boucle
        _source.playOnAwake = false; //permet de ne pas jouer l'audio clip au début
        _source.Play(); //joue l'audio clip
        _estActif = _estActifParDefaut; //permet de choisir l'état initial
        AjusterVolume(); //permet d'ajuster le volume
    }

    /// <summary>
    /// Fonction qui ajuste le volume de la source audio
    /// </summary>
    public void AjusterVolume()
    {
        if(_estActif) _source.volume = GestAudio.instance.volumeMusiqueRef; //si l'audio source est active, on ajuste le volume
        else _source.volume = 0; //sinon on le met à 0
    }
}
