namespace WebScrp.DTOs;

public class InputDataDto
{
    public string Selector { get; set; }

    public string Attribute { get; set; }

    public string UrlBase64 { get; set; }

    public string EncryptedTextBytesBase64 { get; set; }

    public string KeyBytesBase64 { get; set; }

    public string PageBase64 { get; set; }
}