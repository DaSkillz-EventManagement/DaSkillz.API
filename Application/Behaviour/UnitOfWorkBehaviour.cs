using Domain.Repository.UnitOfWork;
using MediatR;
using System.Transactions;
using System.Windows.Input;

namespace Application.Behaviour
{
    public sealed class UnitOfWorkBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, ICommand
    {
        private readonly IUnitOfWork _dataSource;

        public UnitOfWorkBehaviour(IUnitOfWork dataSource)
        {
            _dataSource = dataSource;
        }

        //handle transactions in middleware 
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var response = await next();

                await _dataSource.SaveChangesAsync(cancellationToken);

                transaction.Complete();

                return response;
            }
        }
    }
}
