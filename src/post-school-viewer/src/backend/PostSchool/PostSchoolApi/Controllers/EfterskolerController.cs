using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PostSchoolApi.Models;

namespace PostSchoolApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EfterskolerController : ControllerBase
{
    private Dictionary<int, Efterskole> _efterskoler;

    public EfterskolerController()
    {
        var efterskolder = JsonSerializer.Deserialize<List<Efterskole>>(System.IO.File.ReadAllText("MakePostSchoolData1.json"));
        _efterskoler = efterskolder?.ToDictionary(x => x.Id) ?? [];
    }

    [HttpGet]
    public ActionResult<IEnumerable<Efterskole>> GetEfterskoler()
    {
        return _efterskoler.Values;
    }

    [HttpPut("{id}")]
    public IActionResult UpdateEfterskole(int id, Efterskole efterskole)
    {
        if (id != efterskole.Id) return BadRequest();

        _efterskoler[id] = efterskole;

        return NoContent();
    }
}