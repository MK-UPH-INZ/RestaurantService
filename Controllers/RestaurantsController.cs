using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Data;
using RestaurantService.DTO;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepo repository;
        private readonly IMapper mapper;

        public RestaurantsController(
            IRestaurantRepo repository,
            IMapper mapper
        )
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantReadDTO>> GetRestaurants()
        {
            Console.WriteLine("--> Getting Restaurants...");

            var restaurantItems = repository.GetAllRestaurants();

            return Ok(
                mapper.Map<IEnumerable<RestaurantReadDTO>>(restaurantItems)
            );
        }

        [HttpGet("{id}", Name = "GetRestaurantById")]
        public ActionResult<RestaurantReadDTO> GetRestaurantById(int id)
        {
            var restaurantItem = repository.GetRestaurantById(id);

            if(restaurantItem == null )
            {
                return NotFound();
            }
            return Ok(mapper.Map<RestaurantReadDTO>(restaurantItem));
        }

        [HttpPut("{id}", Name = "UpdateRestaurantById")]
        public ActionResult<RestaurantReadDTO> UpdateRestaurantById(RestaurantUpdateDTO restaurantUpdateDTO, int id)
        {
            var restaurantItem = repository.GetRestaurantById(id);

            if (restaurantItem == null)
            {
                return NotFound();
            }

            mapper.Map<RestaurantUpdateDTO, Restaurant>(restaurantUpdateDTO, restaurantItem);
            repository.SaveChanges();

            return Ok(mapper.Map<RestaurantReadDTO>(restaurantItem));
        }

        [HttpPost]
        public ActionResult<RestaurantReadDTO> CreateRestaurant(RestaurantCreateDTO restaurantCreateDTO)
        {
            var restaurantModel = mapper.Map<Restaurant>(restaurantCreateDTO);
            repository.CreateRestaurant(restaurantModel);
            repository.SaveChanges();

            var restaurantReadDTO = mapper.Map<RestaurantReadDTO>(restaurantModel);

            return CreatedAtRoute(
                nameof(GetRestaurantById), 
                new { Id = restaurantReadDTO.Id }, restaurantReadDTO
            );
        }

        [HttpDelete("{id}", Name = "DeleteRestaurantById")]
        public ActionResult DeleteRestaurant(int id)
        {
            var restaurantItem = repository.GetRestaurantById(id);

            if( restaurantItem == null )
            {
                return NotFound();
            }

            repository.RemoveRestaurant(restaurantItem);
            repository.SaveChanges();

            return NoContent();
        }
    }
}
