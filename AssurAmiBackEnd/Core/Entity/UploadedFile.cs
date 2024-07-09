namespace AssurAmiBackEnd.Core.Entity
{
    public class UploadedFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public string UserId { get; set; }
        public bool IsSuccess { get; set; } // true for success, false for failure
    }

}
