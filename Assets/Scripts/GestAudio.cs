using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// #tp4
/// Classe qui permet de gérer le gestionnaire audio.
/// Permet de faire jouer les sons et les pistes musicales.
/// Auteur du code: Jeremy Chaput
/// Auteur des commentaires: Jeremy Chaput
/// </summary>
public class GestAudio : MonoBehaviour
{
    [SerializeField][Range(0.5f,1f)] float _pitchMinEffetSonore = 0.8f; //pitch minimum de l'effet sonore
    [SerializeField][Range(1f,2f)] float _pitchMaxEffetSonore = 1.2f; //pitch maximum de l'effet sonore
    [SerializeField] float _volumeMusiqueRef = 1f; //volume de référence des pistes musicales
    public float volumeMusiqueRef => _volumeMusiqueRef; //getter du volume de référence des pistes musicales
    [SerializeField] SOPiste[] _tPistes; //tableau des pistes musicales
    public SOPiste[] tPistes => _tPistes; //getter du tableau des pistes musicales
    AudioSource _sourceEffetsSonores; //source audio pour les effets sonores
    static GestAudio _instance; //instance du gestionnaire audio
    static public GestAudio instance => _instance; //getter de l'instance du gestionnaire audio

    /// <summary>
    /// Awake est appelé avant la première frame
    /// </summary>
    void Awake()
    {
        if (_instance == null) _instance = this; //détruit le gestionnaire audio s'il existe déjà
        else
        {
            Debug.Log("Un gestionnaire audio existe déjà, donc celui sur la scène sera détruit");
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); //permet de ne pas détruire le gestionnaire audio lors du changement de scène
        _sourceEffetsSonores = gameObject.AddComponent<AudioSource>(); //ajoute une source audio pour les effets sonores
        CreerLesSourcesMusicales(); //crée les sources musicales
    }

    /// <summary>
    /// Crée les sources pour faire jouer les musiques
    /// </summary>
    void CreerLesSourcesMusicales()
    {
        foreach (SOPiste piste in _tPistes) //crée les sources pour chaque piste
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            piste.Initialiser(source);
        }
    }

    /// <summary>
    /// Fait jouer un son
    /// </summary>
    /// <param name="clip">Le clip audio</param>
    public void JouerEffetSonore(AudioClip clip)
    {
        _sourceEffetsSonores.pitch = Random.Range(_pitchMinEffetSonore, _pitchMaxEffetSonore); //donne un pitch aléatoire au son
        _sourceEffetsSonores.PlayOneShot(clip);
    }

    /// <summary>
    /// Permet de changer le volume.
    /// </summary>
    /// <param name="volume">Le volume cible</param>
    public void ChangerVolume(float volume)
    {
        StopAllCoroutines();
        foreach (SOPiste piste in _tPistes)
        {
            if(piste.estActif) StartCoroutine(CoroutineChangerVolume(piste, volume, 1f));
        }
    }

    /// <summary>
    /// Permet de changer le volume graduellement.
    /// </summary>
    /// <param name="piste">La piste ciblée</param>
    /// <param name="volumeFinal">Le volume ciblé</param>
    /// <param name="duree">La durée de la transition graduelle</param>
    /// <returns></returns>
    IEnumerator CoroutineChangerVolume(SOPiste piste, float volumeFinal, float duree)
    {
        _volumeMusiqueRef = volumeFinal;
        float volumeInitial = piste.source.volume;
        float tempsInitial = Time.time;
        float tempsActuel = tempsInitial;
        float tempsFinal = tempsInitial + duree;
        while (tempsActuel < tempsFinal)
        {
            tempsActuel = Time.time;
            float pourcentage = (tempsActuel - tempsInitial) / duree;
            float nouvVolume = Mathf.Lerp(volumeInitial, volumeFinal, pourcentage);
            piste.source.volume = nouvVolume;
            yield return null;
        }
    }

    /// <summary>
    /// Change l'état d'une piste audio pour la faire jouer ou la faire arrêter
    /// </summary>
    /// <param name="type">Le type de la piste</param>
    /// <param name="estActif">Si elle est active ou non</param>
    public void ChangerEtatLecturePiste(TypePiste type, bool estActif)
    {
        foreach (SOPiste piste in _tPistes)
        {
            if (piste.type == type)
            {
                piste.estActif = estActif;
                return; // comme ça, si y'en a 1000, mais qu'il trouve à 20, il arrête de chercher
            }
        }
    }

}
