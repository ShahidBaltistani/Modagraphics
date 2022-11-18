using System;

namespace Models
{
    public abstract class PaginationBaseModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    public class VehicleTypePaginationModel : PaginationBaseModel
    {
        public string VType { get; set; }
        public string BasePrice { get; set; }
        public bool IsActive { get; set; }
        public bool IsAll { get; set; }
    }

    public class UserPaginationModel : PaginationBaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class CorporateUserPaginationModel : PaginationBaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class SupervisorPaginationModel : PaginationBaseModel
    {
        public string SupervisorUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string LPN { get; set; }
        public string VIN { get; set; }
        public uint Unit { get; set; }
        public DateTime? AssignDate { get; set; }
    }

    public class InstallerPaginationModel : PaginationBaseModel
    {
        public string FleetOwnerName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public DateTime? AwardedDate { get; set; }
        public float Price { get; set; }
    }

    public class ProjectPaginationModel : PaginationBaseModel
    {
        public string FleetOwnerName { get; set; }
        public string ProjectName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ProjectContactName { get; set; }
        public string ProjectContactPhone { get; set; }
        public string ProjectAddress { get; set; }
    }

    public class FleetOwnerPaginationModel : PaginationBaseModel
    {
        public string CorporateUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string PhoneNo { get; set; }
    }

    public class CrewPaginationModel : PaginationBaseModel
    {
        public string CrewUserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? AssignedDate { get; set; }
    }

    public class JobPaginationModel : PaginationBaseModel
    {
        public uint SiteVehicleTypeId { get; set; }
        public string VIN { get; set; }
        public string LPN { get; set; }
        public uint UnitNo { get; set; }

        public string StatusString { get; set; }
        public int? JobStatus { get; set; }

        public JobStatus Status => StatusString == "" || StatusString == null ? default : (JobStatus)Enum.Parse(typeof(JobStatus), StatusString);
    }

    public class CrewJobPaginationModel : JobPaginationModel
    {
        public string VehicleTypeName { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string ProjectName { get; set; }
        public DateTime? StartedDate { get; set; }
    }

    public class CrewJobReviewPaginationModel : JobPaginationModel
    {
        public string ProjectName { get; set; }
        public string SiteName { get; set; }
        public string VehicleTypeName { get; set; }
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class CrewCompletedJobsPaginationModel : CrewJobReviewPaginationModel
    {
        
    }

    public class SitePaginationModel : PaginationBaseModel
    {
        public string ProjectName { get; set; }
        public string FleetOwnerName { get; set; }
        public string SiteName { get; set; }
        public string PO { get; set; }
        public DateTime? OpeningDate { get; set; }
        public int? SiteStatus { get; set; }
    }

    public class SiteBidPaginationModel : PaginationBaseModel
    {
        public int SiteId { get; set; }
        public string ProjectName { get; set; }
        public string SiteName { get; set; }
        public string Installer { get; set; }
        public DateTime? BidDate { get; set; }
        public float BidPrice { get; set; }
    }

    public class LoginPaginationModel : PaginationBaseModel
    {
        public string LoginName { get; set; }
        public string UserName { get; set; }
        public string RoleString { get; set; }
        public string Email { get; set; }
        public string IsActiveString { get; set; }
        public UserRoles Role => RoleString == "" || RoleString == null ? default : (UserRoles)Enum.Parse(typeof(UserRoles), RoleString);

        public bool IsActive => IsActiveString == "" || IsActiveString == null ? default : bool.Parse(IsActiveString);
    }

    public class VehicleTypeSiteAssociationPaginationModel : PaginationBaseModel
    {
        public uint SiteId { get; set; }
        public string VehicleType { get; set; }
        public string Specification { get; set; }
    }

    public class BidPaginationModel : PaginationBaseModel
    {

    }

    public class SiteStatusLogPaginationModel : PaginationBaseModel
    {
        public uint SiteId { get; set; }
        public string CreatedBy { get; set; }
        public SiteStatus SiteStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class JobStatusLogPaginationModel : PaginationBaseModel
    {
        public string Job { get; set; }
        public string CreatedBy { get; set; }
        public bool SiteStatus { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
