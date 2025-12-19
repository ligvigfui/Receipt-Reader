namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(
    ICategoryService categoryService
) : ControllerBase
{
    [HttpPost("get")]
    [ProducesResponse<Category>()]
    public async Task<IActionResult> GetCategory(
        [FromBody] MinimalCategory category,
        [FromQuery] uint depth = 1) =>
        Ok(await categoryService.GetCategory(category, depth));

    [HttpPost("create")]
    [ProducesResponse<Category>()]
    public async Task<IActionResult> CreateCategory(
        [FromBody] Category category) =>
        CreatedAtAction(nameof(CreateCategory), await categoryService.CreateCategory(category));
}
