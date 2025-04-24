using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Team;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.DTOs.User;
using Beavask.Application.DTOs.Event;

namespace Beavask.Application.Service;

public class TeamService(IUnitOfWork unitOfWork, IMapper mapper) : ITeamService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<TeamDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<TeamDto>.NotFound();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<TeamDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.TeamRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<TeamDto>>(entities);
            return Response<IEnumerable<TeamDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<TeamDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> CreateAsync(TeamCreateDto teamCreate)
    {
        try
        {
            var entity = _mapper.Map<Team>(teamCreate);
            await _unitOfWork.TeamRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> UpdateAsync(int id, TeamUpdateDto teamUpdate)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<TeamDto>.NotFound();

            _mapper.Map(teamUpdate, entity);
            await _unitOfWork.TeamRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.TeamRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamWithMembersDto>> GetTeamWithMembersAsync(int teamId)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetTeamWithMembersAsync(teamId);
            if (entity == null)
                return Response<TeamWithMembersDto>.NotFound();

            var dto = _mapper.Map<TeamWithMembersDto>(entity);
            return Response<TeamWithMembersDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamWithMembersDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserDto>>> GetMembersByTeamIdAsync(int teamId)
    {
        var members = await _unitOfWork.TeamRepository.GetMembersByTeamId(teamId);
        if (members == null || !members.Any())
            return Response<IEnumerable<UserDto>>.NotFound();
        
        var dtos = _mapper.Map<IEnumerable<UserDto>>(members);
        return Response<IEnumerable<UserDto>>.Success(dtos);
    
    }

    public async Task<Response<IEnumerable<EventDto>>> GetEventsByTeamIdAsync(int teamId)
    {
        var events = await _unitOfWork.TeamRepository.GetEventsByTeamId(teamId);
        if (events == null || !events.Any())
            return Response<IEnumerable<EventDto>>.NotFound();
        
        var dtos = _mapper.Map<IEnumerable<EventDto>>(events);
        return Response<IEnumerable<EventDto>>.Success(dtos);
    }
}
