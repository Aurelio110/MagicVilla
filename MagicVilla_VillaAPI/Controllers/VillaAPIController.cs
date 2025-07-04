using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;


namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/villaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO?> GetVilla(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad Request");
            }
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound("Not Found");
            }
            return Ok(villa);


        }
        [HttpPost]
        //[Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO)
        {
            
            if (_db.Villas.FirstOrDefault(v => v.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists with this name");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest("Bad Request");
            }

            Villa model = new()
            {
                Name = villaDTO.Name,
                Sqm = villaDTO.Sqm,
                Occupancy = villaDTO.Occupancy,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Amenity = villaDTO.Amenity,
                Rate = villaDTO.Rate,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };

            villaDTO.Id = _db.Villas.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
            _db.Villas.Add(model);
            _db.SaveChanges();
            return Ok(model);
        }
        [HttpDelete("id")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteVillaById(int id)
        {
            if (id == 0)
            {
                return BadRequest("Bad Request");
            }
            Villa? model = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (model == null)
            {
                return NotFound("Not Found");
            }
            _db.Villas.Remove(model);
            _db.SaveChanges();
            return Content("Villa has been deleted");
        }
        [HttpDelete("name")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult DeleteVillaByName(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return BadRequest("Bad Request");
            }
            Villa? model = _db.Villas.FirstOrDefault(v => v.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
            if (model == null)
            {
                return NotFound("Not Found");
            }
            _db.Villas.Remove(model);
            _db.SaveChanges();
            return Ok("Villa has been deleted");
        }
        [HttpPut("id")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdateVillaById(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest("Bad Request");
            }

            Villa? existingVilla = _db.Villas.FirstOrDefault(v => v.Id == id);
            existingVilla.Name = villaDTO.Name;
            existingVilla.Sqm = villaDTO.Sqm;
            existingVilla.Occupancy = villaDTO.Occupancy;

            if (existingVilla == null)
            {
                return NotFound("Not Found");
            }
            _db.Villas.Update(existingVilla);
            _db.SaveChanges();
            return Ok(existingVilla);
        }

        [HttpPatch("id")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateVillaPartial(int id, JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0)
            {
                return BadRequest("Bad Request");
            }
            Villa? villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
            {
                return NotFound("Not Found");
            }
            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Name = villa.Name,
                Sqm = villa.Sqm,
                Occupancy = villa.Occupancy,
                Details = villa.Details,
                ImageUrl = villa.ImageUrl,
                Amenity = villa.Amenity,
                Rate = villa.Rate
            };
            patchDTO.ApplyTo(villaDTO);
            if (villaDTO == null)
            {
                return BadRequest();
            }
            villa.Name = villaDTO.Name;
            villa.Sqm = villaDTO.Sqm;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Details = villaDTO.Details;
            villa.ImageUrl = villaDTO.ImageUrl;
            villa.Amenity = villaDTO.Amenity;
            villa.Rate = villaDTO.Rate;

            _db.Villas.Update(villa);
            _db.SaveChanges();

            return Ok(villa);
        }
    }
}
