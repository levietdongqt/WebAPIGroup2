using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPIGroup2.Models.DTO;
using WebAPIGroup2.Service.Inteface;

namespace WebAPIGroup2.Controllers.HomeModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly ISizeService _sizeService;

        public SizeController(ISizeService sizeService)
        {
            _sizeService = sizeService;
        }
        [HttpGet]
        public async Task<JsonResult> GetAll()
        {
            var sizeDTOs = await _sizeService.GetAll();
            var response = new ResponseDTO<List<SizeDTO>>(HttpStatusCode.OK,"Get successfully",null,sizeDTOs.ToList());
            return new JsonResult(response);
        }


    }
}
