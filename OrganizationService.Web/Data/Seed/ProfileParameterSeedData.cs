using System.Collections.Generic;
using Kebormed.Core.OrganizationService.Web.Data.Entities;

namespace Kebormed.Core.OrganizationService.Web.Data.Seed
{
    public static class ProfileParameterSeedData
    {
        //TODO: _Potentially_ Move this list higher up so it can be shared with non core services
        public static List<ProfileParameterEntity> Values = new List<ProfileParameterEntity>
        {
            new ProfileParameterEntity {ProfileParameterEntityId = 1, Name = "First Name"},
            new ProfileParameterEntity {ProfileParameterEntityId = 2, Name = "Last Name"},
            new ProfileParameterEntity {ProfileParameterEntityId = 3, Name = "Birthdate"},
            new ProfileParameterEntity {ProfileParameterEntityId = 4, Name = "Gender"},
            new ProfileParameterEntity {ProfileParameterEntityId = 5, Name = "Height"},
            new ProfileParameterEntity {ProfileParameterEntityId = 6, Name = "Weight"},
            new ProfileParameterEntity {ProfileParameterEntityId = 7, Name = "BMI"},
            new ProfileParameterEntity {ProfileParameterEntityId = 8, Name = "Therapy Start Date"},
            new ProfileParameterEntity {ProfileParameterEntityId = 9, Name = "Date Collected"},
            new ProfileParameterEntity {ProfileParameterEntityId = 10, Name = "Address 1"},
            new ProfileParameterEntity {ProfileParameterEntityId = 11, Name = "Address 2"},
            new ProfileParameterEntity {ProfileParameterEntityId = 12, Name = "City"},
            new ProfileParameterEntity {ProfileParameterEntityId = 13, Name = "State"},
            new ProfileParameterEntity {ProfileParameterEntityId = 14, Name = "Country"},
            new ProfileParameterEntity {ProfileParameterEntityId = 15, Name = "Postal Code"},
            new ProfileParameterEntity {ProfileParameterEntityId = 16, Name = "Email"},
            new ProfileParameterEntity {ProfileParameterEntityId = 17, Name = "Phone 1"},
            new ProfileParameterEntity {ProfileParameterEntityId = 18, Name = "Phone 2"},
            new ProfileParameterEntity {ProfileParameterEntityId = 19, Name = "Phone 3"},
            new ProfileParameterEntity {ProfileParameterEntityId = 20, Name = "Phone 4"},
            new ProfileParameterEntity {ProfileParameterEntityId = 21, Name = "Phone 4"},
            new ProfileParameterEntity {ProfileParameterEntityId = 22, Name = "Generic 1"},
            new ProfileParameterEntity {ProfileParameterEntityId = 23, Name = "Generic 2"},
            new ProfileParameterEntity {ProfileParameterEntityId = 24, Name = "Generic 3"},
            new ProfileParameterEntity {ProfileParameterEntityId = 25, Name = "Generic 4"},
            new ProfileParameterEntity {ProfileParameterEntityId = 26, Name = "Generic 5"},
            new ProfileParameterEntity {ProfileParameterEntityId = 27, Name = "Physician"},
            new ProfileParameterEntity {ProfileParameterEntityId = 28, Name = "Primary Care Physician"},
            new ProfileParameterEntity {ProfileParameterEntityId = 29, Name = "Group"},
            new ProfileParameterEntity {ProfileParameterEntityId = 30, Name = "Sleep Doctor"},
            new ProfileParameterEntity {ProfileParameterEntityId = 31, Name = "Practice"},
            new ProfileParameterEntity {ProfileParameterEntityId = 32, Name = "PatientId"},
            new ProfileParameterEntity {ProfileParameterEntityId = 33, Name = "PatientId2"},
            new ProfileParameterEntity {ProfileParameterEntityId = 34, Name = "DeviceEditPermission"},
            new ProfileParameterEntity {ProfileParameterEntityId = 35, Name = "AllowPhysiciansEditDeviceSettings"},
            new ProfileParameterEntity {ProfileParameterEntityId = 36, Name = "ContactName"},
            new ProfileParameterEntity {ProfileParameterEntityId = 37, Name = "PatientStatus"}
        };
    }
}
