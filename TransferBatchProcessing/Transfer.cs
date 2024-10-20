namespace TransferBatchProcessing
{
    public class Transfer
    {
        public required string AccountID { get; set; }
        public required string TransferID { get; set; }
        public decimal TotalTransferAmount { get; set; }
    }
}
