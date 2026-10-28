namespace WebScrp.DTOs;

public class TransferDto 
{
    public string FromUrl {get; set;}
    public string Page {get; set;}
    public int elemCount {get; set;}
    public List<string> HtmlValues {get; set;}
    public List<string> Emails {get; set;}
}
