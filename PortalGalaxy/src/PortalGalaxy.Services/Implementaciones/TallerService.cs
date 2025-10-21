using AutoMapper;
using Microsoft.Extensions.Logging;
using PortalGalaxy.Common.Request;
using PortalGalaxy.Common.Response;
using PortalGalaxy.Entities;
using PortalGalaxy.Repositories.Interfaces;
using PortalGalaxy.Services.Interfaces;
using PortalGalaxy.Services.Utils;

namespace PortalGalaxy.Services.Implementaciones;

public class TallerService : ITallerService
{
    private readonly ITallerRepository _repository;
    private readonly ILogger<TallerService> _logger;
    private readonly IMapper _mapper;
    private readonly IFileUploader _fileUploader;

    public TallerService(ITallerRepository repository, ILogger<TallerService> logger, IMapper mapper, IFileUploader fileUploader)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
        _fileUploader = fileUploader;
    }

    public async Task<PaginationResponse<InscritosPorTallerDtoResponse>> ListAsync(BusquedaInscritosPorTallerRequest request)
    {
        var response = new PaginationResponse<InscritosPorTallerDtoResponse>();

        try
        {
            // Codigo
            var tupla = await _repository.ListAsync(request.InstructorId, request.Taller, request.Situacion,
                request.FechaInicio, request.FechaFin, request.PageNumber, request.PageSize);

            response.Data = _mapper.Map<ICollection<InscritosPorTallerDtoResponse>>(tupla.Colecction);
            response.TotalPages = Helper.GetTotalPages(tupla.Total, request.PageSize);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al listar los inscritos por taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse<ICollection<TallerSimpleDtoResponse>>> ListSimpleAsync()
    {
        var response = new BaseResponse<ICollection<TallerSimpleDtoResponse>>();

        try
        {
            response.Data = await _repository.ListAsync(
                predicate: x => x.Situacion == SituacionTaller.Aperturada || x.Situacion == SituacionTaller.Por_Aperturar,
                selector: x => new TallerSimpleDtoResponse
                {
                    Id = x.Id,
                    Nombre = x.Nombre
                });

            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al cargar los talleres";
            _logger.LogCritical(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<PaginationResponse<TallerHomeDtoResponse>> ListarTalleresHomeAsync(BusquedaTallerHomeRequest request)
    {
        var response = new PaginationResponse<TallerHomeDtoResponse>();

        try
        {
            var tupla = await _repository.ListarTalleresHomeAsync(request.Nombre, request.InstructorId,
                request.FechaInicio, request.FechaFin, request.PageNumber, request.PageSize);

            response.Data = _mapper.Map<ICollection<TallerHomeDtoResponse>>(tupla.Collection);
            response.TotalCount = tupla.Total;
            response.TotalPages = Helper.GetTotalPages(tupla.Total, request.PageSize);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al listar los talleres";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }

        return response;
    }

    public async Task<BaseResponse> AddAsync(TallerDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            var entity = _mapper.Map<Taller>(request);

            entity.PortadaUrl = await _fileUploader.UploadFileAsync(request.PortadaBase64, request.PortadaFileName);
            entity.TemarioUrl = await _fileUploader.UploadFileAsync(request.TemarioBase64, request.TemarioFileName);

            await _repository.AddAsync(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al agregar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse<TallerDtoRequest>> FindByIdAsync(int id)
    {
        var response = new BaseResponse<TallerDtoRequest>();
        try
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                response.ErrorMessage = "No se encontró el Taller";
                return response;
            }

            response.Data = _mapper.Map<TallerDtoRequest>(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al buscar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse<TallerHomeDtoResponse>> GetTallerHomeAsync(int id)
    {
        var response = new BaseResponse<TallerHomeDtoResponse>();
        try
        {
            var entity = await _repository.ObtenerTallerHomeAsync(id);
            if (entity == null)
            {
                response.ErrorMessage = "No se encontró el Taller";
                return response;
            }

            response.Data = _mapper.Map<TallerHomeDtoResponse>(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al buscar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse<ICollection<TalleresPorMesDto>>> ReporteTalleresPorMes(int anio)
    {
        var response = new BaseResponse<ICollection<TalleresPorMesDto>>();
        try
        {
            var entity = await _repository.ListarTalleresPorMesAsync(anio);

            response.Data = _mapper.Map<ICollection<TalleresPorMesDto>>(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al buscar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse<ICollection<TalleresPorInstructorDto>>> ReporteTalleresPorInstructor(int anio)
    {
        var response = new BaseResponse<ICollection<TalleresPorInstructorDto>>();
        try
        {
            var entity = await _repository.ListarTalleresPorInstructorAsync(anio);

            response.Data = _mapper.Map<ICollection<TalleresPorInstructorDto>>(entity);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al buscar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> UpdateAsync(int id, TallerDtoRequest request)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                response.ErrorMessage = "No se encontró el Taller";
                return response;
            }

            _mapper.Map(request, entity);
            
            if (request.PortadaBase64 != null)
            {
                entity.PortadaUrl = await _fileUploader.UploadFileAsync(request.PortadaBase64, request.PortadaFileName);
            }
            
            if (request.TemarioBase64 != null)
            {
                entity.TemarioUrl = await _fileUploader.UploadFileAsync(request.TemarioBase64, request.TemarioFileName);
            }
            
            await _repository.UpdateAsync();
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al actualizar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<BaseResponse> DeleteAsync(int id)
    {
        var response = new BaseResponse();
        try
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                response.ErrorMessage = "No se encontró el Taller";
                return response;
            }

            await _repository.DeleteAsync(id);
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al eliminar un Taller";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }

    public async Task<PaginationResponse<TallerDtoResponse>> ListAsync(BusquedaTallerRequest request)
    {
        var response = new PaginationResponse<TallerDtoResponse>();
        try
        {
            var tupla = await _repository.ListarTalleresAsync(request.Nombre, request.Categoria, request.Situacion, request.PageNumber, request.PageSize);

            response.Data = _mapper.Map<ICollection<TallerDtoResponse>>(tupla.Collection);
            response.TotalPages = Helper.GetTotalPages(tupla.Total, request.PageSize);
            response.TotalCount = tupla.Total;
            response.Success = true;
        }
        catch (Exception ex)
        {
            response.ErrorMessage = "Error al listar los Talleres";
            _logger.LogError(ex, "{ErrorMessage} {Message}", response.ErrorMessage, ex.Message);
        }
        return response;
    }
}