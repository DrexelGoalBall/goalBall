using UnityEngine;
using System.Collections.Generic;

public class Referee : MonoBehaviour {

    /// <summary>
    /// This script controls what the referee would say when an event happens
    /// This script should only act from other scripts when audio needs to be spoken
    /// </summary>

    //Get audio source and clips
	public AudioSource AS;

    public AudioClip FifteenSeconds;
    public AudioClip BallOver;
    public AudioClip BlockedOut;
    public AudioClip Center;
    public AudioClip DeadBall;
    public AudioClip ExtraThrows;
    public AudioClip Game;
    public AudioClip HalfTime;
    public AudioClip LineOut;
    public AudioClip OfficialTimeOut;
    public AudioClip Out;
    public AudioClip Overtime;
    public AudioClip PenaltyDeclined;
    public AudioClip Play;
    public AudioClip PersonalPenalty;
    public AudioClip QuietPlease;
    public AudioClip Time;
    public AudioClip TimeOut;
    public AudioClip RedTeam;
    public AudioClip BlueTeam;
    public AudioClip Goal;

    private List<AudioClip> queue;

    void Start()
    {
        AS = gameObject.GetComponent<AudioSource>();
        queue = new List<AudioClip>();
    }

    void Update()
    {
        if (!AS.isPlaying)
        {
            if (queue.Count > 0)
            {
                AudioClip audio = queue[0];
                queue.RemoveAt(0);
                AS.clip = audio;
                AS.Play();
            }
        }
    }
    
    //So other scripts can wait until referee is finished speaking
    public bool refereeSpeaking()
    {
        return AS.isPlaying;
    }
    
    //Things referee can say
    public void PlayFifteenSeconds()
    {
        queue.Add(FifteenSeconds);
    }

    public void PlayBallOver()
    {
        queue.Add(BallOver);
    }

    public void PlayBlockedOut()
    {
        queue.Add(BlockedOut);
    }

    public void PlayCenter()
    {
        queue.Add(Center);
    }

    public void PlayDeadBall()
    {
        queue.Add(DeadBall);
    }

    public void PlayExtraThrows()
    {
        queue.Add(ExtraThrows);
    }

    public void PlayGame()
    {
        queue.Add(Game);
    }

    public void PlayHalfTime()
    {
        queue.Add(HalfTime);
    }

    public void PlayLineOut()
    {
        queue.Add(LineOut);
    }

    public void PlayOfficialTimeOut()
    {
        queue.Add(OfficialTimeOut);
    }

    public void PlayOut()
    {
        queue.Add(Out);
    }

    public void PlayOvertime()
    {
        queue.Add(Overtime);
    }

    public void PlayPenaltyDeclined()
    {
        queue.Add(PenaltyDeclined);
    }

    public void PlayPersonalPenalty()
    {
        queue.Add(PersonalPenalty);
    }

    public void PlayPlay()
    {
        queue.Add(Play);
    }

    public void PlayQuietPlease()
    {
        queue.Add(QuietPlease);
    }

    public void PlayTime()
    {
        queue.Add(Time);
    }

    public void PlayTimeOut()
    {
        queue.Add(TimeOut);
    }

    public void PlayRedTeam()
    {
        queue.Add(RedTeam);
    }

    public void PlayBlueTeam()
    {
        queue.Add(BlueTeam);
    }

    public void PlayGoal()
    {
        queue.Add(Goal);
    }

}
