using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

/* A simple data container for the painting JSON object. */
[Serializable]
public class JSONContainer {
    public string paintingName;
    public string artist;
    public string yearOfWork;
    public string category;
    public string imageName;

    // Store the description paragraphs and piece info objects
    public List<ParagraphJSON> descriptions = new List<ParagraphJSON>();
    public List<PieceJSON> pieces = new List<PieceJSON>();

    [Serializable]
    // Container for each bio description paragraph
    public class ParagraphJSON {
        public string paragraph;
    }

    [Serializable]
    // Container for each piece 
    public class PieceJSON {
        public string pieceName;
        public string caption;
        public string imageName;
        public string description;
    }

    // Prints all stored data to the console
    //public void PrintAllData() {
    //    Debug.Log("Hello I'm printing");
    //    Debug.Log("Painting Name - " + paintingName);
    //    Debug.Log("Painting Artist - " + artist);
    //    Debug.Log("Painting Year - " + yearOfWork);
    //    Debug.Log("Painting Category - " + category);
    //    Debug.Log("Painting Image Reference - " + imageName);

    //    foreach (ParagraphJSON p in descriptions) {
    //        Debug.Log("Description Paragraph - " + p.paragraph);
    //    }

    //    foreach(PieceJSON p in pieces) {
    //        Debug.Log("Piece Name - " + p.pieceName);
    //        Debug.Log("Piece Caption - " + p.caption);
    //        Debug.Log("Piece Image Reference - " + p.imageName);
    //        Debug.Log("Piece Description - " + p.description);
    //    }
    //}
}
