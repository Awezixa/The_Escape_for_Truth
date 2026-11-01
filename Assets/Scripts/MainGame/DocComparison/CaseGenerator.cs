//using System.Collections.Generic;
//using UnityEngine;

//public static class CaseGenerator
//{
//    static readonly string[] names = { "Anna Weiss", "Tomas Reyes", "Mira Okafor", "Jonas Berg" };
//    static readonly string[] sharedKeys = { "Name", "ID", "DOB" };   // fields that appear on BOTH documents

//    public static Case Create()
//    {
//        string name = names[Random.Range(0, names.Length)];
//        string id = Random.Range(10000, 100000).ToString();
//        string dob = Random.Range(1, 29).ToString("00") + "/" + Random.Range(1, 13).ToString("00") + "/1985";

//        var idCard = new DocumentRecord { title = "ID Card" };
//        idCard.fields.Add(new DocField { key = "Name", value = name });
//        idCard.fields.Add(new DocField { key = "ID", value = id });
//        idCard.fields.Add(new DocField { key = "DOB", value = dob });

//        var permit = new DocumentRecord { title = "Work Permit" };
//        permit.fields.Add(new DocField { key = "Name", value = name });
//        permit.fields.Add(new DocField { key = "ID", value = id });
//        permit.fields.Add(new DocField { key = "DOB", value = dob });
//        permit.fields.Add(new DocField { key = "Expiry", value = "12/12/2027" });

//        var c = new Case();
//        c.documents.Add(idCard);
//        c.documents.Add(permit);

//        // half the time, corrupt one shared field on one document
//        if (Random.value < 0.5f)
//        {
//            var doc = c.documents[Random.Range(0, c.documents.Count)];
//            string key = sharedKeys[Random.Range(0, sharedKeys.Length)];
//            foreach (var f in doc.fields)
//                if (f.key == key) f.value = Corrupt(f.value);
//        }

//        // the truth comes from the validator, so it's always consistent
//        c.shouldApprove = CaseValidator.FindMismatchedKeys(c).Count == 0;
//        return c;
//    }

//    // changes the last character slightly (digits wrap 9 -> 0, letters go up one)
//    static string Corrupt(string value)
//    {
//        char last = value[value.Length - 1];
//        char changed = char.IsDigit(last)
//            ? (char)('0' + (last - '0' + 1) % 10)
//            : (char)(last + 1);
//        return value.Substring(0, value.Length - 1) + changed;
//    }
//}