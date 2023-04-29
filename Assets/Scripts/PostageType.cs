using Unity.VisualScripting;

public class PostageType
{
    public string DisplayName { get; set; }
    public int Price { get; set; }
    
    /// <summary>
    /// Package must be delivered within this deadline
    /// </summary>
    public int DeadlineHours { get; set; }
    /// <summary>
    /// Different types can have weight limits.
    /// A random weight is generated and this is used to get a suitable type
    /// </summary>
    public int MaxWeight { get; set; }
}
