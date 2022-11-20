using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.AsyncDataServices;
using RestaurantService.Data;
using RestaurantService.DTO.Restaurant;
using RestaurantService.DTO.Restaurant.Events;
using RestaurantService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantService.Controllers
{
    [EnableCors("Api")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantRepo restaurantRepository;
        private readonly IUserRepo userRepository;
        private readonly IMapper mapper;
        private readonly IMessageBusClient messageBusClient;

        public RestaurantsController(
            IRestaurantRepo restaurantRepository,
            IUserRepo userRepository,
            IMapper mapper,
            IMessageBusClient messageBusClient
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<RestaurantReadDTO>> GetRestaurants()
        {
            Console.WriteLine("--> Getting Restaurants...");

            var restaurantItems = restaurantRepository.GetAllRestaurants();

            return Ok(
                mapper.Map<IEnumerable<RestaurantReadDTO>>(restaurantItems)
            );
        }

        [HttpGet("{id}", Name = "GetRestaurantById")]
        public ActionResult<RestaurantReadDTO> GetRestaurantById(int id)
        {
            var restaurantItem = restaurantRepository.GetRestaurantById(id);

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
            var restaurantItem = restaurantRepository.GetRestaurantById(id);
            var userRole = getIdentityRole(identity);

            if (restaurantItem == null)
                return NotFound();

            if (!isOwnerOfResource(restaurantItem, identity))
                return Unauthorized();

            if (restaurantUpdateDTO.OwnerId != null && userRole == "ADMIN")
            {
                var newOwner = userRepository.GetUserByExternalId(restaurantUpdateDTO.OwnerId.Value);

                if (newOwner != null)
                    restaurantItem.Owner = newOwner;
            }

            mapper.Map<RestaurantUpdateDTO, Restaurant>(restaurantUpdateDTO, restaurantItem);
            restaurantRepository.SaveChanges();

            try
            {
                var restaurantUpdatedDTO = mapper.Map<RestaurantUpdatedDTO>(restaurantItem);
                messageBusClient.UpdateRestaurant(restaurantUpdatedDTO);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Ok(mapper.Map<RestaurantReadDTO>(restaurantItem));
        }

        [HttpPost]
        [Authorize]
        public ActionResult<RestaurantReadDTO> CreateRestaurant(RestaurantCreateDTO restaurantCreateDTO)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userId = getIdentityId(identity);

            var user = userRepository.GetUserByExternalId(userId);

            if (user == null)
                return Unauthorized();

            var restaurantModel = mapper.Map<Restaurant>(restaurantCreateDTO);
            restaurantModel.Owner = user;
            restaurantRepository.CreateRestaurant(restaurantModel);
            restaurantRepository.SaveChanges();

            var restaurantReadDTO = mapper.Map<RestaurantReadDTO>(restaurantModel);
            
            // Send Async
            try
            {
                var restaurantPublishedDTO = mapper.Map<RestaurantPublishedDTO>(restaurantReadDTO);
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
            var restaurantItem = restaurantRepository.GetRestaurantById(id);

            if( restaurantItem == null )
                return NotFound();

            if (!isOwnerOfResource(restaurantItem, identity))
                return Unauthorized();

            var restaurantId = restaurantItem.Id;

            restaurantRepository.RemoveRestaurant(restaurantItem);
            restaurantRepository.SaveChanges();

            try
            {
                messageBusClient.DeleteRestaurant(new RestaurantDeletedDTO()
                {
                    Id = restaurantId
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return NoContent();
        }

        private bool isOwnerOfResource(Restaurant resource, ClaimsIdentity identity)
        {
            if (identity == null)
                return false;

            var userId = getIdentityId(identity);
            var userRole = getIdentityRole(identity);

            if (userId == 0 || userRole == "")
                return false;

            if (userRole == "ADMIN")
                return true;

            return resource.Owner.ExternalId == userId;
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
