using WebScrp.DTOs;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Parser.Html;


namespace WebScrp.Repositores;



public interface IParserRepo
{
    Task<TransferDto> Parse(InputDataDto inputData);
}

public class ParserRepo : IParserRepo
{
    public async Task<TransferDto> Parse(InputDataDto inputData)
    {
        List<string> htmlValues = new List<string>();

        // тут обработать исключения

        byte[] urlBytes = Convert.FromBase64String(inputData.UrlBase64);
        byte[] pageBytes = Convert.FromBase64String(inputData.PageBase64);
        
        string url = Encoding.UTF8.GetString(urlBytes);
        var page = Encoding.UTF8.GetString(pageBytes);

        var htmlParser = new HtmlParser();
        var document = await htmlParser.ParseAsync(page);

        var elements = document.QuerySelectorAll(inputData.Selector);

        int elemCount = elements.Count();

        foreach (var elem in elements)
        {
            var cleanText = string.Join(" ", elem.TextContent.Split(new[] { ' ', '\n', '\r', '\t' }));
            htmlValues.Add(cleanText);

        }

        var emailRegex = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");

        var emails = document.All
                             .Where(element => !string.IsNullOrEmpty(element.TextContent) && element.TextContent.Contains('@'))
                             .SelectMany(elem => emailRegex.Matches(elem.TextContent)).Select(m => m.Value)
                             .Distinct()
                             .ToList();

        return new TransferDto
        {
            FromUrl = url,
            Page = page,
            elemCount = elemCount,
            HtmlValues = htmlValues,
            Emails = emails
        };
    }
}