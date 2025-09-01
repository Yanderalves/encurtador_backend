namespace EncurtadorURL.Models;

public class URLModel
{
    public Guid Id;
    public string URLEncurtada { get; set; } = String.Empty;
    public string URLOriginal { get; set; } = String.Empty;

    public string ChaveEncurtada { get; set; } = String.Empty;


    public URLModel()
    {
    }
}