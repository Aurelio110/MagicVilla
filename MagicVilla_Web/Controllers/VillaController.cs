using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private List<Villa> list = new();
        private readonly IVillaServices _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaServices villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> IndexVilla()
        {
            
            
            var response = await _villaService.GetAllAsync<APIResponse>();
            if (response == null || !response.IsSuccess)
            {

                return View(list);
            }
            if (response != null && response.IsSuccess == true)
            {
                var jsonString = JsonConvert.SerializeObject(response.Result);
                list = JsonConvert.DeserializeObject<List<Villa>>(jsonString);
            }
            return View(list);
        }

        public async Task<IActionResult> CreateVilla()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(Villa model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(model);
                
                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
                
            }
            

            return View(model);
        }

        //[HttpGet]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var response = await _villaService.GetAsync(id);
        //    Villa villa = null;
        //    if (response != null && response.IsSuccess)
        //    {
        //        villa = JsonConvert.DeserializeObject<Villa>(response.Result.ToString());
        //    }
        //    if (villa == null)
        //        return NotFound();
        //    return View(villa);
        //}
    }
}
