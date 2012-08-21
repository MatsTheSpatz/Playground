using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.Xml;

namespace RecipeOcr
{
    public class OcrConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("serverUrl", DefaultValue = "http://cloud.ocrsdk.com/", IsRequired = true)]
        public string ServerUrl
        {
            get
            {
                return (string)this["serverUrl"]; 
            }
            set
            {
                this["serverUrl"] = value; 
            }
        }

        [ConfigurationProperty("applicationId", IsRequired = true)]
        public string ApplicationId
        {
            get
            {
                return (string)this["applicationId"];
            }
            set
            {
                this["applicationId"] = value;
            }
        }

        [ConfigurationProperty("password", IsRequired = false)]
        public string Password
        {
            get
            {
                return (string)this["password"];
            }
            set
            {
                this["password"] = value;
            }
        }

        [ConfigurationProperty("proxy", IsRequired = false)]
        public string Proxy
        {
            get
            {
                return (string)this["proxy"];
            }
            set
            {
                this["proxy"] = value;
            }
        }
    }

}
