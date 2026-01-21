using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebApplication1;

public class AuthML(RequestDelegate next)
{

    public async Task InvokeAsync(HttpContext context)
    {

        await next(context);
    }
}


public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    
    public async Task InvokeAsync(HttpContext context)
    {

        try
        {
            await next(context);
        }
        catch (NotFoundException e)
        {
            await WriteResponse(context, new ErrorResponse(e.Message), HttpStatusCode.NotFound);
        }
        catch (FileNotFoundException e)
        {
            await WriteResponse(context, new ErrorResponse(e.Message), HttpStatusCode.NotFound);
        }
        catch (TaskCanceledException e) when (e.Message.Contains("HttpClient.Timeout"))
        {
            await WriteResponse(context, new ErrorResponse(e.Message), HttpStatusCode.GatewayTimeout);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Exception caught in middleware: {Message}", e.Message);
            if (e.InnerException != null)
            {
                logger.LogError(e.InnerException, "Inner exception caught in middleware: {Message}", e.InnerException.Message);
            }
            await WriteResponse(context, new ErrorResponse(e.Message, e.StackTrace), HttpStatusCode.InternalServerError);
        }
    }

    private static async Task WriteResponse(HttpContext context, ErrorResponse responseBody, HttpStatusCode statusCode)
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(responseBody, options));
    }
}

public record ErrorResponse(string Message, string? StackTrace = null);