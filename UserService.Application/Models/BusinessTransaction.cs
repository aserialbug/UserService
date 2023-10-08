namespace UserService.Application.Models;

public class BusinessTransaction
{
    public BusinessTransactionId Id { get; }
    public bool IsActive { get; private set; }

    
    public event EventHandler<BusinessTransactionClosedEventArgs>? Closed;

    public BusinessTransaction(BusinessTransactionId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
        IsActive = true;
    }

    public async Task Commit()
    {
        if (!IsActive)
            return;
        
        await Task.CompletedTask;
        IsActive = false;
        InvokeClosed();
    }

    public async Task Rollback()
    {
        if (!IsActive)
            return;
        
        await Task.CompletedTask;
        IsActive = false;
        InvokeClosed();
    }

    private void InvokeClosed()
    {
        Closed?.Invoke(this, new BusinessTransactionClosedEventArgs(Id));
    }

    public async Task Cancel()
    {
        if (!IsActive)
            return;
        
        await Task.CompletedTask;
        IsActive = false;
        InvokeClosed();
    }
}