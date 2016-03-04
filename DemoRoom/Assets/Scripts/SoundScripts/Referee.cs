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
        if (Input.GetKeyDown(KeyCode.Keypad7))
            AS.panStereo = -1;
        if (Input.GetKeyDown(KeyCode.Keypad8))
            AS.panStereo = 0;
        if (Input.GetKeyDown(KeyCode.Keypad9))
            AS.panStereo = 1;
        
        // Check if audio is already playing
        if (!AS.isPlaying)
        {
            // Is there any clip to play?
            if (playlist.Count > 0)
            {
                // Get and play next clip
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
    
    //Things referee can say
    /// <summary>
    /// Ref says: fifteen seconds.
    /// </summary>
    public void PlayFifteenSeconds()
    {
        if (isServer)
            queue.Add("FifteenSeconds");
    }

    /// <summary>
    /// Ref says: Ball Over.
    /// </summary>
    public void PlayBallOver()
    {
        if (isServer)
            queue.Add("BallOver");
    }

    /// <summary>
    /// Ref says: blocked out.
    /// </summary>
    public void PlayBlockedOut()
    {
        if (isServer)
            queue.Add("BlockedOut");
    }

    /// <summary>
    /// Ref says: center.
    /// </summary>
    public void PlayCenter()
    {
        if (isServer)
            queue.Add("Center");
    }

    /// <summary>
    /// Ref says: dead ball.
    /// </summary>
    public void PlayDeadBall()
    {
        if (isServer)
            queue.Add("DeadBall");
    }

    /// <summary>
    /// Ref says: Extra throws.
    /// </summary>
    public void PlayExtraThrows()
    {
        if (isServer)
            queue.Add("ExtraThrows");
    }

    /// <summary>
    /// Ref says: game.
    /// </summary>
    public void PlayGame()
    {
        if (isServer)
            queue.Add("Game");
    }

    /// <summary>
    /// Ref says: half time.
    /// </summary>
    public void PlayHalfTime()
    {
        if (isServer)
            queue.Add("HalfTime");
    }

    /// <summary>
    /// Ref says: Line Out.
    /// </summary>
    public void PlayLineOut()
    {
        if (isServer)
            queue.Add("LineOut");
    }

    /// <summary>
    /// Ref says: official time out.
    /// </summary>
    public void PlayOfficialTimeOut()
    {
        if (isServer)
            queue.Add("OfficialTimeOut");
    }

    /// <summary>
    /// Ref says: Out.
    /// </summary>
    public void PlayOut()
    {
        if (isServer)
            queue.Add("Out");
    }

    /// <summary>
    /// Ref says: overtime.
    /// </summary>
    public void PlayOvertime()
    {
        if (isServer)
            queue.Add("Overtime");
    }

    /// <summary>
    /// Ref says: Penalty Declined.
    /// </summary>
    public void PlayPenaltyDeclined()
    {
        if (isServer)
            queue.Add("PenaltyDeclined");
    }

    /// <summary>
    /// Ref says: personal penalty.
    /// </summary>
    public void PlayPersonalPenalty()
    {
        if (isServer)
            queue.Add("PersonalPenalty");
    }

    /// <summary>
    /// Ref says: play.
    /// </summary>
    public void PlayPlay()
    {
        if (isServer)
            queue.Add("Play");
    }

    /// <summary>
    /// Ref says: quiet please.
    /// </summary>
    public void PlayQuietPlease()
    {
        if (isServer)
            queue.Add("QuietPlease");
    }

    /// <summary>
    /// Ref says: time.
    /// </summary>
    public void PlayTime()
    {
        if (isServer)
            queue.Add("Time");
    }

    /// <summary>
    /// Ref says: time out.
    /// </summary>
    public void PlayTimeOut()
    {
        if (isServer)
            queue.Add("TimeOut");
    }

    /// <summary>
    /// Ref says: red team.
    /// </summary>
    public void PlayRedTeam()
    {
        if (isServer)
            queue.Add("RedTeam");
    }

    /// <summary>
    /// Ref says: blue team.
    /// </summary>
    public void PlayBlueTeam()
    {
        if (isServer)
            queue.Add("BlueTeam");
    }

    /// <summary>
    /// Ref says: goal.
    /// </summary>
    public void PlayGoal()
    {
        if (isServer)
            queue.Add("Goal");
    }

    /// <summary>
    /// Ref says: foul.
    /// </summary>
    public void PlayFoul()
    {
        if (isServer)
            queue.Add("Foul");
    }

    /// <summary>
    /// Takes the current scores and announces them in audio.
    /// </summary>
    public void ReadScore(int scoreToRead)
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

        if (tens != 0)
        {
             if (isServer)
                queue.Add(numMap[tens]);    
        }

        if (tens == 0 || ones != 0) // Only use zero if tens is zero
        if (isServer)
            queue.Add(numMap[ones]);    
    }
}
