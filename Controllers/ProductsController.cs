using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.AsyncDataServices;
using RestaurantService.Data;
using RestaurantService.DTO.Product;
using RestaurantService.DTO.Product.Events;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo productRepository;
        private readonly IMapper mapper;
        private readonly IMessageBusClient messageBusClient;

        public ProductsController(
            IProductRepo productRepository,
            IMapper mapper,
            IMessageBusClient messageBusClient
        )
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.messageBusClient = messageBusClient;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDTO>> GetProducts(int? restaurantId)
        {

            var products = (restaurantId == null) 
                ? productRepository.GetAllProducts() 
                : productRepository.GetProductsByRestaurant(restaurantId.Value);

            return Ok(
                mapper.Map<IEnumerable<ProductReadDTO>>(products)
            );
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}", Name = "GetProductById")]
        public ActionResult<ProductReadDTO> GetProductById(int id)
        {
            var product = productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            return Ok(
                mapper.Map<ProductReadDTO>(product)
            );
        }

        // POST api/<ProductsController>
        [HttpPost]
        [Authorize]
        public ActionResult<ProductReadDTO> CreateProduct(ProductCreateDTO productCreateDTO)
        {
            var productModel = mapper.Map<Product>(productCreateDTO);
            productRepository.CreateProduct(productModel);
            productRepository.SaveChanges();

            var product = productRepository.GetProductById(productModel.Id);

            var productReadDTO = mapper.Map<ProductReadDTO>(product);

            try
            {
                var productPublishedDTO = mapper.Map<ProductPublishedDTO>(product);
                messageBusClient.PublishNewProduct(productPublishedDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return CreatedAtRoute(
                nameof(GetProductById),
                new { Id = productReadDTO.Id }, productReadDTO
            );
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}", Name = "UpdateProductById")]
        [Authorize]
        public ActionResult<ProductReadDTO> UpdateProductById(int id, [FromBody] ProductUpdateDTO productUpdateDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var product = productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            if (!isOwnerOfResource(product, identity))
                return Unauthorized();

            mapper.Map<ProductUpdateDTO, Product>(productUpdateDTO, product);
            productRepository.SaveChanges();

            try
            {
                var productUpdatedDTO = mapper.Map<ProductUpdatedDTO>(product);
                messageBusClient.UpdateProduct(productUpdatedDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Ok(mapper.Map<ProductReadDTO>(product));
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}", Name = "DeleteProductById")]
        public ActionResult DeleteProduct(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var product = productRepository.GetProductById(id);

            if (product == null)
                return NotFound();

            if (!isOwnerOfResource(product, identity))
                return Unauthorized();

            var productId = product.Id;

            productRepository.RemoveProduct(product);
            productRepository.SaveChanges();

            try
            {
                messageBusClient.DeleteProduct(new ProductDeletedDTO()
                {
                    Id = productId
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NoContent();
        }

        private bool isOwnerOfResource(Product resource, ClaimsIdentity identity)
        {
            if (identity == null)
                return false;

            var userId = getIdentityId(identity);
            var userRole = getIdentityRole(identity);

            if (userId == 0 || userRole == "")
                return false;

            if (userRole == "ADMIN")
                return true;

            return resource.Restaurant.Owner.ExternalId == userId;
        }

        private int getIdentityId(ClaimsIdentity identity)
        {
            var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return 0;

            return int.Parse(userIdClaim.Value);
        }

        private string getIdentityRole(ClaimsIdentity identity)
        {
            var userRoleClaim = identity.FindFirst(ClaimTypes.Role);

            if (userRoleClaim == null)
                return "";

            return userRoleClaim.Value;
        }
    }
}
