﻿using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GVarHandler : MonoBehaviour
{

    public AudioSource speaker;

    public bool debug_mode;

    // Keybindings data

    public Image AOn;
    public Image SOn;
    public Image DOn;
    public Image FOn;
    public Image GOn;
    public Image HOn;

    public Text AVal;
    public Text SVal;
    public Text DVal;
    public Text FVal;
    public Text GVal;
    public Text HVal;

    //Sliders data
    public Toggle battletime;
    public Slider TSlide;
    public Slider VSlide;

    //Bottom row data
    public Text LPC;

    public Text Freq;
    public Text Ten;
    public Text Ton;

    public Text Tempo;
    public Text Volume;

    public Text Outlook;
    public Text Agency;
    public Text Assurance;

    public Text ATT;

    //holds all the chords
    public AudioCollector Audio;


    //private data for algorithm-ing
    private string[] chords;
    private int prevChord = -1;
    private string[] possChords;
    private int[,] tensions;
    private int[] tones;

    private string[] normalfeels;
    private string[] battlefeels;

    //globals for calculations
    private float frequency;
    private float tension;
    private float tone;
    private float tempo;
    private float volume;

    private int cooldown = 0;
    private int coolValue = 5;
    private bool isPlaying = false;
    private int stopcooldown = 30;
    private int stopcoolvalue = 30;
    private float lastTime = 0.001f;

    private float acy = 0.0f;
    private float olk = 0.0f;
    private float ass = 0.0f;

    private Chord[] chord;
    public AudioClip[] samples;

    private int[] hotKeys;
    // Use this for initialization
    private void Start()
    {
        tone = 0;
        tension = 5;
        frequency = 0.0f;

        hotKeys = new int[] { 0, 10, 14, 19, 9, 20 };


        setPossChords();
        setChords();
        setTones();
        setTensions();
        setFeels();

        //value is between 0 and 1
        //expect max speed to be 240 bpm
        tempo = TSlide.value * 240;

        //value is between 0 and 1
        volume = VSlide.value;

    }

    // initialization but compressed, for algorithm-y stuff
    void setChords()
    {
        chords = new string[6];
        chords[0] = "I";
        chords[1] = "IV";
        chords[2] = "V";
        chords[3] = "vi";
        chords[4] = "iii";
        chords[5] = "ii";
        if (debug_mode)
        {
            AVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[0]];
            SVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[1]];
            DVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[2]];
            FVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[3]];
            GVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[4]];
            HVal.text = GAMESTATS.possChords[GAMESTATS.chosenChords[5]];
        }
    }

    void setPossChords()
    {
        possChords = new string[] { "I", "i", "IIb", "iib", "II", "ii", "IIIb", "iiib", "III", "iii", "IV", "iv",
            "Vb", "vb", "V", "v", "VIb", "vib", "VI", "vi", "VIIb", "viib", "VII", "vii"};
    }

    void setTones()
    {
        tones = new int[] { 3, -1, -2, -2, 1, 2, -2, -1, -2, 2, 2, 1, -2, -1, 2, 1, -2, -2, 2, -3, 1, -2, 1, 1 };
    }

    void setTensions()
    {
        tensions = new int[,]{{ 0,  0,  0,  0,  5,  4,  0,  0,  6,  1,  5,  1,  0,  0,  6,  2,  4,  0,  6,  3,  4,  2,  3,  0},
                                   {-1,  0,  0,  0,  0,  0,  3,  0,  0,  0, -3,  3,  0,  0,  0,  2,  4,  2,  0,  0,  0,  0,  0,  0},
                                   {-1, -3,  0,  1,  0,  0,  1,  0,  0,  0,  0, -2,  2,  1,  0,  2,  1,  0, -2,  0,  1,  0,  0,  2},
                                   {-1,  0,  0,  0,  0,  0,  0,  1,  3,  0,  0,  2, -2, -1,  0,  0,  3,  1, -4,  0,  0,  0,  2,  1},
                                   {-2,  0,  0, -3,  0,  2,  0,  0,  1,  1, -2,  0,  0,  2,  4,  0,  0,  0,  2, -3,  0,  0,  2,  4},
                                   {-5,  2,  0,  0,  1,  0,  0,  0,  4,  2,  3,  5,  0,  0,  5,  0,  0,  0, -2, -5,  3,  4,  0,  0},
                                   {-1, -3,  2,  0,  0,  0,  0,  1,  0,  1,  2,  2,  1,  0,  3, -3, -3, -3,  0,  0,  2,  1, -2, -3},
                                   {-2,  0,  1,  1,  0,  0, -2,  0,  0,  0,  0,  0, -2, -1,  0, -2, -2, -2,  0,  0,  0, -3, -3, -3},
                                   {-3,  0,  0, -1,  0, -2,  0,  0,  0, -2,  4,  0,  0,  1,  2,  0,  0, -2,  0, -8,  0,  0,  0, -1},
                                   {-2,  0,  0,  1,  3,  2,  2,  0,  3,  0,  4,  2,  3,  1,  4, -3,  2,  1,  3, -3,  0,  0,  2,  2},
                                   {-7, -2,  0,  0,  1, -3, -4,  1,  1, -4,  0,  2,  0,  0,  5, -2,  2,  0,  1, -4,  2,  0,  0,  0},
                                   {-3, -3,  2,  0,  0,  2, -3,  0,  1, -2, -1,  0,  0,  0,  1,  2, -4, -2,  0,  0,  2,  1,  0,  1},
                                   {-1,  0,  0, -2,  1,  0, -3,  1,  0,  0, -1,  0,  0,  1,  0,  1, -2, -2, -2,  0,  0, -2,  3,  2},
                                   {-1,  0,  0, -2,  1,  0, -1,  0,  0,  0,  0, -3, -2,  0,  0,  1,  1,  0, -2, -1, -1, -1,  2,  2},
                                   {-8, -3,  0,  0,  1, -4,  3,  0,  2, -2, -1, -1,  0,  0,  0,  2,  1,  0,  2, -5,  1,  0,  2, -2},
                                   {-5, -2, -2,  0,  3,  2, -1,  1,  4, -2, -4, -3,  0,  1,  1,  0,  3,  1,  2,  1, -4, -3,  0,  0},
                                   {-1, -4,  1, -3,  2,  2, -2, -3,  1,  0, -1,  0, -3, -1,  3, -2,  0, -2,  0, -2,  1, -1,  0,  0},
                                   {-1, -2,  0, -2,  0,  2, -2, -1,  2, -3,  0,  2, -1, -2,  0, -2, -1,  0,  0,  0, -2,  1,  0,  0},
                                   {-5,  0,  2,  1,  2, -2,  0,  0,  1, -3, -3,  0,  3,  1,  2,  0,  0,  0,  0, -1,  0,  0,  0, -1},
                                   {-1,  2,  0,  0,  4,  1,  0,  0,  5,  3,  4,  3,  0,  0,  5,  4,  2,  1,  4,  0,  2,  4,  0,  2},
                                   {-8,  0,  0,  0, -3, -3, -4,  2,  3,  0, -5, -2,  0, -1,  0,  2, -2, -2,  0, -3,  0, -1,  0,  0},
                                   {-2,  0,  0,  0,  0,  0, -1, -1,  4,  0, -2, -3, -1, -2,  0,  1, -2, -2,  0, -1, -3,  0,  0,  0},
                                   {-5,  0,  0, -2, -2,  0,  0, -3,  2, -3,  0,  0,  1,  0,  2,  0,  2,  3,  0, -3,  0, -1,  0,  1},
                                   {-1,  0,  0,  0, -4,  0,  0,  0,  0, -2,  0, -2,  0,  2,  4,  0,  1,  0, -2, -4,  0,  0,  1,  0}};
    }

    void setFeels()
    {
        normalfeels = new string[] { "Determination", "Foreboding", "Hopeless", "Nervous", "Elated", "Excited", "Content", "Hopeful" };
        battlefeels = new string[] { "Determined", "Curious", "Defeated", "Terrified", "Victorious", "Cautious", "Relenting", "Alert" };
    }

    private void playChord(Image on, string text, int index)
    {
        if (debug_mode)
            on.color = Color.green;

        string chordPlayed = text;

        //set tone
        tone += (GAMESTATS.tones[index] / 10);
        if (tone > 10)
        {
            tone = 10;
        }
        else if (tone < -10)
        {
            tone = -10;
        }
        GAMESTATS.tone = tone;
        if (debug_mode)
            Ton.text = "" + tone;

        //set tension
        if (prevChord == -1)
        {
            tension = tension + GAMESTATS.tensions[0, index];
        }
        else
        {
            tension = tension + GAMESTATS.tensions[prevChord, index];
        }
        if (tension < 0)
            tension = 0;
        if (tension > 20)
            tension = 20;
        if (debug_mode)
            Ten.text = "" + tension;
        GAMESTATS.tension = tension;

        //set freq
        float deltTime = Time.time - lastTime;
        lastTime = Time.time;
        frequency = (tempo - 240 * deltTime) / (tempo);
        if (frequency < 0)
            frequency = 0;
        if (debug_mode)
            Freq.text = "" + frequency;
        GAMESTATS.frequency = frequency;

        //set LPC
        if (debug_mode)
            LPC.text = text;
        prevChord = index;
        Audio.clips[index].Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            stopcooldown--;
        }
        cooldown--;
        if (cooldown > 0)
        {
            Debug.Log(cooldown + "");
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                playChord(AOn, GAMESTATS.possChords[GAMESTATS.chosenChords[0]], GAMESTATS.chosenChords[0]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[0]].Stop();
                AOn.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                playChord(SOn, GAMESTATS.possChords[GAMESTATS.chosenChords[1]], GAMESTATS.chosenChords[1]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[1]].Stop();
                SOn.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                playChord(DOn, GAMESTATS.possChords[GAMESTATS.chosenChords[2]], GAMESTATS.chosenChords[2]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[2]].Stop();
                DOn.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                playChord(FOn, GAMESTATS.possChords[GAMESTATS.chosenChords[3]], GAMESTATS.chosenChords[3]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[3]].Stop();
                FOn.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                playChord(GOn, GAMESTATS.possChords[GAMESTATS.chosenChords[4]], GAMESTATS.chosenChords[4]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[4]].Stop();
                GOn.color = Color.red;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                playChord(HOn, GAMESTATS.possChords[GAMESTATS.chosenChords[5]], GAMESTATS.chosenChords[5]);
                cooldown = coolValue;
                stopcooldown = stopcoolvalue;
            }
            else
            {
                if (stopcooldown < 0)
                    Audio.clips[GAMESTATS.chosenChords[5]].Stop();
                HOn.color = Color.red;
            }
        }

        GAMESTATS.volume = volume;

        for (int i = 0; i < 24; ++i)
        {
            Audio.clips[i].volume = GAMESTATS.volume;
        }

        tempo = TSlide.value * 240;
        GAMESTATS.tempo = tempo;
        volume = VSlide.value;
        GAMESTATS.volume = volume;
        if (debug_mode)
        {
            Tempo.text = "" + tempo;
            Volume.text = "" + volume;
        }
    }
}