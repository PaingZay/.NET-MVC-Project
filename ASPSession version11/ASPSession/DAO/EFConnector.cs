using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ASPSession.Models;

namespace ASPSession.Connectors;

public class EFConnector : DbContext
{
    public EFConnector(DbContextOptions<EFConnector> options) : base(options)
    { }

    public DbSet<Product> Product { get; set; }
    public DbSet<Cart> Cart { get; set; }
    public DbSet<Order> Order { get; set; }
    public DbSet<Customer> Customer { get; set; }
}

