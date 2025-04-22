using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.LogDtos;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class LogService(IUnitOfWork unitOfWork, IMapper mapper) : ILogService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<LogDto>> CreateAsync(LogCreateDto logCreateDto)
    {
        try
        {
            var entity = _mapper.Map<Log>(logCreateDto);
            await _unitOfWork.LogRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<LogDto>(entity);
            return Response<LogDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<LogDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<LogDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.LogRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<LogDto>>(entities);
            return Response<IEnumerable<LogDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<LogDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<LogDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.LogRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<LogDto>.NotFound();

            var dto = _mapper.Map<LogDto>(entity);
            return Response<LogDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<LogDto>.Fail(ex.Message);
        }
    }
}
