namespace CarRental.Automotor.Infrastructure.Middlewares
{
    public sealed class BusinessException(string message) : Exception(message)
    {
    }
}
