namespace TransferBatchProcessing
{
    public static class ProcessData
    {
        private const decimal Commission = 0.10m;

        public static void ReadFileAndCalculateCommissions(string[] args)
        {
            var error = Validations(args, out var filePath);

            if (string.IsNullOrEmpty(error))
                return;

            List<Transfer> transfers = [];

            try
            {
                ReadFileAndProcessData(filePath, transfers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading the file: {ex.Message}");
                return;
            }

            var accounts = transfers.GroupBy(t => t.AccountID);

            CalculateCommissions(accounts);
        }

        private static string? Validations(string[] args, out string filePath)
        {
            if (args.Length < 1)
            {
                filePath = string.Empty;
                Console.WriteLine("Please, give a file to process");
                return filePath;
            }

            try
            {
                filePath = Path.GetFullPath(args[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Args: {ex.Message}");
                throw;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist.");
                return null;
            }

            return filePath;
        }

        private static void ReadFileAndProcessData(string filePath, List<Transfer> transfers)
        {
            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var data = line.Split(',');

                if (data.Length == 3 &&
                    decimal.TryParse(data[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var amount))
                {
                    transfers.Add(new Transfer
                    {
                        AccountID = data[0],
                        TransferID = data[1],
                        TotalTransferAmount = amount
                    });
                }
            }
        }

        private static void CalculateCommissions(IEnumerable<IGrouping<string, Transfer>> accounts)
        {
            // Find the transfer with the maximum value
            var maxTransfer = accounts.SelectMany(group => group).Max(t => t.TotalTransferAmount);

            foreach (var accountGroup in accounts)
            {
                var accountID = accountGroup.Key;
                var accountTransfers = accountGroup.ToList();

                // Calculate the total commission (10% for all transfers except the highest one)
                var totalCommission = accountTransfers
                    .Where(t => t.TotalTransferAmount != maxTransfer)
                    .Sum(t => t.TotalTransferAmount * Commission);

                Console.WriteLine($"{accountID},{totalCommission:0.00}");
            }
        }
    }
}
