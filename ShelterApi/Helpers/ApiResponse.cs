namespace ShelterApi.Helpers;

public static class ApiResponse
{
    public static object Make(string message, object? result = null) =>
        new { message, result };

    public static string? ValidateAnimal(string name, string species, int age, double weight)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > 40)
            return "A név megadása kötelező, és legfeljebb 40 karakter lehet.";
        if (string.IsNullOrWhiteSpace(species) || species.Length > 30)
            return "A faj megadása kötelező, és legfeljebb 30 karakter lehet.";
        if (age < 0)
            return "A kor nem lehet negatív.";
        if (weight <= 0)
            return "A súlynak pozitívnak kell lennie.";
        return null;
    }
}
