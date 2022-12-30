namespace Properties_to_Rent_API.Models
{
    public class PropertiesDbSettings : IPropertiesDbSettings
    {
        public string PropertyCollectionName { get ; set ; }=string.Empty;
        public string AplicantCollectionName { get; set; } = string.Empty;
        public string ApplicationCollectionName { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string DataBaseName { get; set; } = string.Empty;
    }
}
