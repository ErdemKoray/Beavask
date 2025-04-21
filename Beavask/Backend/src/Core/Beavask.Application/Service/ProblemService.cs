using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Problem;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interfaces;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class ProblemService(IUnitOfWork unitOfWork, IMapper mapper) : IProblemService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<ProblemDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.ProblemRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<ProblemDto>.NotFound();

            var dto = _mapper.Map<ProblemDto>(entity);
            return Response<ProblemDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProblemDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<ProblemDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.ProblemRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<ProblemDto>>(entities);
            return Response<IEnumerable<ProblemDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<ProblemDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<ProblemDto>> CreateAsync(ProblemCreateDto problemCreateDto)
    {
        try
        {
            var entity = _mapper.Map<Problem>(problemCreateDto);
            await _unitOfWork.ProblemRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<ProblemDto>(entity);
            return Response<ProblemDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProblemDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<ProblemDto>> UpdateAsync(int id, ProblemUpdateDto problemUpdateDto)
    {
        try
        {
            var entity = await _unitOfWork.ProblemRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<ProblemDto>.NotFound();

            _mapper.Map(problemUpdateDto, entity);
            await _unitOfWork.ProblemRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<ProblemDto>(entity);
            return Response<ProblemDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<ProblemDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.ProblemRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.ProblemRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}
