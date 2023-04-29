using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class StreamConfig 
{
    public List<PostageType> PostageTypes = MakeDefaultTypes();
    // Random ranges
    public int MaxSize = 3; // Minimum 1
    public int MinValue = 5;
    public int MaxValue = 20;

    // Ranges for random next order timing
    public float RateUpper = 30;
    public float RateLower = 5;

    public StreamConfig() {
         PostageTypes = MakeDefaultTypes();
    }




    // Some default postage types 
    public static List<PostageType> MakeDefaultTypes()
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
