namespace RR.App.Service.Interfaces;

public interface IOCRService
{
    string ExtractTextFromImage(byte[] imageBytes);
}
