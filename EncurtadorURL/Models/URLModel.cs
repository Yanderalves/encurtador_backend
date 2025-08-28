namespace EncurtadorURL.Models;

public class URLModel
{
    public Guid Id;
    public string URLEncurtada { get; set; }
    public string URLOriginal { get; set; }

    public string ChaveEncurtada { get; set; }


    public URLModel()
    {
    }
}