using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

/// <summary>
///     Keeps track of all the games available to join and allows users to join them
/// </summary>
public class JoinGameList : MonoBehaviour
{
    // Reference to NetworkManager_Custom script
    public NetworkManager_Custom networkManager;

    // Referense to ServerLink script
    public ServerLink serverLink;

    // List of strings for currently available games
    public List<string> games = new List<string>();

    // Current index of the game index being inspected
    private int gameIndex = -1;

    // Flag to tell if the contents of the games list has changed and needs the UI to be refreshed
    private bool gamesListUpdated = false;

    // UI components
    public GameObject gamePanel;
    public Text gameName, leftArrow, rightArrow;

    // Whether or not the UI is being displayed
    private bool displayed = false;
    public bool Displayed
    {
        get { return displayed; }
        set
        {
            this.displayed = value;
            if (displayed)
            {
                // Show the panel and get games
                gamePanel.SetActive(true);
                //RefreshGameList();
                GetGameList();
            }
            else
            {
                // No more audio
                ResetAudio();
                // No more games
                ClearGameList();
                ResetUI();
                gameIndex = -1;
                gamePanel.SetActive(false);
            }
        }
    }

    // Get audio source
    private AudioSource audioSource;

    // All of the clips that can be played
    public List<AudioClip> audioClips = new List<AudioClip>();

    // Dictionary to contain all of the clips for access
    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();

    // List of clips that need to be played
    public List<string> playlist = new List<string>();

    // Strings for input
    public string HorizontalInput = "Move Horizontal";
    public string SubmitButton = "Submit";
    public string CancelButton = "Cancel";

    // The previous input value and the threshold to count as full axis tilt
    private float prevHorizontalInputValue = 0f;
    private float axisThresh = 0.9f;

    /// <summary>
    ///     Get the initial information and set up the audio clip dictionary
    /// </summary>
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Set up the event to update the games list
        serverLink.connector.GamesListChanged += UpdateGameList;

