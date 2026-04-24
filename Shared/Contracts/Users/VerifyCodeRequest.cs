namespace Shared.Contracts.Users;

public record VerifyCodeRequest(string PhoneNumber, string Code);