using CRUDCxC.Entities;

namespace CRUDCxC.Data;

public static class DbSeeder
{
    public static void SeedClients(CxCDbContext context)
    {
        if (!context.Clients.Any())
        {
            var clients = new List<Client>
            {
                new Client
                {
                    Name = "Juan Pérez",
                    CreditLimit = 50000,
                    Status = Status.Active,
                    IdentificationNumber = "00112345673"
                },
                new Client
                {
                    Name = "Ana Gómez",
                    CreditLimit = 30000,
                    Status = Status.Active,
                    IdentificationNumber = "00298765439"
                },
                new Client
                {
                    CreditLimit = 15000,
                    Name = "Pedro Ramírez",
                    Status = Status.Active,
                    IdentificationNumber = "00345678908",
                }
            };

            context.Clients.AddRange(clients);
            context.SaveChanges();
        }
    }

    public static void SeedDocumentTypes(CxCDbContext context)
    {

        if (!context.DocumentTypes.Any(dt => dt.Description == "Ingresos x Ventas"))
        {
            context.DocumentTypes.Add(new DocumentType
            {
                Status = Status.Active,
                AccountingAccount = "4-01",
                Description = "Ingresos x Ventas"
            });
        }

        var clients = context.Clients.ToList();

        foreach (var client in clients)
        {
            string descripcion = $"Cuentas x Cobrar Cliente '{client.Name}'";
            if (!context.DocumentTypes.Any(dt => dt.Description == descripcion))
            {
                context.DocumentTypes.Add(new DocumentType
                {
                    Status = Status.Active,
                    Description = descripcion,
                    AccountingAccount = $"1-01-{client.Id}",
                });
            }
        }

        context.SaveChanges();
    }

    public static void SeedTransactions(CxCDbContext context)
    {
        var clients = context.Clients.ToList();

        var docTypes = context.DocumentTypes
            .Where(dt => dt.Description.StartsWith("Cuentas x Cobrar Cliente"))
            .ToList();

        if (!clients.Any() || !docTypes.Any()) return;

        var random = new Random();
        int globalDocCounter = 1;

        foreach (var client in clients)
        {
            var documentType = docTypes.FirstOrDefault(dt => dt.Description.Contains(client.Name));
            if (documentType == null) continue;

            for (int day = 1; day <= 28; day++)
            {
                var amount = Math.Min(random.Next(1000, 10000), (int)client.CreditLimit);

                var transaction = new Transaction
                {
                    Amount = amount,
                    ClientId = client.Id,
                    DocumentTypeId = documentType.Id,
                    MovementType = MovementType.Debit,
                    DocumentNumber = $"DOC-{globalDocCounter:D5}",
                    Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day),
                };

                context.Transactions.Add(transaction);
                globalDocCounter++;
            }
        }

        context.SaveChanges();
    }

}
