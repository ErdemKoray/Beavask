using Beavask.Application.Common;
using Beavask.Application.DTOs.UserContact;
using Beavask.Application.Interfaces;
using AutoMapper;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class UserContactService(IUnitOfWork unitOfWork, IMapper mapper) : IUserContactService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<UserContactDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.UserContactRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<UserContactDto>.NotFound();
            }
            
            var dto = _mapper.Map<UserContactDto>(entity);
            return Response<UserContactDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<UserContactDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserContactDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.UserContactRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<UserContactDto>>(entities);
            return Response<IEnumerable<UserContactDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<UserContactDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserContactDto>>> GetByUserIdAsync(int userId)
    {
        try
        {
            var entities = await _unitOfWork.UserContactRepository.GetAsync(query => 
                query.Where(uc => uc.UserId == userId));
            var dtos = _mapper.Map<IEnumerable<UserContactDto>>(entities);
            return Response<IEnumerable<UserContactDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<UserContactDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserContactDto>> CreateAsync(UserContactCreateDto userContactCreate)
    {
        try
        {
            var entity = _mapper.Map<UserContact>(userContactCreate);
            await _unitOfWork.UserContactRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = _mapper.Map<UserContactDto>(entity);
            return Response<UserContactDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<UserContactDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserContactDto>> UpdateAsync(int id, UserContactCreateDto userContactCreate)
    {
        try
        {
            var existingEntity = await _unitOfWork.UserContactRepository.GetByIdAsync(id);
            if (existingEntity == null)
            {
                return Response<UserContactDto>.NotFound();
            }

            _mapper.Map(userContactCreate, existingEntity);
            await _unitOfWork.UserContactRepository.UpdateAsync(existingEntity);
            await _unitOfWork.SaveChangesAsync();
            
            var dto = _mapper.Map<UserContactDto>(existingEntity);
            return Response<UserContactDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<UserContactDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.UserContactRepository.GetByIdAsync(id);
            if (entity == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.UserContactRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}
