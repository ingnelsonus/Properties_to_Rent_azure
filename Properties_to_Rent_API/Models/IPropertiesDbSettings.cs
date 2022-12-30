namespace Properties_to_Rent_API.Models
{
    public interface IPropertiesDbSettings
    {
        public string PropertyCollectionName { get; set; }

        public string AplicantCollectionName { get; set; }

        public string ApplicationCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DataBaseName { get; set; }
    }
}
