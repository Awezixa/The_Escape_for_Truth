using System.Collections.Generic;
using UnityEngine;

public class DocumentGenerator : MonoBehaviour
{
    
    // a document you write yourself, shown exactly as typed
    [System.Serializable]
    public class HandwrittenDocument
    {
        public string name;
        public string address;
        public string city;
        public bool isValid = true;
        public EDocumentType type = EDocumentType.Real;
    }

    //lists for info to go on docs, need figure out what to put on. change lists as well
    public List<string> firstNames = new List<string> { "Anna", "Tomas", "Mira", "Jonas" };
    public List<string> surnames = new List<string> { "Weiss", "Reyes", "Okafor", "Berg" };
    public List<string> streetFirstWords = new List<string> { "Maple", "Harbour", "Station" };
    public List<string> streetLastWords = new List<string> { "Street", "Road", "Lane" };
    public List<string> cities = new List<string> { "Aldport", "Brightmoor", "Carrow" };
    public List<string> fakeCities = new List<string> { "Zzyzx", "Nowhereton" };

    //if want to generate a hand written note 
    public List<HandwrittenDocument> handwritten = new List<HandwrittenDocument>();
    [Range(0f, 1f)] public float handwrittenChance = 0.3f;   // chance of picking one of yours

    //chance of getting a fake doc
    [Range(0f, 1f)] public float baseFlawChance = 0.35f;     // chance a random document is flawed
    public float flawChancePerLevel = 0.05f;

    int difficulty = 0;
    //things can add and change to make fake docs
    enum Flaw { FakeStamp, PrintedStamp, MissingName, MissingAddress, FakeCity }

    public void SetDifficulty(int newDifficulty) => difficulty = newDifficulty;

    public void Generate(DocumentView doc)
    {
        // 1. sometimes use one you wrote yourself
        if (handwritten.Count > 0 && Random.value < handwrittenChance)
        {
            HandwrittenDocument h = handwritten[Random.Range(0, handwritten.Count)];
            doc.Initialise(h.name + "\n" + h.address + "\n" + h.city, h.isValid, h.type);
            return;
        }

        // 2. otherwise build a random, correct document
        string firstName = Pick(firstNames);
        string surname = Pick(surnames);
        string houseNumber = Random.Range(1, 420).ToString();
        string streetFirst = Pick(streetFirstWords);
        string streetLast = Pick(streetLastWords);
        string city = Pick(cities);
        EDocumentType type = EDocumentType.Real;
        bool valid = true;

        // 3. sometimes break it. Harder flaws unlock as difficulty rises.
        float flawChance = baseFlawChance + flawChancePerLevel * difficulty;
        if (Random.value < flawChance)
        {
            valid = false;
            int unlocked = Mathf.Min(5, 1 + difficulty);   // difficulty 0 = only FakeStamp
            Flaw flaw = (Flaw)Random.Range(0, unlocked);

            switch (flaw)
            {
                case Flaw.FakeStamp:
                    type = EDocumentType.Fake;
                    break;
                case Flaw.PrintedStamp:
                    type = EDocumentType.Printed;
                    break;
                case Flaw.MissingName:
                    if (Random.value < 0.5f) firstName = ""; else surname = "";
                    break;
                case Flaw.MissingAddress:
                    int roll = Random.Range(0, 3);
                    if (roll == 0) houseNumber = "";
                    else if (roll == 1) streetFirst = "";
                    else streetLast = "";
                    break;
                case Flaw.FakeCity:
                    city = Pick(fakeCities);
                    break;
            }
        }

        // 4. assemble the text, tidying any gaps left by removed parts
        string fullName = (firstName + " " + surname).Trim();
        string street = (houseNumber + " " + streetFirst + " " + streetLast).Replace("  ", " ").Trim();

        doc.Initialise(fullName + "\n" + street + "\n" + city, valid, type);
        Debug.Log("Generated: " + fullName + " | " + street + " | " + city + " | valid = " + valid);
    }

    string Pick(List<string> list)
    {
        if (list == null || list.Count == 0) return "";
        return list[Random.Range(0, list.Count)];
    }
}
