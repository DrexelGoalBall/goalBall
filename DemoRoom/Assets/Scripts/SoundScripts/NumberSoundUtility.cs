using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
///     Provides methods to obtain sound filenames from numbers
/// </summary>
public class NumberSoundUtility : MonoBehaviour
{
    /// <summary>
    ///     List of all the sound files to play for the given number
    /// </summary>
    /// <param name="number">Number to get sound files for</param>
    /// <returns></returns>
    public static List<string> NumberToSoundFilenames(int number)
    {
        List<string> soundFilenames = new List<string>();

        string fullNumber = number.ToString();
        int ones = fullNumber[fullNumber.Length - 1] - '0';
        int tens = fullNumber.Length > 1 ? fullNumber[fullNumber.Length - 2] - '0' : 0;
        tens = tens * 10;

        if (tens > 10)
        {
            // Add tens place sound filename
            soundFilenames.Add(tens.ToString());
            if (ones > 0)
            {
                // Add ones place sound filename since it is not a zero (ex. "twenty""one")
                soundFilenames.Add(ones.ToString());
            }
        }
        else if (tens < 10)
        {
            // Add ones place sound filename
            soundFilenames.Add(ones.ToString());
        }
        else
        {
            // Add tens+one sound filename
            soundFilenames.Add((tens + ones).ToString());
        }

        return soundFilenames;
    }
}
