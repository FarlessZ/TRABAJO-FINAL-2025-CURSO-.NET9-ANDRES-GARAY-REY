using System;

namespace PortalGalaxy.Common.Response;

public class BaseResponse
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}

public class BaseResponse<T> : BaseResponse
{
    public T? Data { get; set; }
}

public class PaginationResponse<T> : BaseResponse
{
    public ICollection<T>? Data { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}

public class BadRequestResponse
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Status { get; set; }
    public string TraceId { get; set; } = null!;
}