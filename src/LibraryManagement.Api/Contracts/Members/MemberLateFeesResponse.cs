namespace LibraryManagement.Api.Contracts.Members
{
    public class MemberLateFeesResponse
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = "EUR";
    }
}