using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Web.Resource;
using Newtonsoft.Json;
using Properties_to_Rent_API.Models;
using Properties_to_Rent_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Properties_to_Rent_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PopertyOrgController : ControllerBase
    {
        //static readonly string[] scopeRequiredByApi = new string[] { "api://571efa1b-e8bb-4d89-a023-11c9143c6cf8/ReadOnly", "api://571efa1b-e8bb-4d89-a023-11c9143c6cf8/ReadWrite" };
        private readonly IPropertyServices _propertiesServices;
        private readonly IDistributedCache _cache;
        private readonly ILogger<PopertyOrgController> _logger;
        private readonly IQueueServices _queueServices;

        public PopertyOrgController(IPropertyServices propertiesServices,
                                    IDistributedCache cache,
                                    ILogger<PopertyOrgController> logger,
                                    IQueueServices queueServices)
        {
            _propertiesServices = propertiesServices;
            _cache = cache;
            _logger = logger;
            _queueServices = queueServices;
        }

        // GET: api/<PopertyOrgController>
        [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Property>> GetAll()
        {
            _logger.LogInformation("Property.GetAll");
            List<Property> lsProperties = new List<Property>();
            var cacheLsProperties = _cache.GetString("propertyList");

            try
            {                              
                if (!string.IsNullOrEmpty(cacheLsProperties))
                {
                    //cache..
                    lsProperties = JsonConvert.DeserializeObject<List<Property>>(cacheLsProperties);
                    _logger.LogInformation("Property.GetAll from cache");
                }
                else
                {
                    lsProperties = _propertiesServices.GetAll();
                    DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
                    cacheOptions.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                    _cache.SetString("propertyList", JsonConvert.SerializeObject(lsProperties), cacheOptions);
                    _logger.LogInformation("Property.GetAll from db");
                }

                _logger.LogInformation("Property.GetAll finish");

                _queueServices.SendMessageAsync(lsProperties, "add-property-data");
                return lsProperties;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return lsProperties;
            }
        }

        // GET api/<PopertyOrgController>/5
        [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet]
        [Route("GetById")]
        public ActionResult<Property> GetById(string id)
        {
            Property property = new Property();
            var cachePropertyById = _cache.GetString("propertyById"+id);

            if (!string.IsNullOrEmpty(cachePropertyById))
            {
                //cache..
                property = JsonConvert.DeserializeObject<Property>(cachePropertyById);
            }
            else
            {
                property = _propertiesServices.GetByID(id);
                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
                cacheOptions.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                _cache.SetString("propertyById"+id, JsonConvert.SerializeObject(property), cacheOptions);
            }

            return property;
        }

        // GET api/<PopertyOrgController>/5
        [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet]
        [Route("GetPropertiesByCity")]
        public ActionResult<List<Property>> GetPropertiesByCity(string city)
        {
            List<Property> lsPropertiesByCity = new List<Property>();
            var cacheLsPropertiesByCity = _cache.GetString("propertyListByCity"+city);

            if (!string.IsNullOrEmpty(cacheLsPropertiesByCity))
            {
                //cache..
                lsPropertiesByCity = JsonConvert.DeserializeObject<List<Property>>(cacheLsPropertiesByCity);
            }
            else
            {
                lsPropertiesByCity = _propertiesServices.GetPropertiesByCity(city);
                DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
                cacheOptions.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));
                _cache.SetString("propertyListByCity"+ city, JsonConvert.SerializeObject(lsPropertiesByCity), cacheOptions);
            }

            return lsPropertiesByCity;            
        }

        // POST api/<PopertyOrgController>
        [Authorize(Roles = "Api.ReadWrite")]
        [HttpPost]
        [Route("Create")]
        public ActionResult<Property> Create([FromBody] Property property)
        {
            _propertiesServices.Create(property);
            return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
        }

        // PUT api/<PopertyOrgController>/5
        [Authorize(Roles = "Api.ReadWrite")]
        [HttpPut]
        [Route("Update")]
        public ActionResult Update(string id, [FromBody] Property value)
        {
            var checkifExistingProperty = _propertiesServices.GetByID(id);

            if (checkifExistingProperty == null)
            {
                return NotFound($"Property with Id = {id} not foud");
            }

            _propertiesServices.Update(id, value);
            return NoContent();
        }

        // DELETE api/<PopertyOrgController>/5
        [Authorize(Roles = "Api.ReadWrite")]
        [HttpDelete]       
        [Route("Delete")]
        public ActionResult Delete(string id)
        {
            var checkifExistingProperty = _propertiesServices.GetByID(id);

            if (checkifExistingProperty == null)
            {
                return NotFound($"Property with Id = {id} not foud");
            }

            _propertiesServices.Remove(id);
            return Ok($"Student with Id= {id} deleted");

        }
    }
}
