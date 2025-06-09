using System;

public class BusinessPartnerAddress
{
    public Guid Id { get; set; }
    public Guid IdBusinnesPartner { get; set; }
    public string CEP { get; set; }
    public string District { get; set; }
    public string Complement { get; set; }
    public string City { get; set; }
    public string UF { get; set; }
}
