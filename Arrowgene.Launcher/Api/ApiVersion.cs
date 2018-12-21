using System.Runtime.Serialization;

namespace Arrowgene.Launcher.Api
{
    [DataContract]
    public class ApiVersion
    {
        [DataMember(Name = "launcherVersion")]
        public int LauncherVersion { get; set; }

        [DataMember(Name = "launcherUrl")]
        public string LauncherUrl { get; set; }

        [DataMember(Name = "danceVersion")]
        public int DanceVersion { get; set; }

        [DataMember(Name = "danceUrl")]
        public string DanceUrl { get; set; }

        [DataMember(Name = "danceUrlMirror")]
        public string DanceUrlMirror { get; set; }

        [DataMember(Name = "ez2OnVersion")]
        public int Ez2OnVersion { get; set; }

        [DataMember(Name = "ez2OnUrl")]
        public string Ez2OnUrl { get; set; }

        [DataMember(Name = "ez2OnUrlMirror")]
        public string Ez2OnUrlMirror { get; set; }
    }
}
