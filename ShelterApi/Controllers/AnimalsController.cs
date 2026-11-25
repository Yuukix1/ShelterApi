using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Helpers;
using ShelterApi.Models;
using ShelterApi.Models.DTOs;

namespace ShelterApi.Controllers;

[ApiController]
[Route("animals")]
public class AnimalsController : ControllerBase
{
    private readonly AnimalshelterContext _context;

    public AnimalsController(AnimalshelterContext context)
    {
        _context = context;
    }

    [HttpPost("newAnimal")]
    public async Task<IActionResult> NewAnimal([FromBody] NewAnimalDto dto)
    {
        try
        {
            var error = ApiResponse.ValidateAnimal(dto.Name, dto.Species, dto.Age, dto.Weight);
            if (error != null)
                return BadRequest(ApiResponse.Make(error));

            var shelter = await _context.Shelters.FirstOrDefaultAsync(s => s.Id == dto.ShelterId);
            if (shelter == null)
                return NotFound(ApiResponse.Make("A megadott menhely nem létezik."));

            var currentCount = await _context.Animals.CountAsync(a => a.ShelterId == dto.ShelterId);
            if (currentCount >= shelter.Capacity)
                return Conflict(ApiResponse.Make("A menhely megtelt, nem fér el több állat."));

            var animal = new Animal
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Species = dto.Species,
                Age = dto.Age,
                Weight = dto.Weight,
                ShelterId = dto.ShelterId,
                RegTime = DateTime.Now
            };

            _context.Animals.Add(animal);
            await _context.SaveChangesAsync();

            return StatusCode(201, ApiResponse.Make("Az állat sikeresen rögzítve.", animal));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpGet("allAnimal")]
    public async Task<IActionResult> AllAnimal()
    {
        try
        {
            var animals = await _context.Animals.ToListAsync();
            if (animals.Count == 0)
                return NotFound(ApiResponse.Make("Nincs egyetlen állat sem az adatbázisban."));

            return Ok(ApiResponse.Make("Az állatok sikeresen lekérdezve.", animals));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpGet("byId")]
    public async Task<IActionResult> ById([FromQuery] Guid id)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
                return NotFound(ApiResponse.Make("Nem található állat a megadott azonosítóval."));

            return Ok(ApiResponse.Make("Az állat sikeresen lekérdezve.", animal));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] UpdateAnimalDto dto)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
                return NotFound(ApiResponse.Make("Nem található állat a megadott azonosítóval."));

            var error = ApiResponse.ValidateAnimal(dto.Name, dto.Species, dto.Age, dto.Weight);
            if (error != null)
                return BadRequest(ApiResponse.Make(error));

            var shelter = await _context.Shelters.FirstOrDefaultAsync(s => s.Id == dto.ShelterId);
            if (shelter == null)
                return NotFound(ApiResponse.Make("A megadott menhely nem létezik."));

            if (animal.ShelterId != dto.ShelterId)
            {
                var currentCount = await _context.Animals.CountAsync(a => a.ShelterId == dto.ShelterId);
                if (currentCount >= shelter.Capacity)
                    return Conflict(ApiResponse.Make("A célmenhely megtelt, nem fér el több állat."));
            }

            animal.Name = dto.Name;
            animal.Species = dto.Species;
            animal.Age = dto.Age;
            animal.Weight = dto.Weight;
            animal.ShelterId = dto.ShelterId;

            await _context.SaveChangesAsync();

            return Ok(ApiResponse.Make("Az állat sikeresen módosítva.", animal));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpDelete("deleteAnimal")]
    public async Task<IActionResult> DeleteAnimal([FromQuery] Guid id)
    {
        try
        {
            var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
            if (animal == null)
                return NotFound(ApiResponse.Make("Nem található állat a megadott azonosítóval."));

            _context.Animals.Remove(animal);
            await _context.SaveChangesAsync();

            return Ok(ApiResponse.Make("Az állat sikeresen törölve.", animal));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }
}
