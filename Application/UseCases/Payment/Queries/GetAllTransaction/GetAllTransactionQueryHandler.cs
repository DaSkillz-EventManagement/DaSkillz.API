﻿using Application.Abstractions.Caching;
using Application.ResponseMessage;
using AutoMapper;
using Domain.DTOs.Payment.Response;
using Domain.Models.Response;
using Domain.Repositories;
using MediatR;
using System.Net;

namespace Application.UseCases.Payment.Queries.GetAllTransaction
{
    public class GetAllTransactionQueryHandler : IRequestHandler<GetAllTransactionQuery, APIResponse>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IRedisCaching _caching;

        public GetAllTransactionQueryHandler(ITransactionRepository transactionRepository, IMapper mapper, IRedisCaching caching)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _caching = caching;
        }

        public async Task<APIResponse> Handle(GetAllTransactionQuery request, CancellationToken cancellationToken)
        {
            var raw = await _transactionRepository.GetAll();
            var result = _mapper.Map<IEnumerable<TransactionResponseDto>>(raw);
            return new APIResponse
            {
                StatusResponse = HttpStatusCode.OK,
                Message = MessageCommon.GetSuccesfully,
                Data = result,
            };
        }
    }
}
