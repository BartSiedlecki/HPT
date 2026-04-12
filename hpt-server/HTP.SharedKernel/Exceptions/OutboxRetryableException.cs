namespace HPT.SharedKernel.Exceptions;

public class OutboxRetryableException(string message) : Exception(message);