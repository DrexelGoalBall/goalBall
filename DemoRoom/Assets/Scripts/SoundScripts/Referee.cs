using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

/// <summary>
/// This script controls what the referee would say when an event happens
/// This script should only act from other scripts when audio needs to be spoken
/// </summary>
public class Referee : NetworkBehaviour 
{
    // Get audio source
	public AudioSource AS;

    // All of the clips that can be played
    public List<AudioClip> audioClips = new List<AudioClip>();

    // Dictionary to contain all of the clips for access
    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

    // Synchronized list of all clips added from server
    public SyncListString queue = new SyncListString();

    // List of clips that need to be played
    public List<string> playlist = new List<string>();

    /// <summary>
    /// Sets up certain object when the scene starts up.
    /// </summary>
    void Awake()
    {
        AS = gameObject.GetComponent<AudioSource>();

        // Add all of the clips to the dictionary
        for (int i = 0; i < audioClips.Count; i++)
        {
            AudioClip ac = audioClips[i];
            clipDictionary.Add(ac.name, ac);
        }
    }

    /// <summary>
    /// initializes the queue of strings.
    /// </summary>
    void Start()
    {
        // Set the callback for the queue
        queue.Callback = OnStringChanged;
    }

    /// <summary>
    /// Manages the queue, and plays sounds that are listed in the queue.
    /// </summary>
    void Update()
    {
        // Debug controls to test audio in multiple builds
        //if (Input.GetKeyDown(KeyCode.Keypad7))
        //    AS.panStereo = -1;
        //if (Input.GetKeyDown(KeyCode.Keypad8))
        //    AS.panStereo = 0;
        //if (Input.GetKeyDown(KeyCode.Keypad9))
        //    AS.panStereo = 1;
        
        // Check if audio is already playing
        if (!AS.isPlaying)
        {
            // Is there any clip to play?
            if (playlist.Count > 0)
            {
                // Get and play next clip
                if (!clipDictionary[playlist[0]])
                {
                    playlist.RemoveAt(0);
                    return;
                }
                AudioClip audio = clipDictionary[playlist[0]];
                AS.clip = audio;
                AS.Play();
                Debug.Log("Referee: " + AS.clip.name);
                // Remove so it is not played again
                playlist.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Adds string to the end of the queue.
    /// </summary>
    /// <param name="op"></param>
    /// <param name="index"></param>
    private void OnStringChanged(SyncListString.Operation op, int index)
    {
        //Debug.Log(op + " ... " + (index-1));

        if (op == SyncListString.Operation.OP_ADD)
        {
            playlist.Add(queue[index-1]);
        }
    }

    /// <summary>
    /// Checks if the referee is currently speaking and returns a boolean.
    /// </summary>
    /// <returns></returns>
    public bool refereeSpeaking()
    {
        if (AS.isPlaying || playlist.Count > 0) return true;
        return false;
    }

    /// <summary>
    ///     On the server, add this clip name to the queue and local playlist
    /// </summary>
    /// <param name="clipName">Name of clip to add to the queue</param>
    private void AddClipStringToQueue(string clipName)
    {
        queue.Add(clipName);
        playlist.Add(clipName);
    }
    
    //Things referee can say
    /// <summary>
    /// Ref says: fifteen seconds.
    /// </summary>
    public void PlayFifteenSeconds()
    {
        if (isServer)
            AddClipStringToQueue("Fifteen2");
    }

    /// <summary>
    /// Ref says: Ball Over.
    /// </summary>
    public void PlayBallOver()
    {
        if (isServer)
            AddClipStringToQueue("Ball_Over2");
    }

    /// <summary>
    /// Ref says: blocked out.
    /// </summary>
    public void PlayBlockedOut()
    {
        if (isServer)
            AddClipStringToQueue("Blocked_Out0");
    }

    /// <summary>
    /// Ref says: center.
    /// </summary>
    public void PlayCenter()
    {
        if (isServer)
            AddClipStringToQueue("Center0");
    }

    /// <summary>
    /// Ref says: dead ball.
    /// </summary>
    public void PlayDeadBall()
    {
        if (isServer)
            AddClipStringToQueue("Dead_Ball");
    }

    /// <summary>
    /// Ref says: Extra throws.
    /// </summary>
    public void PlayExtraThrows()
    {
        if (isServer)
            AddClipStringToQueue("Extra_Throws0");
    }

    /// <summary>
    /// Ref says: game.
    /// </summary>
    public void PlayGame()
    {
        if (isServer)
            AddClipStringToQueue("Game0");
    }

    /// <summary>
    /// Ref says: half time.
    /// </summary>
    public void PlayHalfTime()
    {
        if (isServer)
            AddClipStringToQueue("HalfTime0");
    }

    /// <summary>
    /// Ref says: Line Out.
    /// </summary>
    public void PlayLineOut()
    {
        if (isServer)
            AddClipStringToQueue("LineOut1");
    }

    /// <summary>
    /// Ref says: official time out.
    /// </summary>
    public void PlayOfficialTimeOut()
    {
        if (isServer)
            AddClipStringToQueue("TO0");
    }

    /// <summary>
    /// Ref says: Out.
    /// </summary>
    public void PlayOut()
    {
        if (isServer)
            AddClipStringToQueue("Out1");
    }

    /// <summary>
    /// Ref says: overtime.
    /// </summary>
    public void PlayOvertime()
    {
        if (isServer)
            AddClipStringToQueue("Overtime0");
    }

    /// <summary>
    /// Ref says: Penalty Declined.
    /// </summary>
    public void PlayPenaltyDeclined()
    {
        if (isServer)
            AddClipStringToQueue("Penalty_Declined1");
    }

    /// <summary>
    /// Ref says: personal penalty.
    /// </summary>
    public void PlayPersonalPenalty()
    {
        if (isServer)
            AddClipStringToQueue("Personal_Penalty0");
    }

    /// <summary>
    /// Ref says: play.
    /// </summary>
    public void PlayPlay()
    {
        if (isServer)
            AddClipStringToQueue("Play1");
    }

    /// <summary>
    /// Ref says: quiet please.
    /// </summary>
    public void PlayQuietPlease()
    {
        if (isServer)
            AddClipStringToQueue("Quiet2");
    }

    /// <summary>
    /// Ref says: time.
    /// </summary>
    public void PlayTime()
    {
        if (isServer)
            AddClipStringToQueue("Time0");
    }

    /// <summary>
    /// Ref says: time out.
    /// </summary>
    public void PlayTimeOut()
    {
        if (isServer)
            AddClipStringToQueue("TimeOut0");
    }

    /// <summary>
    /// Ref says: red team.
    /// </summary>
    public void PlayRedTeam()
    {
        if (isServer)
            AddClipStringToQueue("RedTeam1");
    }

    /// <summary>
    /// Ref says: blue team.
    /// </summary>
    public void PlayBlueTeam()
    {
        if (isServer)
            AddClipStringToQueue("Blue_Team0");
    }

    /// <summary>
    /// Ref says: goal.
    /// </summary>
    public void PlayGoal()
    {
        if (isServer)
            AddClipStringToQueue("Goal1");
    }

    /// <summary>
    /// Ref says: foul.
    /// </summary>
    public void PlayFoul()
    {
        if (isServer)
            AddClipStringToQueue("Personal_Penalty2");
    }

    /// <summary>
    /// Ref says: game over.
    /// </summary>
    public void PlayGameOver()
    {
        if (isServer)
            AddClipStringToQueue("GameOver");
    }

    /// <summary>
    /// Ref says: wins.
    /// </summary>
    public void PlayWins()
    {
        if (isServer)
            AddClipStringToQueue("Wins");
    }

    /// <summary>
    /// Ref says: final score.
    /// </summary>
    public void PlayFinalScore()
    {
        if (isServer)
            AddClipStringToQueue("FinalScore");
    }

    /// <summary>
    /// Ref says: press throw button to return to menu.
    /// </summary>
    public void PlayReturnToMenu()
    {
        if (isServer)
            AddClipStringToQueue("ReturnToMenu");
    }

    /// <summary>
    /// Takes the current scores and announces them in audio.
    /// </summary>
    public void ReadScore(int scoreToRead)
    {
        if (isServer)
        {
            Dictionary <int, string> numMap = new Dictionary <int, string>()
            {
                {0, "Zero"},
                {1, "One"},
                {2, "Two"},
                {3, "Three"},
                {4, "Four"},
                {5, "Five"},
                {6, "Six"},
                {7, "Seven"},
                {8, "Eight"},
                {9, "Nine"},
                {10, "Ten"},
                {11, "Eleven"},
                {12, "Twelve"},
    //          {13, "Thirteen"},
    //          {14, "Fourteen"},
    //          {15, "Fifteen"},
    //          {16, "Sixteen"},
    //          {17, "Seventeen"},
    //          {18, "Eighteen"},
    //          {19, "Nineteen"},
                {20, "Twenty"},
                {30, "Thirty"},
                {40, "Fourty"},
                {50, "Fifty"},
                {60, "Sixty"},
                {70, "Seventy"},
                {80, "Eighty"},
                {90, "Ninety"}
            };
            string fullScore = scoreToRead.ToString();
            int ones = fullScore[fullScore.Length-1] - '0';
            int tens = fullScore.Length > 1 ? fullScore[fullScore.Length-2] - '0' : 0;

            tens = tens * 10; // Allows reading directly into the dictionary above

    //       print ("TENS: " + tens);
    //       print("ONES: " + ones);
            if (tens > 10)
            {
                    AddClipStringToQueue(numMap[tens]);    
            }
            
            if (tens == 10 && ones != 0) // Tens are different because english is a beautiful language
            {
                if (ones == 1 || ones == 2) // Special cases
                {
                     AddClipStringToQueue(numMap[ones + 10]);
                }
                else
                {
                    AddClipStringToQueue(numMap[ones]);
                    AddClipStringToQueue("Teen");
                }
            }
            else if (tens == 0 || ones != 0) // Only use zero if tens is zero
            {
                AddClipStringToQueue(numMap[ones]);
            }
        }

    }

}
