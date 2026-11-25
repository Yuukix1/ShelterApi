using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShelterApi.Helpers;
using ShelterApi.Models;

namespace ShelterApi.Controllers;

[ApiController]
[Route("queries")]
public class QueriesController : ControllerBase
{
    private readonly AnimalshelterContext _context;

    public QueriesController(AnimalshelterContext context)
    {
        _context = context;
    }

    [HttpGet("allSheltersWithAnimals")]
    public async Task<IActionResult> AllSheltersWithAnimals()
    {
        try
        {
            var shelters = await _context.Shelters
                .Include(s => s.Animals)
                .ToListAsync();

            return Ok(ApiResponse.Make("A menhelyek az állataikkal sikeresen lekérdezve.", shelters));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpGet("allAnimalsWithShelter")]
    public async Task<IActionResult> AllAnimalsWithShelter()
    {
        try
        {
            var animals = await _context.Animals
                .Join(_context.Shelters,
                    a => a.ShelterId,
                    s => s.Id,
                    (a, s) => new
                    {
                        a.Id,
                        a.Name,
                        a.Species,
                        a.Age,
                        a.Weight,
                        a.RegTime,
                        ShelterName = s.Name
                    })
                .ToListAsync();

            return Ok(ApiResponse.Make("Az állatok a menhelyük nevével sikeresen lekérdezve.", animals));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpGet("animalsOfShelter")]
    public async Task<IActionResult> AnimalsOfShelter([FromQuery] Guid shelterId)
    {
        try
        {
            var shelter = await _context.Shelters
                .Include(s => s.Animals)
                .Where(s => s.Id == shelterId)
                .FirstOrDefaultAsync();

            if (shelter == null)
                return NotFound(ApiResponse.Make("Nem található menhely a megadott azonosítóval."));

            return Ok(ApiResponse.Make("A menhely és állatai sikeresen lekérdezve.", shelter));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }

    [HttpGet("animalCountByShelter")]
    public async Task<IActionResult> AnimalCountByShelter()
    {
        try
        {
            var counts = await _context.Shelters
                .Select(s => new
                {
                    ShelterId = s.Id,
                    ShelterName = s.Name,
                    AnimalCount = s.Animals.Count()
                })
                .OrderByDescending(x => x.AnimalCount)
                .ToListAsync();

            return Ok(ApiResponse.Make("Az állatok száma menhelyenként sikeresen lekérdezve.", counts));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse.Make("Hiba történt: " + ex.Message));
        }
    }
}
