using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StreamConfig
{
    public List<PostageType> PostageTypes;
    // Random ranges
    public int MaxSize = 3; // Minimum 1
    public int MinValue = 5;
    public int MaxValue = 20;

    // Ranges for random next order timing
    public int RateUpper = 25;
    public int RateLower = 12;

    public float ShortestTime = 20.0f;
    public float LongestTime = 60.0f;


    // Some default postage types 
    public static List<PostageType> GetDefaultTypes()
    {
        // TODO - add type insurance settings for how much the player has to pay back for lost packages
        var types = new List<PostageType>();
        
        types.Add(new PostageType
        {
            DisplayName = "Same day",
            DeadlineHours = 24,
            Price = 5,
        });

        types.Add(new PostageType
        {
            DisplayName = "3 day",
            DeadlineHours = 72,
            Price = 3,
        });

        types.Add(new PostageType
        {
            DisplayName = "Same week",
            DeadlineHours = 24*7,
            Price = 3,
        });

        types.Add(new PostageType
        {
            DisplayName = "Worldwide 1 month",
            DeadlineHours = 28*24,
            Price = 5,
        });

        types.Add(new PostageType
        {
            DisplayName = "Worldwide 2 month",
            DeadlineHours = (28 * 24) * 2,
            Price = 5,
        });

        return types;

    }
}
