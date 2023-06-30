using CLI_TImer.MVVM.Model;
using System.Collections.Generic;

namespace CLI_TImer.Utils
{
    //ToDo
    //Save after changes
    //Default Profile
    public static class NewProfileManager
    {
        public static List<Profile>? ProfileList { get; set; }
        public static Profile DefaultProfile { get; set; } = new Profile();

        private static string AddProfile(Profile profile)
        {
            foreach(Profile p in ProfileList)
            {
                if (p.Name == profile.Name)
                {
                    return "A Profile with this name already exists";
                }
            }

            ProfileList.Add(profile);

            //Save
            return "Profile successfully added";
        }

        private static string RemoveProfile(Profile profile) 
        {
            if (!ProfileList.Contains(profile)) return "Profile does not exist";

            ProfileList.Remove(profile);
            return "Profile Successfully deleted";

            //Save
        }

        public static Profile? GetProfile(string name) 
        {
            foreach(Profile p in ProfileList)
            {
                if(p.Name == name) return p;
            }

            return new();
        }
    }
}
