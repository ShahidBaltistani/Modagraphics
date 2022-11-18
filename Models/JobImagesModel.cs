namespace Models
{
    public class JobImagesModel
    {
        public uint Id { get; set; }
        public uint JobId { get; set; }
        public uint SiteId { get; set; }
        public uint CrewId { get; set; }
        public JobStatus Status { get; set; }
        public JobImagesStatus jobImagesStatus { get; set; }
        public JobImagesStatus ImagesDeleteStatus { get; set; }
        public string Picture { get; set; }
        public string Remarks { get; set; }
        
    }
}
