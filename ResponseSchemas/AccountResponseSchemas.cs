using CompClubAPI.Models;

namespace CompClubAPI.ResponseSchema;

public class BalanceHistoryResponse
{
    public List<BalanceHistory> History {get; set;}
}

public class MessageResponse
{
    public string Message {get; set;}
}

public class ErrorResponse
{
    public string Error {get; set;}
}