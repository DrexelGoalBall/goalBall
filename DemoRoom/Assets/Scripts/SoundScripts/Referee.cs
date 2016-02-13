using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Referee : NetworkBehaviour 
{
    /// <summary>
    /// This script controls what the referee would say when an event happens
    /// This script should only act from other scripts when audio needs to be spoken
    /// </summary>

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

    void Start()
    {
        // Set the callback for the queue
        queue.Callback = OnStringChanged;
    }

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

    private void OnStringChanged(SyncListString.Operation op, int index)
    {
        //Debug.Log(op + " ... " + (index-1));

        if (op == SyncListString.Operation.OP_ADD)
        {
            playlist.Add(queue[index-1]);
        }
    }

    //So other scripts can wait until referee is finished speaking
    public bool refereeSpeaking()
    {
        if (AS.isPlaying || playlist.Count > 0) return true;
        return false;
    }
    
    //Things referee can say
    public void PlayFifteenSeconds()
    {
        if (isServer)
            queue.Add("FifteenSeconds");
    }

    public void PlayBallOver()
    {
        if (isServer)
            queue.Add("BallOver");
    }

    public void PlayBlockedOut()
    {
        if (isServer)
            queue.Add("BlockedOut");
    }

    public void PlayCenter()
    {
        if (isServer)
            queue.Add("Center");
    }

    public void PlayDeadBall()
    {
        if (isServer)
            queue.Add("DeadBall");
    }

    public void PlayExtraThrows()
    {
        if (isServer)
            queue.Add("ExtraThrows");
    }

    public void PlayGame()
    {
        if (isServer)
            queue.Add("Game");
    }

    public void PlayHalfTime()
    {
        if (isServer)
            queue.Add("HalfTime");
    }

    public void PlayLineOut()
    {
        if (isServer)
            queue.Add("LineOut");
    }

    public void PlayOfficialTimeOut()
    {
        if (isServer)
            queue.Add("OfficialTimeOut");
    }

    public void PlayOut()
    {
        if (isServer)
            queue.Add("Out");
    }

    public void PlayOvertime()
    {
        if (isServer)
            queue.Add("Overtime");
    }

    public void PlayPenaltyDeclined()
    {
        if (isServer)
            queue.Add("PenaltyDeclined");
    }

    public void PlayPersonalPenalty()
    {
        if (isServer)
            queue.Add("PersonalPenalty");
    }

    public void PlayPlay()
    {
        if (isServer)
            queue.Add("Play");
    }

    public void PlayQuietPlease()
    {
        if (isServer)
            queue.Add("QuietPlease");
    }

    public void PlayTime()
    {
        if (isServer)
            queue.Add("Time");
    }

    public void PlayTimeOut()
    {
        if (isServer)
            queue.Add("TimeOut");
    }

    public void PlayRedTeam()
    {
        if (isServer)
            queue.Add("RedTeam");
    }

    public void PlayBlueTeam()
    {
        if (isServer)
            queue.Add("BlueTeam");
    }

    public void PlayGoal()
    {
        if (isServer)
            queue.Add("Goal");
    }

    public void PlayFoul()
    {
        if (isServer)
            queue.Add("Foul");
    }
}
