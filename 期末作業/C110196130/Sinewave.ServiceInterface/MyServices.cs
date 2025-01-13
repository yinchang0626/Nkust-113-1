using ServiceStack;
using Sinewave.ServiceModel;

namespace Sinewave.ServiceInterface;

public class MyServices : Service
{
    public object Any(Hello request)
    {
        return new HelloResponse { Result = $"Hello, {request.Name}!" };
    }
}