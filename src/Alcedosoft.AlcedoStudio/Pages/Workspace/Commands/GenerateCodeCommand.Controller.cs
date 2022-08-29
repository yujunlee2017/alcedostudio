namespace Alcedosoft.AlcedoStudio;

public partial class GenerateCodeCommand : Command
{
    public string GenerateController(string projectName, FileSchema schema)
    {
        string name = schema.Name;
        string nameLower = schema.Name.ToLower();

        return $@"namespace {projectName};

[ApiController]
[Route(""api/[controller]"")]
public class {name}Controller : ControllerBase
{{
    private readonly DataContext _context;

    public {name}Controller(DataContext context)
    {{
        _context = context;
    }}

    // GET: api/{name}
    [HttpGet]
    public async Task<ActionResult<IEnumerable<{name}>>> Get{name}()
    {{
        if (_context.{name} == null)
        {{
            return NotFound();
        }}

        return await _context.{name}.ToListAsync();
    }}

    // GET: api/{name}/5
    [HttpGet(""{{id}}"")]
    public async Task<ActionResult<{name}>> Get{name}(int id)
    {{
        if (_context.{name} == null)
        {{
            return NotFound();
        }}

        var {nameLower} = await _context.{name}.FindAsync(id);

        if ({nameLower} == null)
        {{
            return NotFound();
        }}

        return {nameLower};
    }}

    // PUT: api/{name}/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut(""{{id}}"")]
    public async Task<IActionResult> Put{name}(int id, {name} {nameLower})
    {{
        if (id != {nameLower}.Id)
        {{
            return BadRequest();
        }}

        _context.Entry({nameLower}).State = EntityState.Modified;

        try
        {{
            _ = await _context.SaveChangesAsync();
        }}
        catch (DbUpdateConcurrencyException)
        {{
            if (!{name}Exists(id))
            {{
                return NotFound();
            }}
            else
            {{
                throw;
            }}
        }}

        return NoContent();
    }}

    // POST: api/{name}
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<{name}>> Post{name}({name} {nameLower})
    {{
        if (_context.{name} == null)
        {{
            return Problem(""Entity set '{name}' is null."");
        }}

        _ = _context.{name}.Add({nameLower});
        _ = await _context.SaveChangesAsync();

        return CreatedAtAction(""Get{name}"", new {{ id = {nameLower}.Id }}, {nameLower});
    }}

    // DELETE: api/{name}/5
    [HttpDelete(""{{id}}"")]
    public async Task<IActionResult> Delete{name}(int id)
    {{
        if (_context.{name} == null)
        {{
            return NotFound();
        }}

        var {nameLower} = await _context.{name}.FindAsync(id);
        if ({nameLower} == null)
        {{
            return NotFound();
        }}

        _ = _context.{name}.Remove({nameLower});
        _ = await _context.SaveChangesAsync();

        return NoContent();
    }}

    private bool {name}Exists(int id)
    {{
        return (_context.{name}?.Any(e => e.Id == id)).GetValueOrDefault();
    }}
}}
";
    }
}
