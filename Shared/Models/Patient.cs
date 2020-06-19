using System;
using System.Collections.Generic;
using System.Text;

namespace FIT3077.Shared.Models
{
    public class Patient
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMonitored { get; private set; } = false;

        public void ChangePatientMonitorState()
        {
            IsMonitored = !IsMonitored;
        }
    }
}
