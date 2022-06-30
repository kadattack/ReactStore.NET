using PowerReact.Data;
using PowerReact.DTOs;
using PowerReact.Entities;
using PowerReact.Extensions;
using PowerReact.RequestHelpers;
using PowerReact.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PowerReact.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ImageService _imageService;

        public ProductsController(DataContext context, IMapper mapper, ImageService imageService)
        {
            _imageService = imageService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetProducts([FromQuery] ProductParams productParams)
        {
            var query = _context.Products
                .Sort(productParams.OrderBy)
                .Search(productParams.SearchTerm)
                .Filter(productParams.Brands, productParams.Types)
                .AsQueryable();

            var products = await PagedList<Product>.ToPagedList(query,
                productParams.PageNumber, productParams.PageSize);

            Response.AddPaginationHeader(products.MetaData);

            return products;
        }

        [HttpGet("{id}", Name = "GetProduct")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();

            return Ok(new { brands, types });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct([FromForm]CreateProductDto productDto)
        {
            var product = _mapper.Map<Product>(productDto);

            if (productDto.File != null)
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (!imageResult)
                    return BadRequest(new ProblemDetails{Title = "Image doesn't exist"});
                var domain = Request.GetDisplayUrl().Split("/");
                var domainname= domain[0];

                product.PictureUrl = Path.Combine(domainname, "Images", productDto.File.Name);
                product.PublicId = product.Id.ToString();
            }

            _context.Products.Add(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);

            return BadRequest(new ProblemDetails { Title = "Problem creating new product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct([FromForm]UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(productDto.Id);

            if (product == null) return NotFound();

            _mapper.Map(productDto, product);

            if (productDto.File != null) 
            {
                var imageResult = await _imageService.AddImageAsync(productDto.File);

                if (!imageResult)
                    return BadRequest(new ProblemDetails{Title = "Image doesn't exist"});

                if (!string.IsNullOrEmpty(product.PublicId)) 
                    await _imageService.DeleteImageAsync(product.PublicId);

                var domain = Request.GetDisplayUrl().Split("/");
                var domainname= domain[0];

                product.PictureUrl = Path.Combine(domainname, "Images", productDto.File.Name);
                product.PublicId = product.Id.ToString();
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok(product);

            return BadRequest(new ProblemDetails { Title = "Problem updating product" });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.PublicId)) 
                await _imageService.DeleteImageAsync(product.PublicId);

            _context.Products.Remove(product);

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem deleting product" });
        }



    }
}