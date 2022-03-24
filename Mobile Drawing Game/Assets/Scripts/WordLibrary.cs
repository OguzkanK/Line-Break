using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class WordLibrary
{
    public List<List<string>> GetCategoriesList()
    {
        return CategoriesList;
    }
    
    private List<List<string>> CategoriesList = new List<List<string>>()
    {
        AnimalsList,
        VehiclesList,
        //FlagsList,
        ProfessionsList,
        FruitsAndVeggiesList,
        FurnituresList
    };
    
    private static readonly List<string> AnimalsList = new List<string>()
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
        "Macaw",
        "Deer",
        "Goose"
        
    };
    
    private static readonly List<string> VehiclesList = new List<string>()
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
    
    private static readonly List<string> FlagsList = new List<string>()
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
        "Hungary",
        "India"
    };
    
    private static readonly List<string> ProfessionsList = new List<string>()
    {
        "Pilot",
        "Babysitter",
        "Baker",
        "Engineer",
        "Cashier",
        "Dentist",
        "Electrician",
        "Barber",
        "Cook",
        "Farmer",
        "Photographer",
        "Artist"
    };
    
    private static readonly List<string> FruitsAndVeggiesList = new List<string>()
    {
        "Banana",
        "Cucumber",
        "Eggplant",
        "Lemon",
        "Watermelon",
        "Pineapple",
        "Pepper",
        "Pomegranate",
        "Spinach",
        "Onion",
        "Coconut",
        "Tomato",
        "Carrot"
    };
    
    private static readonly List<string> FurnituresList = new List<string>()
    {
        "Couch",
        "Bed",
        "Chair",
        "Grandfather Clock",
        "Mirror",
        "Table",
        "Television",
        "Carpet",
        "Cradle",
        "Bookcase"
    };
}