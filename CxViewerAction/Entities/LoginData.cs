using System;
using System.Collections.Generic;
using CxViewerAction.Entities.Enum;
using CxViewerAction.Entities.Enumeration;
using System.Xml.Serialization;
using CxViewerAction.Helpers;

namespace CxViewerAction.Entities
{
    /// <summary>
    /// Login data
    /// </summary>
    public class LoginData : IEntity
    {
        #region [Public Constants]

        /// <summary>
        /// Default language code
        /// </summary>
        public const int DEFAULT_LANGUAGE_CODE = 1033;

        #endregion

        #region [Private Constants]

        /// <summary>
        /// Full service path url format
        /// </summary>
        private const string _baseFormat = "http{0}://{1}";
        private const string _servicePathFormat = "{0}/Cxwebinterface/CxWsResolver.asmx";

        #endregion

        #region [Private Members]

        private string _serverDomain = null;
        private string _server = null;
        private string _serverBaseUri = null;
        private bool _ssl = false;        

        private EntityId _id;
        private string _userName;
        private string _password;
        private bool _isLogging;
        string unboundRunID;

        private int _updateStatusInterval = 10;
        private int _maxZipFileSize = 400;

        private SimpleDecision _isRunScanInBackground;
        private SimpleDecision _isOpenPerspective;

        private string[] _excludeFileExt = "bak,tmp,aac,aif,iff,m3u,mid,mp3,mpa,ra,wav,wma,3g2,3gp,asf,asx,avi,flv,mov,mp4,mpg,rm,swf,vob,wmv,bmp,gif,jpg,png,psd,tif,swf,jar,zip,rar,exe,dll,pdb,7z,gz,tar.gz,tar,gz,ahtm,ahtml,fhtml,hdm,hdml,hsql,ht,hta,htc,htd,htmls,ihtml,mht,mhtm,mhtml,ssi,stm,stml,ttml,txn,xhtm,xhtml".Split(',');
        private string[] _excludeFolder = "bin,obj,.svn,_svn,backup".Split(',');

        private int _reconnectInterval = 15;
        private int _reconnectCount = 5;

        private bool _disableConnectionOptimizations = false;

        SerializableDictionary<string, string> _perspectives;
        
        private List<BindProject> bindedProjects;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Empty constructor
        /// </summary>
        public LoginData() { }

        /// <summary>
        /// Constructor with param
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <param name="server">Server name</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        public LoginData(EntityId id, string server, string userName, string password)
        {
            this.ID = id;
            this.Server = server;
            this.UserName = userName;
            this.Password = password;
            this.isSaml = false;
        }

        #endregion [ Constructors ]

        #region [ Properties ]

        /// <summary>
        /// Get or set server domain
        /// </summary>
        [XmlIgnore()]
        public string ServerDomain
        {
            get { return _serverDomain; }
            set { _serverDomain = value; }
        }

        public string ServerDomainEncrypted
        {
            get
            {
                if (!String.IsNullOrEmpty(ServerDomain))
                {
                    return EncryptHelper.EncryptString(ServerDomain);
                }
                else
                {
                    return null;
                }
            }
            set { _serverDomain = EncryptHelper.DecryptString(value); ; }
        }

        /// <summary>
        /// Get or set server full url
        /// </summary>
        [XmlIgnore()]
        public string Server
        {
            get
            {
                if (_serverDomain != null)
                {
                    _serverBaseUri = _serverDomain;
                    string httpServerDomain = _serverDomain.Replace("http://", string.Empty);
                    if (httpServerDomain.Length != _serverDomain.Length)

                    {
                        return string.Format(_servicePathFormat, _serverBaseUri);
                    }

                    string httpsServerDomain = _serverDomain.Replace("https://", string.Empty);
                    if (httpsServerDomain.Length != _serverDomain.Length)
                    {
                        return string.Format(_servicePathFormat, _serverBaseUri);
                    }
                }
                _serverBaseUri = string.Format(_baseFormat, (_ssl ? "s" : ""), _serverDomain);
                return string.Format(_servicePathFormat, _serverBaseUri);
            }
            set { _server = value; }
        }              

        public string ServerBaseUri
        {
            get { return _serverBaseUri; }
        }

        public string ServerEncrypted
        {
            get
            {
                if (!String.IsNullOrEmpty(Server))
                {
                    return EncryptHelper.EncryptString(Server);
                }
                else
                {
                    return null;
                }
            }
            set { _server = EncryptHelper.DecryptString(value); }
        }

        /// <summary>
        /// Gets or sets value indicating that connection must be established throw htts protocol
        /// </summary>
        public bool Ssl
        {
            get { return _ssl; }
            set { _ssl = value; }
        }

        /// <summary>
        /// Get or set Entity prorepty
        /// </summary>
        public EntityId ID { get { return _id; } set { _id = value; } }

        /// <summary>
        /// Get or set User name
        /// </summary>
        [XmlIgnore()]
        public string UserName { get { return _userName; } set { _userName = value; } }

        public string UserNameEncrypted
        {
            get
            {
                if (!String.IsNullOrEmpty(UserName))
                {
                    return EncryptHelper.EncryptString(UserName);
                }
                else
                {
                    return null;
                }
            }
            set { _userName = EncryptHelper.DecryptString(value); }
        }

