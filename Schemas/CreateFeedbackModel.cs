namespace CompClubAPI.Schemas;

public class CreateFeedbackModel
{
    public int clubId { get; set; }
    public int rating { get; set; }
    public string comment { get; set; }
}