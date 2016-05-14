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

    // All of the clips that can be player made in a make shift dictionary
    public List<string> Keys = new List<string>();
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
        for (int i = 0; i < Keys.Count; i++)
        {
            AudioClip ac = audioClips[i];
            string name = Keys[i];
            clipDictionary.Add(name, ac);
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
                //// Get and play next clip
                //if (!clipDictionary[playlist[0]])
                //{
                //    playlist.RemoveAt(0);
                //    return;
                //}
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
            AddClipStringToQueue("Fifteen Seconds");
    }

    /// <summary>
    /// Ref says: Ball Over.
    /// </summary>
    public void PlayBallOver()
    {
        if (isServer)
            AddClipStringToQueue("Ball Over");
    }

    /// <summary>
    /// Ref says: blocked out.
    /// </summary>
    public void PlayBlockedOut()
    {
        if (isServer)
            AddClipStringToQueue("Blocked Out");
    }

    /// <summary>
    /// Ref says: center.
    /// </summary>
    public void PlayCenter()
    {
        if (isServer)
            AddClipStringToQueue("Center");
    }

    /// <summary>
    /// Ref says: dead ball.
    /// </summary>
    public void PlayDeadBall()
    {
        if (isServer)
            AddClipStringToQueue("Dead Ball");
    }

    /// <summary>
    /// Ref says: Extra throws.
    /// </summary>
    public void PlayExtraThrows()
    {
        if (isServer)
            AddClipStringToQueue("Extra Throws");
    }

    /// <summary>
    /// Ref says: game.
    /// </summary>
    public void PlayGame()
    {
        if (isServer)
            AddClipStringToQueue("Game");
    }

    /// <summary>
    /// Ref says: half time.
    /// </summary>
    public void PlayHalfTime()
    {
        if (isServer)
            AddClipStringToQueue("Half Time");
    }

    /// <summary>
    /// Ref says: Line Out.
    /// </summary>
    public void PlayLineOut()
    {
        if (isServer)
            AddClipStringToQueue("Line Out");
    }

    /// <summary>
    /// Ref says: official time out.
    /// </summary>
    public void PlayOfficialTimeOut()
    {
        if (isServer)
            AddClipStringToQueue("Official Time Out");
    }

    /// <summary>
    /// Ref says: Out.
    /// </summary>
    public void PlayOut()
    {
        if (isServer)
            AddClipStringToQueue("Out");
    }

    /// <summary>
    /// Ref says: overtime.
    /// </summary>
    public void PlayOvertime()
    {
        if (isServer)
            AddClipStringToQueue("Overtime");
    }

    /// <summary>
    /// Ref says: Penalty Declined.
    /// </summary>
    public void PlayPenaltyDeclined()
    {
        if (isServer)
            AddClipStringToQueue("Penalty Declined");
    }

    /// <summary>
    /// Ref says: personal penalty.
    /// </summary>
    public void PlayPersonalPenalty()
    {
        if (isServer)
            AddClipStringToQueue("Personal Penalty");
    }

    /// <summary>
    /// Ref says: play.
    /// </summary>
    public void PlayPlay()
    {
        if (isServer)
            AddClipStringToQueue("Play");
    }

    /// <summary>
    /// Ref says: quiet please.
    /// </summary>
    public void PlayQuietPlease()
    {
        if (isServer)
            AddClipStringToQueue("Quiet Please");
    }

    /// <summary>
    /// Ref says: time.
    /// </summary>
    public void PlayTime()
    {
        if (isServer)
            AddClipStringToQueue("Time");
    }

    /// <summary>
    /// Ref says: time out.
    /// </summary>
    public void PlayTimeOut()
    {
        if (isServer)
            AddClipStringToQueue("Time Out");
    }

    /// <summary>
    /// Ref says: red team.
    /// </summary>
    public void PlayRedTeam()
    {
        if (isServer)
            AddClipStringToQueue("Red Team");
    }

    /// <summary>
    /// Ref says: blue team.
    /// </summary>
    public void PlayBlueTeam()
    {
        if (isServer)
            AddClipStringToQueue("Blue Team");
    }

    /// <summary>
    /// Ref says: goal.
    /// </summary>
    public void PlayGoal()
    {
        if (isServer)
            AddClipStringToQueue("Goal");
    }

    /// <summary>
    /// Ref says: foul.
    /// </summary>
    public void PlayFoul()
    {
        if (isServer)
            AddClipStringToQueue("Foul");
    }

    /// <summary>
    /// Ref says: game over.
    /// </summary>
    public void PlayGameOver()
    {
        if (isServer)
            AddClipStringToQueue("Game Over");
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
            AddClipStringToQueue("Final Score");
    }

    /// <summary>
    /// Ref says: press throw button to return to menu.
    /// </summary>
    public void PlayReturnToMenu()
    {
        if (isServer)
            AddClipStringToQueue("Return To Menu");
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
                {13, "Thirteen"},
                {14, "Fourteen"},
                {15, "Fifteen"},
                {16, "Sixteen"},
                {17, "Seventeen"},
                {18, "Eighteen"},
                {19, "Nineteen"},
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
                AddClipStringToQueue(numMap[ones + 10]);
            }
            else if (tens == 0 || ones != 0) // Only use zero if tens is zero
            {
                AddClipStringToQueue(numMap[ones]);
            }
        }

    }

}
