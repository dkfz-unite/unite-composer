namespace Unite.Composer.Extensions;

public static class ExceptionExtensions
{
    public static string GetShortMessage(this Exception ex)
    {
        return GetShortMessage(ex, "");
    }

    private static string GetShortMessage(Exception exception, string message = "")
    {
        var separator = Environment.NewLine;

        if (exception.StackTrace != null)
            message += $"{exception.GetType().Name} - {exception.Message}{separator}{exception.StackTrace.Split(separator).First()}";
        else
            message += $"{exception.GetType().Name} - {exception.Message}";

        if (exception.InnerException != null)
        {
            return GetShortMessage(exception.InnerException, message + separator);
        }

        return message;
    }
}
