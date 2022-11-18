namespace Models
{
    public enum UserRoles
    {
        User,
        ApprovalManager,
        FinanceManager,
        CorporateUser,
        Installer,
        FleetOwner,
        Crew,
        Supervisor
    }

    public enum UserStatus
    {
        WaitingForApproval,
        Approved,
        Rejected
    }

    public enum CorporateUserStatus
    {
        WaitingForApproval,
        Approved,
        Rejected
    }

    public enum SupervisorStatus
    {
        WaitingForApproval,
        Approved,
        Rejected
    }

   
    public enum UserType
    {
        DateEntryOperator,
        ApprovalManager,
        FinanceManager
    }

    public enum BidStatus
    {
        Pending,
        Awarded,
        Assigned,
        Rejected
    }

    public enum SiteStatus
    {
        Pendning,
        OpenForBid,
        Awarded,
        Assigned,
        InProcess,
        ReadyForInvoice,
        Invoiced,
        ClosedForBid
    }

    public enum TakedownStatus
    {
        Site,
        VehicleType,
        None
    }

    public enum FleetOwnerStatus
    {
        WaitingForApproval,
        Approved,
        Rejected
    }

    public enum InstallerStatus
    {
        WaitingForApproval,
        Approved,
        Rejected
    }
    //Child table Requied for JobStaus
    public enum JobStatus
    {
        Pending,
        InQueue,
        InProcess,
        UnderReviewSupervisor,
        UnderReviewApprovalManager,
        IncidentReported,
        SentForRepair,
        Completed,
        Rejected
    }

    public enum JobImagesStatus
    {
        IsStart,
        IsEnd,
        IsDeleted,
        IncidentReported
    }
    public enum ProjectStatus
    {
        Active,
        InActive,
        Pending
    }

    public enum StatusByFleetOwner
    {

    }
}
