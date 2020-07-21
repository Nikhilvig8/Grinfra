using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;

namespace GrInfra.Models
{
    public class clsAuthentication
    {
        private DirectoryEntry _Entry;
        private DirectorySearcher _Search;
        private SearchResult _Result;
        private String _DomainName;
        private String _DomainIP;
        private String _Path;
        private String _FilterAttribute;
        private String _DomainUser;
        private Object _Obj;
        private String _LoggedOnWindowUser;
        private String _WindowUser;

        public string MyWindowUser
        {
            get { return _WindowUser; }
            set { _WindowUser = value; }
        }
        public DirectoryEntry MyEntry
        {
            get { return _Entry; }
            set { _Entry = value; }
        }
        public DirectorySearcher MySearch
        {
            get { return _Search; }
            set { _Search = value; }
        }

        public SearchResult MyResult
        {
            get { return _Result; }
            set { _Result = value; }
        }

        public string MyPath
        {
            get { return _Path; }
            set { _Path = value; }
        }
        public string MyFilterAttribute
        {
            get { return _FilterAttribute; }
            set { _FilterAttribute = value; }
        }


        public string MyDomainUser
        {
            get { return _DomainUser; }
            set { _DomainUser = value; }
        }

        public string MyDomainName
        {
            get { return _DomainName; }
            set { _DomainName = value; }
        }


        public string MyLoggedOnWindowUser
        {
            get { return _LoggedOnWindowUser; }
            set { _LoggedOnWindowUser = value; }
        }


        public string MyDomainIp
        {
            get { return _DomainIP; }
            set { _DomainIP = value; }
        }


        public object MyObj
        {
            get { return _Obj; }
            set { _Obj = value; }
        }




        public bool IsValidAdsUser(string _DomainIP, string UserName, string password)
        {
            _DomainUser = _DomainName + UserName;//.grinfra.local,administrator@grinfra.local,grinfra/adminstaror  

            _Entry = new DirectoryEntry("LDAP://" + _DomainIP, _DomainUser, password);
            try
            {
                _Obj = _Entry.NativeObject;
                _Search = new DirectorySearcher(_Entry);
                _Search.Filter = "(SAMAccountName=" + UserName + ")";
                _Search.PropertiesToLoad.Add("cn");
                _Result = _Search.FindOne();
                if (_Result == null)
                {
                    return false;
                }
                else
                {
                    _Path = _Result.Path;
                    //_FilterAttribute  FilterAttribute = CType(_Result.Properties("cn")(0), String)
                    _FilterAttribute = _Result.ToString();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
    }

}