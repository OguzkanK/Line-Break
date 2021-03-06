using System;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordRandomizer : MonoBehaviour
{
    private readonly WordLibrary _wordLibrary= new WordLibrary();
    public TMP_Text selectedWordDisplay;
    public GameObject selectedWordDisplayGameObject;
    public TurnManager turnManager;
    public PhotonView view;

    private void Start()
    {
        if(PhotonNetwork.LocalPlayer.IsMasterClient){
            GenerateWord();
        }
    }

    public void GenerateWord()
    {
        List<string> selectedCategory = GETWordList();
        int randomizedIndex = Random.Range(0, selectedCategory.Count - 1);
        view.RPC("PrepareSelectedWordDisplay", RpcTarget.AllBuffered, selectedCategory[randomizedIndex]);
    }

    [PunRPC]
    public void PrepareSelectedWordDisplay(string wordToSet)
    {
        selectedWordDisplay.text = wordToSet;
        if(turnManager.isDrawer) selectedWordDisplayGameObject.SetActive(true);
    }
    
    private List<string> GETWordList()
    {
        List<List<string>> categoryList = _wordLibrary.GetCategoriesList();
        int randomizedIndex = Random.Range(0, categoryList.Count - 1);
        List<String> selectedCategory = categoryList[randomizedIndex];
        return selectedCategory;
    }
}
