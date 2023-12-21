using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using System.Runtime.InteropServices;

[CreateAssetMenu(fileName = "Sauvegarde", menuName = "TIM/Sauvegarde")]
/// <summary>
/// #tp4
/// Scriptable object de la sauvegarde.
/// Permet d'écrire et de lire un fichier de sauvegarde.
/// Auteur du code : Jeremy Chaput
/// Auteur des commentaires : Jeremy Chaput
/// </summary>
public class SOSauvegarde : ScriptableObject
{
    [SerializeField] string _nom; //nom du joueur
    [SerializeField] float _score; //score du joueur
    [SerializeField] List<NomScore> _listeNomScore = new List<NomScore>(); //liste qui va recevoir le nom et le score des éventuels joueurs
    int _index = 0; //index de la liste
    
    [DllImport("__Internal")]
    static extern void SynchroniserWebGL();
    string _fichier = "data.tim";

    public List<NomScore> listeNomScore //setter/getter de la liste
    { 
        get => _listeNomScore; 
        set
        {
            _listeNomScore.Sort((x, y) => y.score.CompareTo(x.score)); //classe la liste en ordre 
            _listeNomScore = value;
        }   
    }

    [ContextMenu("Lire fichier")]
    /// <summary>
    /// Lit les informations du fichier de sauvegarde.
    /// </summary>
    public void LireFichier()
    {
        string fichierEtChemin = Application.persistentDataPath + "/" + _fichier;
        if (File.Exists(fichierEtChemin))
        {
            string contenu = File.ReadAllText(fichierEtChemin);
            JsonUtility.FromJsonOverwrite(contenu, this);

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
        }
    }

    [ContextMenu("Ecrire fichier")]
    /// <summary>
    /// Écrit les informations dans le fichier de sauvegarde.
    /// </summary>
    public void EcrireFichier()
    {
        string fichierEtChemin = Application.persistentDataPath + "/" + _fichier;
        string contenu = JsonUtility.ToJson(this, true);
        File.WriteAllText(fichierEtChemin, contenu);

        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            SynchroniserWebGL();
        }
    }

    /// <summary>
    /// Fonction permettant d'ajouter un nom et un score à la liste.
    /// </summary>
    /// <param name="nom">Le nom du joueur</param>
    /// <param name="score">Le score du joueur</param>
    public void AjouterNomScore(string nom, float score)
    {
        listeNomScore.Add(new NomScore { nom = nom, score = score }); // ajoute le nom et le score à la liste
        listeNomScore[_index].nom = nom; //ajoute le nom à la liste à l'index donné
        listeNomScore[_index].score = score; //ajoute le score à la liste à l'index donné
    }

    /// <summary>
    /// Fonction permettant de vider la liste
    /// à l'aide d'un bouton caché dans le coin inférieur droit de l'écran de fin.
    /// Je laisse le bouton à des fins de débogage.
    /// </summary>
    public void ViderListe()
    {
        _listeNomScore.Clear();
    }
}

[System.Serializable]
public class NomScore
{
    public string nom;
    public float score;
}