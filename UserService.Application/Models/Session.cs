using System.Collections.Concurrent;

namespace UserService.Application.Models;

public class Session
{
    private bool _free;
    private readonly ConcurrentDictionary<BusinessTransactionId, BusinessTransaction> _transactions = new();
    public SessionId Id { get; }
    public bool IsActive { get; private set; }

    public event EventHandler<SessionCompletedEventArgs>? Completed;
    
    public Session(SessionId sessionId)
    {
        ArgumentNullException.ThrowIfNull(nameof(sessionId));
        Id = sessionId;
        IsActive = true;
    }

    public Task<BusinessTransaction> StartTransaction()
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot start transaction on inactive session {Id}");
        
        var id = BusinessTransactionId.New();
        var transaction = _transactions.GetOrAdd(id,
            transactionId => new BusinessTransaction(transactionId));
        
        transaction.Closed += TransactionOnClosed;
        return Task.FromResult(transaction);
    }

    private void TransactionOnClosed(object? sender, BusinessTransactionClosedEventArgs e)
    {
        TryFinishSession();
    }

    private void TryFinishSession()
    {
        if (!_transactions.Any(pair => pair.Value.IsActive) && _free)
        {
            IsActive = false;
            InvokeCompleted();   
        }
    }

    private void InvokeCompleted()
    {
        Completed?.Invoke(this, new SessionCompletedEventArgs(Id));
    }

    public Task Free()
    {
        _free = true;
        TryFinishSession();
        return  Task.CompletedTask;
    }
}