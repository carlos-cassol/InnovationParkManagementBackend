namespace InnovationParkManagementBackend.Application.DTO
{
    public class AddressDTO
    {
        public string? Cep { get; set; }
        public string? Street { get; set; }
        public int Number { get; set; }
        public string? Neighborhood { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Uf {  get; set; }
    }
}
