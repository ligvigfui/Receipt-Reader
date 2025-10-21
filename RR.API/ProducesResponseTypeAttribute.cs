namespace RR.API;

public class ProducesResponseAttribute<T>(HttpStatusCode statusCode = HttpStatusCode.OK) : ProducesResponseTypeAttribute(typeof(T), (int)statusCode)
{
}
public class ProducesResponseAttribute(HttpStatusCode statusCode = HttpStatusCode.OK) : ProducesResponseTypeAttribute((int)statusCode)
{
}
