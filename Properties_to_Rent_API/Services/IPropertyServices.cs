using Properties_to_Rent_API.Models;

namespace Properties_to_Rent_API.Services
{
    public interface IPropertyServices
    {
        List<Property> GetAll();
        List<Property> GetPropertiesByCity(string city);
        Property GetByID(string id);
        Property Create(Property property);
        void Update(string id, Property property);
        void Remove(string id);
    }
}
