using System;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Mvc;
using WebScrp.DTOs;




namespace WebScrp.Controllers;

[ApiController]
[Route("[controller]")]
public class PageHandlerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> ScrapePage(InputDataDto inputData) 
    {
        List<string> htmlValues = new List<string>();
        var builder = new StringBuilder();


        byte[] urlBytes = Convert.FromBase64String(inputData.UrlBase64);
        byte[] pageBytes = Convert.FromBase64String(inputData.PageBase64);
        string url = Encoding.UTF8.GetString(urlBytes);
        var page = Encoding.UTF8.GetString(pageBytes);


        var htmlParser = new HtmlParser();
        var document = await htmlParser.ParseAsync(page);

        var elements = document.QuerySelectorAll(inputData.Selector);

        int elemCount = elements.Count();

        foreach(var elem in elements)
        {
            var cleanText = string.Join(" ", elem.TextContent.Split(new[] {' ', '\n', '\r', '\t'}));
            // htmlValues.Add(elem.TextContent.Replace(" ", ""));
            htmlValues.Add(cleanText);

        }

        var emailRegex = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");


        // поменять на JOIN
        var emails = document.All
                             .Where(element => !string.IsNullOrEmpty(element.TextContent) && element.TextContent.Contains('@'))
                             .SelectMany(elem => emailRegex.Matches(elem.TextContent)).Select(m => m.Value)
                             .Distinct()
                             .ToList();

        return Ok(new
        {
            Url = url,
            Page = page,
            ElementsCount = elemCount,
            Values = htmlValues,
            Emails = emails
        });
    }
}
