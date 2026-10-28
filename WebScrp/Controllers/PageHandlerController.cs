using System;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using AngleSharp.Dom;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Microsoft.AspNetCore.Mvc;
using WebScrp.DTOs;
using System.Security.Cryptography;
using WebScrp.Repositores;

namespace WebScrp.Controllers;

[ApiController]
[Route("[controller]")]
public class PageHandlerController : ControllerBase
{
    private readonly IParserRepo _parserService;
    private readonly IDecryptorRepo _decryptorService;
    public PageHandlerController(IParserRepo parserService, IDecryptorRepo decryptorRepo) 
    {
        _parserService = parserService;
        _decryptorService = decryptorRepo;
    }

    [HttpPost]
    public async Task<IActionResult> ScrapePage(InputDataDto inputData) 
    {

        // проверить на отсутствие параметров в объекте
        var dto = await _parserService.Parse(inputData);
        var decryptedText = await _decryptorService.Decrypt(inputData);

        if (dto is null || decryptedText is null)
        {
            return BadRequest();
        }


        return Ok(new
        {
            Url = dto.FromUrl,
            Page = dto.Page,
            ElementsCount = dto.elemCount,
            Values = dto.HtmlValues,
            Emails = dto.Emails,
            DecryptedMessage = decryptedText,
        });
    }
}
