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
    public string description = "";

    private JSONObject parsedData;

    public PaintingJSONContainer(JSONObject parsedData) {
        this.parsedData = parsedData;
        GetTopLevelFields();
        GetDescriptions();
        GetPieces();
    }

    // Store the piece objects
    public List<PieceJSON> pieces = new List<PieceJSON>();

    // Container for each piece 
    public class PieceJSON {
        public string pieceName;
        public string caption;
        public string imageName;
        public string description;

        public PieceJSON(string pieceName, string caption, string imageName, string description) {
            this.pieceName = pieceName;
            this.caption = caption;
            this.imageName = imageName;
            this.description = description;
        }
    }

    // Prints all stored data to the console
    public void PrintAllData() {
        Debug.Log("Painting Name - " + paintingName);
        Debug.Log("Painting Artist - " + artist);
        Debug.Log("Painting Year - " + yearOfWork);
        Debug.Log("Painting Category - " + category);
        Debug.Log("Painting Image Reference - " + imageName);
        Debug.Log("Description - " + description);

        foreach(PieceJSON p in pieces) {
            Debug.Log("Piece Name - " + p.pieceName);
            Debug.Log("Piece Caption - " + p.caption);
            Debug.Log("Piece Image Reference - " + p.imageName);
            Debug.Log("Piece Description - " + p.description);
        }
    }

    // Collects and assigns top-level fields in the object
    private void GetTopLevelFields() {
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

    // Gets the description paragraphs
    private void GetDescriptions() {
        if (parsedData.HasField("descriptions"))
        {
            List<string> stringParagraphs = new List<string>();
            JSONObject JSONParagraphs = parsedData.GetField("descriptions");
            for (int i = 0; i < JSONParagraphs.Count; i++)
            {
                JSONObject innerObject = JSONParagraphs.list[i];
                string currentParagraph = "";
                if (innerObject.HasField("paragraph")) {
                    currentParagraph = innerObject.GetField("paragraph").str;
                }
                stringParagraphs.Add(currentParagraph);
            }

            // Combine all paragraphs into one mass of text with proper paragraphing
            foreach (string s in stringParagraphs) {
                description += s + Environment.NewLine + Environment.NewLine;
            }
        }
    }

    // Collects and assigns the information for each piece
    private void GetPieces() {
        if (parsedData.HasField("pieces")) {
            JSONObject piecesList = parsedData.GetField("pieces");
            foreach (JSONObject j in piecesList.list) {
                GetPiece(j);
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
            pieceCaption = piece.GetField("caption").str;
        }

        if (piece.HasField("imageName")) {
            pieceImageName = piece.GetField("imageName").str;
        }

        if (piece.HasField("description")) {
            pieceDescription = piece.GetField("description").str;
        }

        PieceJSON newPiece = new PieceJSON(pieceName, pieceCaption, pieceImageName, pieceDescription);
        pieces.Add(newPiece);
    }
}
