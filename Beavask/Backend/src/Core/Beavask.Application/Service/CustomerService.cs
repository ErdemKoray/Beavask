using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Customer;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;
public class CustomerService(IUnitOfWork unitOfWork, IMapper mapper) : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<CustomerDto>> CreateAsync(CustomerCreateDto customerCreateDto)
    {
        try
        {
            var customer = _mapper.Map<Customer>(customerCreateDto);
            await _unitOfWork.CustomerRepository.AddAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Response<CustomerDto>.Success(customerDto);
        }
        catch (Exception ex)
        {
            return Response<CustomerDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<CustomerDto>> GetByIdAsync(int id)
    {
        try
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return Response<CustomerDto>.NotFound();
            }

            var customerDto = _mapper.Map<CustomerDto>(customer);
            return Response<CustomerDto>.Success(customerDto);
        }
        catch (Exception ex)
        {
            return Response<CustomerDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<CustomerDto>>> GetAllAsync()
    {
        try
        {
            var customers = await _unitOfWork.CustomerRepository.GetAsync();
            var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);
            return Response<IEnumerable<CustomerDto>>.Success(customerDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<CustomerDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<CustomerDto>> UpdateAsync(int id, CustomerUpdateDto customerUpdateDto)
    {
        try
        {
            var existingCustomer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                return Response<CustomerDto>.NotFound();
            }

            _mapper.Map(customerUpdateDto, existingCustomer);
            await _unitOfWork.CustomerRepository.UpdateAsync(existingCustomer);
            await _unitOfWork.SaveChangesAsync();
            var customerDto = _mapper.Map<CustomerDto>(existingCustomer);
            return Response<CustomerDto>.Success(customerDto);
        }
        catch (Exception ex)
        {
            return Response<CustomerDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
            if (customer == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.CustomerRepository.DeleteAsync(customer);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}
