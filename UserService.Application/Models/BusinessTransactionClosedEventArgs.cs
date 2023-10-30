namespace UserService.Application.Models;

public class BusinessTransactionClosedEventArgs : EventArgs
{
    public BusinessTransactionId TransactionId { get; }
    
    public BusinessTransactionClosedEventArgs(BusinessTransactionId transactionId)
    {
        TransactionId = transactionId;
    }
}