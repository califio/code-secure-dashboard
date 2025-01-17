using CodeSecure.Api.Project.Model;
using CodeSecure.Database.Entity;

namespace CodeSecure.Api.Project;

public class ProjectAutoMapper : AutoMapper.Profile
{
    public ProjectAutoMapper()
    {
        CreateMap<Projects, ProjectInfo>()
            .ForMember(dest => dest.SourceType,
                opt => opt.MapFrom(src => src.SourceControl!.Type));
        
        CreateMap<ProjectCommits, ProjectCommitSummary>()
            .ForMember(dest => dest.CommitId,
            opt => opt.MapFrom(src => src.Id));
        
        CreateMap<Findings, ProjectFinding>()
            .ForMember(dest => dest.Scanner,
                opt => opt.MapFrom(src => src.Scanner!.Name))
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => src.Scanner!.Type));
        
        CreateMap<Users, ProjectUser>()
            .ForMember(dest => dest.UserId,
            opt => opt.MapFrom(src => src.Id));
    }
}