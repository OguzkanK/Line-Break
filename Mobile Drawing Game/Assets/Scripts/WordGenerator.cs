using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordGenerator : MonoBehaviour
{
    private WordLibrary _wordLibrary= new WordLibrary();
    
    public void generateWord()
    {
        List<String> selectedWordList = new List<string>();
        List<String> selectedCategory = GETWordList();
        for(int i = 0; i < 3; i++)
        {
            int randomizedIndex = Random.Range(0, selectedCategory.Count - 1);
            selectedWordList.Add(selectedCategory[randomizedIndex]);
            selectedCategory.RemoveAt(randomizedIndex);
        }
    }
    
    public List<String> GETWordList()
    {
        List<List<String>> CategoryList = _wordLibrary.CategoriesList;
        int randomizedIndex = Random.Range(0, CategoryList.Count - 1);
        List<String> selectedCategory = CategoryList[randomizedIndex];
        return selectedCategory;
    }
}
