namespace Application.Domain.DTOs;

public record RegisterRequest(string Name, string Email, string Password, bool IsAdmin);