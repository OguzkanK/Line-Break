using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WordLibrary
{
    public List<List<String>> CategoriesList = new List<List<String>>()
    {
        AnimalsList,
        VehiclesList,
        FlagsList
    };
    
    private static readonly List<String> AnimalsList = new List<String>()
    {
        "Turtle",
        "Lion",
        "Cat",
        "Mouse",
        "Dog",
        "Horse",
        "Penguin",
        "Elephant",
        "Zebra",
        "Rhinoceros",
        "Snake",
        "Macaw"
        
    };
    
    private static readonly List<String> VehiclesList = new List<String>()
    {
        "Car",
        "Bicycle",
        "Tractor",
        "Taxi",
        "Bus",
        "Ambulance",
        "Airplane",
        "Helicopter",
        "Train",
        "Cable Car",
        "Skateboard",
        "Rowboat"
    };
    
    private static readonly List<String> FlagsList = new List<String>()
    {
        "Canada",
        "Argentina",
        "Germany",
        "Japan",
        "South Korea",
        "China",
        "France",
        "Italy",
        "Turkey",
        "Greece",
        "Bulgaria",
        "Hungary"
    };
}
