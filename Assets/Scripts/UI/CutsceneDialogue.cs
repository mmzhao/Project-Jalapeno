using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneDialogue : MonoBehaviour {

    [Serializable]
    public struct NamedSprite
    {
        public string name;
        public Sprite image;
    }

    public TextAsset dialogueFile;
    public NamedSprite[] sprites;
    public Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    private int currentLine = 0;
    string[] leftSprites;
    string[] rightSprites;
    string[] speakers;
    string[] dialogueLines;
    public PlayerController pc;

    public Text dialogue;
    public Text speaker;
    public Image leftSprite;
    public Image rightSprite;

    void Awake () {
        foreach (NamedSprite sprite in sprites)
        {
            spriteDict.Add(sprite.name, sprite.image);
        }

        string[] lines = dialogueFile.text.Split('\n');

        leftSprites = new string[lines.Length];
        rightSprites = new string[lines.Length];
        speakers = new string[lines.Length];
        dialogueLines = new string[lines.Length];

        for (int i = 0; i < lines.Length; i++) {
            string[] components = lines[i].Split(new char[] { '/', ':' });

            leftSprites[i] = components[0];
            rightSprites[i] = components[1];
            speakers[i] = components[2];
            dialogueLines[i] = components[3];
        }

    }

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp("Attack1"))
        {
            UpdateDialogue();
        }
	}
    
    public void UpdateDialogue ()
    {
        if (currentLine == 0)
        {
            pc.enabled = false; //don't register any inputs
            Time.timeScale = 0; // ZA WARUDO! TOKI WO TOMARE!
        }
        else if (currentLine == dialogueLines.Length)
        {
            pc.enabled = true;
            Time.timeScale = 1; // Resume time
            gameObject.SetActive(false);
            return;
        }

        dialogue.text = dialogueLines[currentLine];
        speaker.text = speakers[currentLine];

        if (!leftSprites[currentLine].Contains("*"))
        {
            leftSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        }
        else
        {
            leftSprites[currentLine] = leftSprites[currentLine].Substring(1, leftSprites[currentLine].Length - 2);
            leftSprite.color = new Color(1, 1, 1, 1);
        }

        if (!rightSprites[currentLine].Contains("*"))
        {
            rightSprite.color = new Color(0.5f, 0.5f, 0.5f, 0.8f);
        }
        else
        {
            rightSprites[currentLine] = rightSprites[currentLine].Substring(1, rightSprites[currentLine].Length - 2);
            rightSprite.color = new Color(1, 1, 1, 1);
        }

        Sprite sp = null;
        spriteDict.TryGetValue(leftSprites[currentLine], out sp);
        leftSprite.sprite = sp;
        spriteDict.TryGetValue(rightSprites[currentLine], out sp);
        rightSprite.sprite = sp;

        currentLine++;
    }

}
