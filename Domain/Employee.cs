﻿namespace Domain;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
    public int? CompanyId { get; set; }
    public Passport Passport { get; set; } = new Passport();
    public Department Department { get; set; } = new Department();
}