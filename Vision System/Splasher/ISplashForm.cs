using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vision_System
{
    interface ISplashForm
    {
        void SetStatusInfo(string NewStatusInfo);
        void SetProgressInfo(int NewProgressInfo);
    }
}
