namespace CarRental.Automotor.Domain.Exceptions
{
    public sealed class BusinessException(string message) : Exception(message)
    {
    }
}
