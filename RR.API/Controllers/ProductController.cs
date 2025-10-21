namespace RR.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(
    IProductService productService
) : ControllerBase
{
    [HttpPost("GetFiltered")]
    [ProducesResponse<List<Product>>]
    [ProducesResponse<GlobalErrorResponse>(HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync([FromBody] SortPageFilter<Product> sortPageFilter)
    {
        var products = await productService.GetProductsAsync(sortPageFilter);
        return Ok(products);
    }

    //[HttpPatch("Create")]
    //[ProducesResponse<Product>(HttpStatusCode.Created)]
    //[ProducesResponse<GlobalErrorResponse>(HttpStatusCode.BadRequest)]
    //public async Task<IActionResult> CreateAsync([FromBody][Required] Product product)
    //{
    //    var newProduct = await productService.CreateProductAsync(product);
    //    return CreatedAtAction(nameof(CreateAsync), newProduct);
    //}
}
