namespace Zamp.Server.Infrastructure.Data;

public class DataAccessException(string message, Exception innerException) 
    : Exception(message, innerException);