using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* A simple data container for the painting JSON object. */
public class PaintingJSONContainer {
    public string paintingName = "";
    public string artist = "";
    public string yearOfWork = "";
    public string category = "";
    public string imageName = "";

    private JSONObject parsedData;

    public PaintingJSONContainer(JSONObject parsedData) {
        Debug.Log("Hello");
        this.parsedData = parsedData;
        GetTopLevelFields();
        GetDescriptions();
        GetPieces();
    }

    // Store the description paragraphs and piece info objects
    public List<string> descriptions = new List<string>();
    public List<PieceJSON> pieces = new List<PieceJSON>();

    // Container for each piece 
    public class PieceJSON {
        public string pieceName;
        public string caption;
        public string imageName;
        public string description;

        public PieceJSON(string pieceName, string caption, string imageName, 
                         string description) {
            this.pieceName = pieceName;
            this.caption = caption;
            this.imageName = imageName;
            this.description = description;
        }
    }

    // Prints all stored data to the console
    public void PrintAllData() {
        Debug.Log("Hello I'm printing");
        Debug.Log("Painting Name - " + paintingName);
        Debug.Log("Painting Artist - " + artist);
        Debug.Log("Painting Year - " + yearOfWork);
        Debug.Log("Painting Category - " + category);
        Debug.Log("Painting Image Reference - " + imageName);

        foreach (string p in descriptions) {
            Debug.Log("Description Paragraph - " + p);
        }

        foreach(PieceJSON p in pieces) {
            Debug.Log("Piece Name - " + p.pieceName);
            Debug.Log("Piece Caption - " + p.caption);
            Debug.Log("Piece Image Reference - " + p.imageName);
            Debug.Log("Piece Description - " + p.description);
        }
    }

    // Collects and assigns top-level fields in the object
    private void GetTopLevelFields() {
        Debug.Log("Getting top level fields");
        if (parsedData.HasField("paintingName")) {
            paintingName = parsedData.GetField("paintingName").str;
        }

        if (parsedData.HasField("artist")) {
            artist = parsedData.GetField("artist").str;
        }

        if (parsedData.HasField("yearOfWork")) {
            yearOfWork = parsedData.GetField("yearOfWork").str;
        }

        if (parsedData.HasField("category")) {
            category = parsedData.GetField("category").str;
        }

        if (parsedData.HasField("imageName")) {
            imageName = parsedData.GetField("imageName").str;
        }
    }

    // Collects and assigns the top-level descriptions 
    private void GetDescriptions() {
        if (parsedData.HasField("descriptions")) {
            JSONObject paragrahps = parsedData.GetField("descriptions");
            for (int i = 0; i < paragrahps.Count; i++)
            {
                string descriptionParagraph = paragrahps.list[i].str;
                descriptions.Add(descriptionParagraph);
            }
        }
    }

    // Collects and assigns the information for each piece
    private void GetPieces() {
        if (parsedData.HasField("pieces")) {
            JSONObject piecesList = parsedData.GetField("pieces");
            for (int i = 0; i < piecesList.Count; i++) {
                JSONObject piece = piecesList.list[i];
                GetPiece(piece);
            }
        }
    }

    // Collects information from given piece and assigns to list
    private void GetPiece(JSONObject piece) {
        string pieceName = "";
        string pieceCaption = "";
        string pieceImageName = "";
        string pieceDescription = "";

        if (piece.HasField("pieceName")) {
            pieceName = piece.GetField("pieceName").str;
        }

        if (piece.HasField("caption")) {
            pieceCaption = piece.GetField("pieceCaption").str;
        }

        if (piece.HasField("imageName")) {
            pieceImageName = piece.GetField("imageName").str;
        }

        if (piece.HasField("description")) {
            pieceDescription = piece.GetField("description").str;
        }

        PieceJSON newPiece = new PieceJSON(pieceName, 
                                           pieceCaption, 
                                           pieceImageName, 
                                           pieceDescription);
        pieces.Add(newPiece);
    }
}
