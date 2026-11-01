//using System.Collections.Generic;
//using UnityEngine;

//public static class CaseValidator
//{
//    // Any key that appears on two documents with different values is a discrepancy
//    public static List<string> FindMismatchedKeys(Case c)
//    {
//        var result = new List<string>();
//        for (int i = 0; i < c.documents.Count; i++)
//            for (int j = i + 1; j < c.documents.Count; j++)
//                foreach (var f in c.documents[i].fields)
//                {
//                    string other = c.documents[j].Get(f.key);
//                    if (other != null && other != f.value && !result.Contains(f.key))
//                        result.Add(f.key);
//                }
//        return result;
//    }
//}
