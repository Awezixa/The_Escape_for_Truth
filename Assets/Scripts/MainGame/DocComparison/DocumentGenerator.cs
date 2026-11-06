using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;

public class DocumentGenerator : MonoBehaviour
{
    //lists for infor to go on docs, need figure out what to put on
    List<string> firstNames = new List<string>{ "words of wisdom", "yes" };

    List<string> surnames = new List<string>{ "THis magical state", "new text"};

    List<string> streetLastWords = new List<string> {"blank", "new text" };

    List<string> streetFirstWords = new List<string> { "yep", "new text" };

    List<string> cities = new List<string> { "ok", "new text" };

    List<string> fakeCities = new List<string> {"bad", "new text" };

    int difficulty = 0;
    public float z = 0;

    const int baseFakeValue = 6;
    public void SetDifficulty(int newDifficulty)
    {
        difficulty = newDifficulty;
    }

    //random text gen for the docs, need to update and tweak with clanker
    public void Generate(Document documentComponent)
    {
        string firstName = PickRandomFromList(firstNames) + " ";
        string surname = PickRandomFromList(surnames);
        string houseNumber = Mathf.RoundToInt(Random.Range(1, 420)).ToString() + " ";
        string streetFirstWord = PickRandomFromList(streetFirstWords) + " ";
        string streetLastWord = PickRandomFromList(streetLastWords);
        string city = PickRandomFromList(cities);

        EDocumentType type = Random.Range(0f, 1f) >= .65f ? EDocumentType.Real : EDocumentType.Fake;

        bool isLetterValid = true;

        int value = Random.Range(1, baseFakeValue + 1 + difficulty);

        //first couple letters guaranteed to be right
        //if (DocumentManager.instance.scorePennies < 3 && DocumentManager.instance.scorePounds < 1)
        //    value = 0;

        //10 percent chance of an evil letter if the letter is incorrect, overall 1/100 letters are evil
        int evilLetter = Mathf.RoundToInt(Random.Range(1, 10));

        //generate incorrect letter
        if (value >= baseFakeValue)
        {
            isLetterValid = false;

            //if (evilLetter == 1)
            //{
            //letter = MakeEvilLetter();
            //return;
            //}

            switch (value)
            {
                case baseFakeValue:
                    //no stamp
                    type = EDocumentType.Fake;
                    break;
                case baseFakeValue + 1:
                    type = Random.Range(0f, 1f) >= .5f ? EDocumentType.Fake : EDocumentType.Printed;
                    //todo
                    break;
                case baseFakeValue + 2:
                    //wrong name
                    {
                        int roll = Random.Range(0, 2);
                        if (roll == 0)
                            firstName = "";
                        else
                            surname = "";
                        break;
                    }
                case baseFakeValue + 3:
                    //wrong address
                    {
                        int roll = Random.Range(0, 3);
                        if (roll == 0)
                            houseNumber = "";
                        else if (roll == 1)
                            streetFirstWord = "";
                        else
                            streetLastWord = "";
                        break;
                    }
                case baseFakeValue + 4:
                    //fake city
                    city = PickRandomFromList(fakeCities);
                    break;
            }

        }

        string information = firstName + surname + "\n" + houseNumber + streetFirstWord + streetLastWord + "\n" + city;
        //documentComponent.Initialise(address, isLetterValid, type, z);
        z += .2f;
        if (z > 8.5)
            z = 0;
    }

    //Letter MakeEvilLetter()
    //{
    //	return new Letter();
    //}

    string PickRandomFromList(List<string> list)
    {
        int index = Mathf.RoundToInt(Random.Range(0, list.Count));

        return list[index];
    }
}
