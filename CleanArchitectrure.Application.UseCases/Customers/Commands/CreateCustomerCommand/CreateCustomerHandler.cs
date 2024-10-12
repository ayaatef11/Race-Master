using AutoMapper;
using CleanArchitectrure.Application.UseCases.Commons.Bases;
using MediatR;
using RunGroop.Repository.Interfaces;

namespace CleanArchitectrure.Application.UseCases.Customers.Commands.CreateCustomerCommand
{
    public class CreateCustomerHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCustomerCommand, BaseResponse<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

		public async Task<BaseResponse<bool>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var customer = _mapper.Map<Customer>(command);
                response.Data = await _unitOfWork.Customers.InsertAsync(customer);
                if (response.Data) 
                {
                    response.succcess = true;
                    response.Message  = "Create succeed!";
                }
            }
            catch (Exception ex)
            {
                response.Message  = ex.Message;
            }
            return response;
        }
    }
}
