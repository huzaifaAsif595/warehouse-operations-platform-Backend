using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PeakLogix.EntityFramework.Entities.PickProSD;
using PeakLogix.PickProApi.Common.DTOs.PackageLabels;
using PeakLogix.PickProApi.Controllers.Api;
using PeakLogix.PickProApi.Services.PackageLabels.Interfaces;
using PeakLogix.PickProApi.Startup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Controllers.PackageLabels
{
	[ApiController]
	[Hateoas("packageLabels")]
	[Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes = AuthConstants.BasicAuthenticationScheme)]
	public class PackageLabelsController : ControllerBase
	{
		private readonly IPackageLabelsService _packageLabelsService;
		private readonly ILogger<PackageLabelsController> _logger;

		public PackageLabelsController(IPackageLabelsService packageLabelsService, ILogger<PackageLabelsController> logger)
		{
			_packageLabelsService = packageLabelsService;
			_logger = logger;
		}

		[HttpGet("[action]/{orderNumber}")]
		public async Task<ActionResult<List<DateTime>>> GetRequiredDatesForOrder(string orderNumber)
		{
			var requiredDates = await _packageLabelsService.GetRequiredDatesForOrder(orderNumber);

			if (requiredDates?.Count > 0)
				return Ok(requiredDates);

			return StatusCode(404, $"Order {orderNumber} not found.");
		}

		[HttpGet("[action]")]
		public async Task<ActionResult<List<PkgContainerTypeDto>>> GetAllPackageLabelTypes()
		{
			var packageLabelTypes = await _packageLabelsService.GetAllPackageLabelTypes();

			if (packageLabelTypes?.Count > 0)
				return Ok(packageLabelTypes);

			return StatusCode(404, $"No {nameof(PkgContainerTypeDto)}s found.");
		}

		[HttpPost("[action]")]
		public async Task<ActionResult<List<PkgItemDto>>> GetPackageLabelItems([FromBody] PackageLabelsReq request)
		{
			var items = await _packageLabelsService.GetPackageLabelItems(request);

			if (items?.Count > 0)
				return Ok(items);

			return StatusCode(404, $"No package label items found for order {request.OrderNumber} on {request.RequiredDate:d}.");
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SavePackageItem([FromBody] PackageItem packageItem)
		{
			await _packageLabelsService.SavePackageItem(packageItem);
			return Ok();
		}

		[HttpDelete("[action]/{id}")]
		public async Task<IActionResult> DeletePackageItem(Guid id)
		{
			await _packageLabelsService.DeletePackageItem(id);
			return Ok();
		}

		[HttpPost("[action]")]
		public async Task<IActionResult> SavePackageContainer([FromBody] PkgContainerDto pkgContainerDto)
		{
			await _packageLabelsService.SavePackageContainer(pkgContainerDto);
			return Ok();
		}

		[HttpDelete("[action]/{id}")]
		public async Task<IActionResult> DeletePackageContainer(Guid id)
		{
			await _packageLabelsService.DeletePackageContainer(id);
			return Ok();
		}

		[HttpGet("[action]/{itemNumber}")]
		public async Task<ActionResult<PackageItemEditDto>> GetPackageItemForEdit(string itemNumber)
		{
			var packageItemEditDto = await _packageLabelsService.GetPackageItemForEdit(itemNumber);
			return Ok(packageItemEditDto);
		}
	}
}
