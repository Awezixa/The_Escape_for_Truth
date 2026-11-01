//using System;
//using System.Collections.Generic;

//[Serializable]
//public class DocField
//{
//    public string key;
//    public string value;
//}

//[Serializable]
//public class DocumentRecord
//{
//    public string title;
//    public List<DocField> fields = new List<DocField>();

//    public string Get(string key)
//    {
//        foreach (var f in fields)
//            if (f.key == key) return f.value;
//        return null;
//    }
//}

//public class Case
//{
//    public List<DocumentRecord> documents = new List<DocumentRecord>();
//    public bool shouldApprove;
//}