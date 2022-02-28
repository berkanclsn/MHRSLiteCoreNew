using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.Enums
{
    public class AllEnums
    {
    }

    public enum Genders : byte
    {
        Belirtilmemis,
        Bay,
        Bayan
    }
    public enum RoleNames:byte
    {
       Passive,
       Admin,
       Patient,
       PassiveDoctor,
       ActiveDoctor
    }

    public enum AppointmentStatus:byte
    {
        Past=0,
        Active=1,
        Cancelled=2
    }
}
