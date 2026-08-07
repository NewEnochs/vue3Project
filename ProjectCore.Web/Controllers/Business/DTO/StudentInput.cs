using ProjectCore.DAL;
using ProjectCore.Util;

namespace ProjectCore.Web.Controllers.Business
{
    public class StudentInput : PageInputBase
    {
    }

    public class EditStudnetInput : Student
    {
    }

    public class StudentIdInput
    {
        public string GUID { get; set; } = null!;
    }

    public class GradeInput : PageInputBase
    {
    }

    public class EditGradeInput : Grade
    {
    }

    public class GradeIdInput
    {
        public string GradeGUID { get; set; } = null!;
    }

    public class AddressInput : PageInputBase
    {
    }

    public class EditAddressInput : Address
    {
    }

    public class AddressIdInput
    {
        public int AddressID { get; set; }
    }

    public class CompanyInput : PageInputBase
    {
    }

    public class EditCompanyInput : Company
    {
    }

    public class CompanyIdInput
    {
        public string CompanyGUID { get; set; } = null!;
    }

    public class MenuInput : PageInputBase
    {
    }

    public class EditMenuInput : Menu
    {
    }

    public class MenuIdInput
    {
        public string MenuGUID { get; set; } = null!;
    }

    public class ConstellationInput : PageInputBase
    {
    }

    public class EditConstellationInput : Constellation
    {
    }

    public class ConstellationIdInput
    {
        public int ConstellationID { get; set; }
    }
}
