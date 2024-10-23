using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class PdfController : ControllerBase
{
    private readonly IConverter _converter;

    public PdfController(IConverter converter)
    {
        _converter = converter;
    }

    [HttpGet("/")]
    public IActionResult Teste(){
        return Ok("Teste");
    }

    [HttpPost("convert")]
    public IActionResult ConvertHtmlToPdf([FromBody] Base64HtmlRequest request)
    {
        try
        {
            // Decodifica o HTML em Base64
            var html = Encoding.UTF8.GetString(Convert.FromBase64String(request.Base64Html));

            // Configura o documento PDF
            var pdfDocument = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                },
                Objects = {
                    new ObjectSettings() {
                        HtmlContent = html,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            // Converte para PDF
            byte[] pdf = _converter.Convert(pdfDocument);

            // Retorna o PDF em Base64
            string base64Pdf = Convert.ToBase64String(pdf);
            return Ok(new { Base64Pdf = base64Pdf });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}

public class Base64HtmlRequest
{
    public string Base64Html { get; set; }
}
