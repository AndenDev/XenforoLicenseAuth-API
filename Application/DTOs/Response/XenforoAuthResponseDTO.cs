namespace Application.DTOs.Response
{
    public class XenforoAuthResponseDTO
    {
        public bool Success { get; set; }
        public string SessionId { get; set; }
        public XenforoUserDTO User { get; set; }
        public class XenforoUserDTO
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public int UserGroupId { get; set; }
            public string UserGroupName { get; set; }
        }
    }
}
