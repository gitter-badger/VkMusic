using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VkMusicSync;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for Authorize.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window
    {

        public string TokenID { get; private set; }

        public long UserID { get; private set; }

        public AuthorizeWindow(int appId)
        {
            InitializeComponent();


            webBrowser.WebSession = WebCore.CreateWebSession(AppPaths.WebSessionPath, new WebPreferences { AcceptLanguage = "ru-ru,ru" });

            webBrowser.Source = new Uri(CreateAuthorizeUrlFor(appId));

            webBrowser.AddressChanged += webBrowser_AddressChanged;

        }

        void webBrowser_AddressChanged(object sender, UrlEventArgs e)
        {
            if (e.OriginalString.StartsWith("https://oauth.vk.com/blank.html"))
            {
                var pairs = e.OriginalString.Split('#')[1].Split('&').Select(s => s.Split('=')).ToDictionary(s => s[0], s => s[1]);

                TokenID = pairs["access_token"];
                UserID = Convert.ToInt64(pairs["user_id"]);

                this.Close();
            }

        }

        private static string CreateAuthorizeUrlFor(int appId)
        {
            var builder = new StringBuilder("https://oauth.vk.com/authorize?");

            builder.AppendFormat("client_id={0}&", appId);
            builder.AppendFormat("scope={0}&", "audio");
            builder.Append("redirect_uri=https://oauth.vk.com/blank.html&");
            builder.AppendFormat("display={0}&", "page");
            builder.Append("response_type=token");

            return builder.ToString();
        }
    }
}
