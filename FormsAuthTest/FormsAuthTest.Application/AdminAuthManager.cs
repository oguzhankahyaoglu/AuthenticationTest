namespace FormsAuthTest.Application
{
    public class AdminAuthManager
    {
        public enum LoginStatuses { IP_ALREADY_BANNED, SUCCESS, FAIL, FAILBANNED };

        public static AdminMembershipManager Manager = new AdminMembershipManager();
    }
}
