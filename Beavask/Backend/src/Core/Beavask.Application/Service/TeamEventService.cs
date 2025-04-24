using Beavask.Application.Common;
using Beavask.Application.DTOs.TeamEvent;
using Beavask.Application.Interface;
using Beavask.Application.Interface.Service;
using Beavask.Domain.Entities.Join;

namespace Beavask.Application.Service
{
    public class TeamEventService(IUnitOfWork unitOfWork) : ITeamEventService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Response<bool>> CreateAsync(TeamEventCreateDto dto)
        {
            try
            {
                var entity = new TeamEvent
                {
                    TeamId =  dto.TeamId,
                    EventId = dto.EventId,
                    CreatedAt = DateTime.UtcNow
                };

                await _unitOfWork.TeamEventRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message);
            }
        }

        public async Task<Response<bool>> DeleteAsync(TeamEventCreateDto dto)
        {
            try
            {
                var existing = await _unitOfWork.TeamEventRepository.GetAsync(query =>
                    query.Where(te => te.TeamId == dto.TeamId && te.EventId == dto.EventId));

                var entity = existing.FirstOrDefault();
                if (entity == null)
                    return Response<bool>.NotFound();

                await _unitOfWork.TeamEventRepository.DeleteAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                return Response<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.Fail(ex.Message);
            }
        }
    }
}
