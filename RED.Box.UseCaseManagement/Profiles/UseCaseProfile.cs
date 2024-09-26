using AutoMapper;
using UseCaseManagement.Domain.Entities;
using UseCaseManagement.Service.Contract;

namespace UseCaseManagement.Service.Profiles;

public class UseCaseProfile :  Profile
{
    public UseCaseProfile()
    {
        /***************Vendor***************/
        CreateMap<Vendor, VendorResponse>();
        CreateMap<Vendor, VendorResponseWithDetails>();
        
        /***************UseCase***************/
        CreateMap<UseCase, UseCaseResponse>();
        CreateMap<UseCase, UseCaseResponseWithDetails>();

        /***************LogSource***************/
        CreateMap<LogSource, LogSourceResponse>();
        CreateMap<LogSource, LogSourceResponseWithDetails>();
        CreateMap<LogSource, UpdateLogSourceResponse>();
        
        /***************LogSourceFile***************/
        CreateMap<LogSourceFile, LogSourceFileResponse>();
        CreateMap<LogSourceFile, LogSourceFileResponseToDetails>();

        /***************UseCaseFile***************/
        CreateMap<UseCaseFile, UseCaseFileResponse>();
        CreateMap<UseCaseFile, UseCaseFileResponseToDetails>();
    }
}
