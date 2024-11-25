using jsreport.Shared;
using jsreport.Types;
using Microsoft.AspNetCore.Mvc;
using Razor.Templating.Core;
using Utility.Models.Admin.Download;

namespace API.Areas.Frontend
{
    [Route("webapi/[controller]")]
	[ApiController]
	public class PdfController : ControllerBase
	{
		private readonly IRenderService _render;

		public PdfController(IRenderService render)
		{
			_render = render;
		}

		/// <summary>
		/// Generate PDF file based on input html page content 
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		[HttpPost]
		[IgnoreAntiforgeryToken]
			public async Task<IActionResult> Convert([FromForm] string html)
			{
				var report = await _render.RenderAsync(new RenderRequest
				{
					Template = new   Template 
                    {
						Recipe = Recipe.ChromePdf,
						Engine = Engine.JsRender,
						Content = html
					}
				});

				return File(report.Content, "application/pdf", "file.pdf");
			}
 
		/// <summary>
		/// Generate PDF file based on Invoice data
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[IgnoreAntiforgeryToken]
		public async Task<FileStreamResult> DownloadPDF()
		{
			var invoiceModel = new  Invoice
			{
				InvoiceNumber = new Random().Next(1000,5000).ToString(),
				CreatedDate = DateTime.Now,
				DueDate = DateTime.Now.AddDays(7),
				CompanyAddress = new Address
				{
					Name = "MPP",
					AddressLine1 = "Sanabel Tower",
					City = "Sharq",
					Country = "Kuwait",
					Email = "info@mediaphonekwt.com",
					PinCode = "00000000"
				},
				BillingAddress = new	Address
				{
					Name = "Media Phone Plus",
					AddressLine1 = "Media",
					City = "Sharq",
					Country = "Kuwait",
					Email = "mpp-customer@gmail.com",
					PinCode = "12121212"
				},
				PaymentMethod = new PaymentMethod
				{
					Name = "Cash",
					ReferenceNumber = new Random().Next(12345,100000).ToString()
				},
				LineItems = new List<LineItem>
				{
					new LineItem { Id = 1, ItemName = "Keyboards", Quantity = 3, PricePerItem = 10.33M },
					new LineItem { Id = 2, ItemName = "USD 100G", Quantity = 10, PricePerItem = 90.54M }
				},
				CompanyLogoUrl = "https://www.mediaphoneplus.com/images/mpplogo.svg"
			};

			///its a template view, invoiceModel pass as parameter inside invoice.cshtml file
			var invoiceHtml = await RazorTemplateEngine.RenderAsync("/Views/invoice.cshtml", invoiceModel);
			var report = await _render.RenderAsync(new RenderRequest
			{
				Template = new Template
				{
					Recipe = Recipe.ChromePdf,
					Engine = Engine.JsRender,
					Content = invoiceHtml
				}
			});
			return File(report.Content, "application/pdf", "file.pdf");
		}
	}
}
