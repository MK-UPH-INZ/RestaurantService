using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.AsyncDataServices;
using RestaurantService.Data;
using RestaurantService.DTO;
using RestaurantService.Models;
using RestaurantService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepo repository;
        private readonly IMapper mapper;
        private readonly IUserDataClient userDataClient;
        private readonly IMessageBusClient messageBusClient;

        public RestaurantsController(
            IRestaurantRepo repository,
            IMapper mapper,
            IUserDataClient userDataClient,
            IMessageBusClient messageBusClient
        )
        {
            this.repository = repository;
            this.mapper = mapper;
            this.userDataClient = userDataClient;
            this.messageBusClient = messageBusClient;
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
        [Authorize]
        public ActionResult<RestaurantReadDTO> UpdateRestaurantById(RestaurantUpdateDTO restaurantUpdateDTO, int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return Unauthorized();

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = identity.FindFirst(ClaimTypes.Role);

            if (userId == null || userRole == null)
                return Unauthorized();

            var restaurantItem = repository.GetRestaurantById(id);

            if (restaurantItem == null)
                return NotFound();

            if (userRole.Value != "ADMIN")
                return Unauthorized();

            mapper.Map<RestaurantUpdateDTO, Restaurant>(restaurantUpdateDTO, restaurantItem);
            repository.SaveChanges();

            return Ok(mapper.Map<RestaurantReadDTO>(restaurantItem));
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<RestaurantReadDTO>> CreateRestaurant(RestaurantCreateDTO restaurantCreateDTO)
        {
            var restaurantModel = mapper.Map<Restaurant>(restaurantCreateDTO);
            repository.CreateRestaurant(restaurantModel);
            repository.SaveChanges();

            var restaurantReadDTO = mapper.Map<RestaurantReadDTO>(restaurantModel);

            // Send Sync
            try
            {
                await userDataClient.SendRestaurantToUser(restaurantReadDTO);
            } catch( Exception e)
            {
                Console.WriteLine(e);
            }
            
            // Send Async
            try
            {
                var restaurantPublishedDTO = mapper.Map<RestaurantPublishedDTO>(restaurantReadDTO);
                restaurantPublishedDTO.Event = "Rstaurant_Published";
                messageBusClient.PublishNewRestaurant(restaurantPublishedDTO);
            } catch ( Exception e )
            {
                Console.WriteLine(e);
            }

            return CreatedAtRoute(
                nameof(GetRestaurantById), 
                new { Id = restaurantReadDTO.Id }, restaurantReadDTO
            );
        }

        [HttpDelete("{id}", Name = "DeleteRestaurantById")]
        [Authorize]
        public ActionResult DeleteRestaurant(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity == null)
                return Unauthorized();

            var userId = identity.FindFirst(ClaimTypes.NameIdentifier);
            var userRole = identity.FindFirst(ClaimTypes.Role);

            if (userId == null || userRole == null)
                return Unauthorized();

            var restaurantItem = repository.GetRestaurantById(id);

            if( restaurantItem == null )
            {
                return NotFound();
            }

            if (userRole.Value != "ADMIN")
                return Unauthorized();

            repository.RemoveRestaurant(restaurantItem);
            repository.SaveChanges();

            return NoContent();
        }
    }
}
