namespace CompClubAPI.Schemas;

public class CreateFeedbackModel
{
    public int ClubId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}