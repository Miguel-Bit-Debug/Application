namespace Application.Domain.DTOs.Request;

public record RegisterRequest(string Name, string Email, string Password, bool IsAdmin);