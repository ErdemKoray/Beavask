using Beavask.Application.DTOs.Milestone;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.Interface.Service;

namespace Beavask.Application.Service;

public class MilestoneService : IMilestoneService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MilestoneService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

public async Task<Response<MilestoneDto>> CreateAsync(MilestoneCreateDto milestoneCreateDto)
{
    try
    {
        var project = await _unitOfWork.ProjectRepository.GetByIdAsync(milestoneCreateDto.ProjectId);
        var milestone = _mapper.Map<Milestone>(milestoneCreateDto);
        milestone.Project = project; 

        await _unitOfWork.MilestoneRepository.AddAsync(milestone);
        await _unitOfWork.SaveChangesAsync();

        var milestoneDto = _mapper.Map<MilestoneDto>(milestone);
        return Response<MilestoneDto>.Success(milestoneDto);
    }
    catch (Exception ex)
    {
        return Response<MilestoneDto>.Fail(ex.Message);
    }
}


    public async Task<Response<MilestoneDto>> GetByIdAsync(int id)
    {
        try
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdAsync(id);
            if (milestone == null)
            {
                return Response<MilestoneDto>.NotFound();
            }

            var milestoneDto = _mapper.Map<MilestoneDto>(milestone);
            return Response<MilestoneDto>.Success(milestoneDto);
        }
        catch (Exception ex)
        {
            return Response<MilestoneDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<MilestoneDto>>> GetAllAsync()
    {
        try
        {
            var milestones = await _unitOfWork.MilestoneRepository.GetAsync();
            var milestoneDtos = _mapper.Map<IEnumerable<MilestoneDto>>(milestones);
            return Response<IEnumerable<MilestoneDto>>.Success(milestoneDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<MilestoneDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<MilestoneDto>> UpdateAsync(int id, MilestoneUpdateDto milestoneUpdateDto)
    {
        try
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdAsync(id);
            if (milestone == null)
            {
                return Response<MilestoneDto>.NotFound();
            }

            _mapper.Map(milestoneUpdateDto, milestone);
            await _unitOfWork.MilestoneRepository.UpdateAsync(milestone);
            await _unitOfWork.SaveChangesAsync();

            var milestoneDto = _mapper.Map<MilestoneDto>(milestone);
            return Response<MilestoneDto>.Success(milestoneDto);
        }
        catch (Exception ex)
        {
            return Response<MilestoneDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var milestone = await _unitOfWork.MilestoneRepository.GetByIdAsync(id);
            if (milestone == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.MilestoneRepository.DeleteAsync(milestone);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}