        // Add all of the clips to the dictionary
        for (int i = 0; i < audioClips.Count; i++)
        {
            AudioClip ac = audioClips[i];
            clipDictionary.Add(ac.name, ac);
        }
    }

    /// <summary>
    ///     Accept user input and play audio for as long as this object is displayed
    /// </summary>
    void Update()
    {
        if (displayed)
        {
            PlayAudio();
            UserInput();
            
            // Refresh the UI list so users can see new games
            if (gamesListUpdated)
            {
                if (games.Count > 0)
                {
                    RefreshGameList();
                    gamesListUpdated = false;
                }
                else
                {
                    // No games to show, mention that and close list
                    //playlist.Add("NoGames");
                    this.Displayed = false;
                    gamesListUpdated = false;
                }
            }
        }
    }

    /// <summary>
    ///     Reset the UI back to its original state
    /// </summary>
    private void ResetUI()
    {
        gameName.text = "No Games";
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Show/hide the arrows depending on number of games and current index
    /// </summary>
    private void DisplayArrows()
    {
        // Check if there is more than a single game
        if (games.Count > 1)
        {
            // Show/hide the arrows according to the game index
            leftArrow.gameObject.SetActive(gameIndex > 0);
            rightArrow.gameObject.SetActive(gameIndex < games.Count - 1);
        }
        else
        {
            // Hide the arrows
            leftArrow.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(false);
        }
    }

    /// <summary>
    ///     Retrieves available games from server
    /// </summary>
    private void GetGameList()
    {
        // Connect to the server if it is not already connected
        if (!serverLink.connector.isConnected())
        {
            serverLink.ConnectToServer();
        }

        // If the server is connected, request it to send the games
        if (serverLink.connector.isConnected())
        {
            serverLink.connector.ListGames();
        }
    }

    /// <summary>
    ///     When the games list has been changed, update the local game list
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateGameList(object sender, EventArgs e)
    {
        Debug.Log(serverLink.connector.lstGames.Count);

        if (serverLink.connector.lstGames.Count > 0)
        {
            // Check if there are any differences
            if (!games.SequenceEqual(serverLink.connector.lstGames))
            {
                games = serverLink.connector.lstGames;
                gamesListUpdated = true;
            }
        }
        else
        {
            // There are no games, but the game list cannot be hidden in event so mark it as updated and handle in Update()
            games = serverLink.connector.lstGames;
            gamesListUpdated = true;
        }
    }

    /// <summary>
    ///     Resets the list of all the games
    /// </summary>
    private void ClearGameList()
    {
        games.Clear();
    }

    /// <summary>
    ///     Updates the on-screen game list if there are games, hides the game list if not
    /// </summary>
    private void RefreshGameList()
    {
        // Do not talk anymore about the old games
        ResetAudio();

        // Check if there are any games to list
        if (games.Count > 0)
        {
            // Start index back at front of list
            gameIndex = 0;
            // Update the UI and start reading it
            gameName.text = "Game " + games[gameIndex];
            ReadGameName();
            // Show the right arrow if necessary
            DisplayArrows();
        }
        else
        {
            // No games to show, mention that and close list
            //playlist.Add("NoGames");
            this.Displayed = false;
        }
    }

    /// <summary>
    ///     Check if the user has provided input and react accordingly
    /// </summary>
    private void UserInput()
    {
        // Check if the player wants to hide the game list
        if (InputPlayers.player0.GetButtonDown(CancelButton))
        {
            this.Displayed = false;
            return;
        }

        // Make sure there are games to accept any additional input
        if (games.Count > 0)
        {
            // Check if the player wants to join the currently selected game
            if (InputPlayers.player0.GetButtonDown(SubmitButton))
            {
                // Make sure we are connected to the manager
                if (serverLink.connector.isConnected())
                {
                    // Get the port number for the game to join
                    int portNum;
                    if (int.TryParse(games[gameIndex], out portNum))
                    {
                        Debug.Log("IP: " + serverLink.gameManagerIP + " Port: " + portNum);
                        networkManager.JoinDirect(serverLink.gameManagerIP, portNum);
                    }
                }

                return;
            }

            float horizontalInputValue = InputPlayers.player0.GetAxisRaw(HorizontalInput);

            if (gameIndex > 0)
            {
                if (horizontalInputValue < -axisThresh && prevHorizontalInputValue > -axisThresh)
                {
                    // Do not talk anymore about the old games
                    ResetAudio();
                    // Move to previous game
                    gameIndex--;
                    gameName.text = "Game " + games[gameIndex];
                    ReadGameName();
                    // Show/hide the arrows if necessary
                    DisplayArrows();
                }
            }
            else if (gameIndex < games.Count - 1)
            {
                if (horizontalInputValue > axisThresh && prevHorizontalInputValue < axisThresh)
                {
                    // Do not talk anymore about the old games
                    ResetAudio();
                    // Move to next game
                    gameIndex++;
                    gameName.text = "Game " + games[gameIndex];
                    DisplayArrows();
                    // Show/hide the arrows if necessary
                    ReadGameName();
                }
            }

            prevHorizontalInputValue = horizontalInputValue;
        }
    }

    /// <summary>
    ///     Play the next audio if there is no other clips playing
    /// </summary>
    private void PlayAudio()
    {
        // Check if audio is already playing
        if (!audioSource.isPlaying)
        {
            // Is there any clip to play?
            if (playlist.Count > 0)
            {
                // Get and play next clip
                AudioClip audio = clipDictionary[playlist[0]];
                audioSource.clip = audio;
                audioSource.Play();
                // Remove so it is not played again
                playlist.RemoveAt(0);
            }
        }
    }

    /// <summary>
    ///     Stops the audio and clears the playlist
    /// </summary>
    private void ResetAudio()
    {
        audioSource.Stop();
        audioSource.clip = null;
        playlist.Clear();
    }

    /// <summary>
    ///     Adds the appropriate clips to the playlist to read the name of the game out loud
    /// </summary>
    private void ReadGameName()
    {
        playlist.Add("Game2");

        // Get each individual digit
        foreach (char c in games[gameIndex])
        {
            int number = (int)System.Char.GetNumericValue(c);
            // Retrieve all of the sound files for this number
            List<string> soundFilenames = NumberSoundUtility.NumberToSoundFilenames(number);
            if (soundFilenames != null)
            {
                foreach (string sfn in soundFilenames)
                {
                    playlist.Add(sfn);
                }
            }
        }
    }
}
