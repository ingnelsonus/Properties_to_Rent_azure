using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using Properties_to_Rent_API.Models;
using Properties_to_Rent_API.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Properties_to_Rent_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PopertyOrgController : ControllerBase
    {
        static readonly string[] scopeRequiredByApi = new string[] { "api://571efa1b-e8bb-4d89-a023-11c9143c6cf8/ReadOnly", "api://571efa1b-e8bb-4d89-a023-11c9143c6cf8/ReadWrite" };
        private readonly IPropertyServices _propertiesServices;

        public PopertyOrgController(IPropertyServices propertiesServices)
        {
            _propertiesServices = propertiesServices;
        }


        // GET: api/<PopertyOrgController>
        [Authorize(Roles = "Api.ReadOnly,Api.ReadWrite")]
        [HttpGet]
        [Route("GetAll")]
        public ActionResult<List<Property>> GetAll()
        {
            //HttpContext.VerifyUserHasAnyAcceptedScope(scopeRequiredByApi);
            var result = _propertiesServices.GetAll();
            return result;
        }

        // GET api/<PopertyOrgController>/5
        [HttpGet]
        [Route("GetById")]
        public ActionResult<Property> GetById(string id)
        {
            return _propertiesServices.GetByID(id);
        }

        // GET api/<PopertyOrgController>/5
        [HttpGet]
        [Route("GetPropertiesByCity")]
        public ActionResult<List<Property>> GetPropertiesByCity(string city)
        {
            return _propertiesServices.GetPropertiesByCity(city);
        }

        // POST api/<PopertyOrgController>
        [HttpPost]
        public ActionResult<Property> Post([FromBody] Property property)
        {
            _propertiesServices.Create(property);
            return CreatedAtAction(nameof(GetById), new { id = property.Id }, property);
        }

        // PUT api/<PopertyOrgController>/5
        [HttpPut]
        public ActionResult Put(string id, [FromBody] Property value)
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
        [HttpDelete]
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