        /// <summary>
        /// Get or set password
        /// </summary>
        [XmlIgnore()]
        public string Password { get { return _password; } set { _password = value; } }

        public string PasswordEncrypted
        {
            get
            {
                if (!String.IsNullOrEmpty(Password))
                {
                    return EncryptHelper.EncryptString(Password);
                }
                else
                {
                    return null;
                }
            }
            set { _password = EncryptHelper.DecryptString(value); }
        }

        /// <summary>
        /// Gets or sets interval to call server to get current process state
        /// </summary>
        public int UpdateStatusInterval { get { return _updateStatusInterval; } set { _updateStatusInterval = value; } }

        /// <summary>
        /// Get or set dialog state. If true - dialog validated sucessfull and user hit button go next step
        /// </summary>
        public bool IsLogging { get { return _isLogging; } set { _isLogging = value; } }

        /// <summary>
        /// Get or set status to show scan dialog window
        /// </summary>
        public SimpleDecision IsRunScanInBackground { get { return _isRunScanInBackground; } set { _isRunScanInBackground = value; } }

        /// <summary>
        /// Get or set status to show open perspective dialog after scan completed
        /// </summary>
        public SimpleDecision IsOpenPerspective { get { return _isOpenPerspective; } set { _isOpenPerspective = value; } }

        /// <summary>
        /// Stored projects perspective URL
        /// </summary>
        public SerializableDictionary<string, string> Perspectives { get { return _perspectives; } set { _perspectives = value; } }

        /// <summary>
        /// Gets or sets extensions list to exclude when project compressed before scan
        /// </summary>
        public string[] ExcludeFileExt { get { return _excludeFileExt; } set { _excludeFileExt = value; } }

        /// <summary>
        /// Gets or sets folder list to exclude when project compressed before scan
        /// </summary>
        public string[] ExcludeFolder { get { return _excludeFolder; } set { _excludeFolder = value; } }

        /// <summary>
        /// Gets or sets reconnection interval in seconds
        /// </summary>
        public int ReconnectInterval
        {
            get { return _reconnectInterval; }
            set { _reconnectInterval = value; }
        }

        /// <summary>
        /// Gets or sets counts to reconnect
        /// </summary>
        public int ReconnectCount
        {
            get { return _reconnectCount; }
            set { _reconnectCount = value; }
        }

        public string UnboundRunID
        {
            get { return unboundRunID; }
            set { unboundRunID = value; }
        }     
        
        public List<BindProject> BindedProjects
        {
            get { return bindedProjects; }
            set { bindedProjects = value; }
        }

        /// <summary>
        /// Gets or sets max allowed zip file size in megabites for scan
        /// </summary>
        public int MaxZipFileSize
        {
            get { return _maxZipFileSize; }
            set { _maxZipFileSize = value; }
        }

        public bool SSO
        {
            get;
            set;
        }

        public bool isSaml
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the option to disable connection optimizations
        /// This optimization should be disabled when there is a proxy or firewall between the plugin and the server
        /// </summary>
        public bool DisableConnectionOptimizations
        {
            get { return _disableConnectionOptimizations; }
            set { _disableConnectionOptimizations = value; }
        }

        #endregion [ Properties ]

        #region [ Public Methods ]

        /// <summary>
        /// Verify if user enter all data to start auth verification
        /// </summary>
        /// <returns></returns>
        public bool CanLog()
        {
            return ((String.IsNullOrEmpty(this.Server)) || (String.IsNullOrEmpty(this.UserName)) || (String.IsNullOrEmpty(this.Password))) ? false : true;
        }

        /// <summary>
        /// Adding project perspective dictionary
        /// </summary>
        /// <param name="project"></param>
        /// <param name="perspective"></param>
        /// <returns></returns>
        public void AddProjectPerspective(string project, string perspective)
        {
            if (Perspectives == null)
                Perspectives = new SerializableDictionary<string, string>();

            if (Perspectives.ContainsKey(project))
            {
                string oldPerspective = Perspectives[project];
                Perspectives[project] = perspective;
                StorageHelper.Delete(oldPerspective);
            }
            else
                Perspectives.Add(project, perspective);
        }

       

        /// <summary>
        /// Clear user auth data
        /// </summary>
        internal void Clear()
        {
            _userName = string.Empty;
            _password = string.Empty;
            _serverDomain = string.Empty;
        }

        #endregion [ Publica Methods ]

        public class BindProject
        {
            public void AddScanReport(ScanReportInfo scanReportInfo)
            {
                if (scanReports == null)
                    scanReports = new List<ScanReportInfo>();

                scanReports.Add(scanReportInfo);
            }

            public void ClearScanReports()
            {
                if (scanReports == null)
                    scanReports = new List<ScanReportInfo>();

                scanReports.Clear();
            }

            List<ScanReportInfo> scanReports;

            public string RootPath
            {
                get;
                set;
            }

            public string ProjectName
            {
                get;
                set;
            }


            public long BindedProjectId
            {
                get;
                set;
            }

            public long SelectedScanId
            {
                get;
                set;
            }

            public bool IsBound
            {
                get;
                set;
            }

            public bool IsPublic
            {
                get;
                set;
            }

            public List<ScanReportInfo> ScanReports { get { return scanReports; } set { scanReports = value; } }
        }

    }
}
