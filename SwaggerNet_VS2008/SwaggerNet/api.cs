using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Reflection;

/// <summary>
/// Summary description for api
/// </summary>
namespace SwaggerNet
{
    public enum paramType
    {
        query,
        path,
        post
    }

    public enum httpMethod
    {
        POST,
        GET
    }

    public class apis
    {
        public apis()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region properties


        private List<api> _api = new List<api>();

        public List<api> Api
        {
            get { return _api; }
            set { _api = value; }
        }

        private operations _operations = new operations();

        public operations Operations
        {
            get { return _operations; }
            set { _operations = value; }
        }



        private string _basePath = string.Empty;

        public string BasePath
        {
            get { return _basePath; }
            set { _basePath = value; }
        }
        private string _SwaggerVersion = string.Empty;

        public string SwaggerVersion
        {
            get { return _SwaggerVersion; }
            set { _SwaggerVersion = value; }
        }
        private string _apiVersion = string.Empty;

        public string ApiVersion
        {
            get { return _apiVersion; }
            set { _apiVersion = value; }
        }

        private string _resourcePath = string.Empty;

        public string ResourcePath
        {
            get { return _resourcePath; }
            set { _resourcePath = value; }
        }

        #endregion

        #region methods
        #region method: generateResourceListing
        public string generateResourceListing()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("{\n");
            sb.Append("\"apis\":[\n");

            if (_api.Count > 0)
            {
                int lastone = _api.Count;
                int i = 1;
                foreach (api a in _api)
                {
                    sb.Append("{\n");
                    sb.Append("\"path\":\"" + a.Path + "\",\n");
                    sb.Append("\"description\":\"" + a.Description + "\"\n");
                    sb.Append("}\n");
                    if (i < _api.Count)
                    {
                        sb.Append(",");
                    }
                    i++;
                }
            }

            sb.Append("],\n");
            sb.Append("\"basePath\":\"" + _basePath + "\",\n");
            sb.Append("\"swaggerVersion\":\"" + _SwaggerVersion + "\",\n");
            sb.Append("\"apiVersion\":\"" + _apiVersion + "\"\n");
            sb.Append("}");


            return sb.ToString();
        }
        #endregion

        #region method: generateOperationListing
        public string generateOperationListing()
        {
            StringBuilder sb = new StringBuilder();
            if (_api.Count > 0)
            {
                sb.Append("{\"apis\":[\n");
                foreach (api ame in _api)
                {
                    sb.Append("{\"path\":\"" + ame.Path + "\",\n");
                    sb.Append("\"description\":\""+ame.Description+"\",\n");
                    sb.Append("\"operations\":\n");
                    
                    sb.Append("[{\"parameters\":[\n");

                    sb.Append("{\"name\":\"" + ame.Operations.Parameters.Name + "\",\n");
                    sb.Append("\"description\":\"" + ame.Operations.Parameters.Description + "\",\n");
                    sb.Append("\"dataType\":\"" + ame.Operations.Parameters.DataType + "\",\n");
                    sb.Append("\"required\":" + ame.Operations.Parameters.Required.ToString().ToLower() + ",\n");

                    if (ame.Operations.Parameters.ParamType == paramType.query)
                    {
                        sb.Append("\"allowMultiple\":" + ame.Operations.Parameters.AllowMultiple.ToString().ToLower() + ",\n");
                    }
                    if (ame.Operations.Parameters.ParamType != paramType.post)
                    {
                        if (ame.Operations.Parameters.AllowableValues != null)
                        {
                            if (ame.Operations.Parameters.AllowableValues.GetUpperBound(0) > 0)
                            {
                                int count = ame.Operations.Parameters.AllowableValues.GetUpperBound(0);
                                sb.Append("\"allowableValues\":[");
                                int x = 0;
                                foreach (string s in ame.Operations.Parameters.AllowableValues)
                                {
                                    sb.Append("\"" + s + "\"");
                                    if (x < count)
                                    {
                                        sb.Append(", ");
                                    }
                                    x++;
                                }
                                sb.Append("],");
                            }
                            else
                            {
                                //sb.Append("\"allowableValues\":[],");
                            }
                        }
                        else
                        {
                            //sb.Append("\"allowableValues\":[],");
                        }
                    }
                    sb.Append("\"paramType\":\"" + ame.Operations.Parameters.ParamType.ToString() + "\"}],\n");

                    sb.Append("\"httpMethod\":\"" + ame.Operations.HttpMethod.ToString() + "\",\n");
                    sb.Append("\"notes\":\"" + ame.Operations.Notes + "\",\n");

                    if (ame.Operations.ErrorResponses.Count > 0)
                    {
                        sb.Append(generateErrorResponses(ame.Operations.ErrorResponses));
                        //    "errorResponses":[{"reason":"Invalid ID supplied","code":400},{"reason":"Pet not found","code":404}],
                    }

                    sb.Append("\"nickname\":\"" + ame.Operations.Nickname + "\",\n");
                    sb.Append("\"responseClass\":\"" + ame.Operations.ResponseClass + "\",\n");
                    sb.Append("\"summary\":\"" + ame.Operations.Summary + "\"\n");
                    sb.Append("}]}\n");
                }

                sb.Append("],\n\n");
                List<models> _internalModels = new List<models>();
                foreach (api a in _api)
                {
                    if (a.Models.Count > 0)
                    {
                        foreach (models m in a.Models)
                        {
                            _internalModels.Add(m);
                        }
                    }
                }

                if (_internalModels.Count > 0)
                {
                    sb.Append("\"models\":{\n");
                    sb.Append(generateModels(_internalModels));
                    sb.Append("},\n");
                }
                sb.Append("\"resourcePath\":\"" + _resourcePath + "\",\n");
                sb.Append("\"basePath\":\"" + _basePath + "\",\n");
                sb.Append("\"swaggerVersion\":\""+_SwaggerVersion+"\",\n");
                sb.Append("\"apiVersion\":\"" + _apiVersion + "\"\n");
                sb.Append("}\n");
            }

            return sb.ToString();
        }
        #endregion

        #region method: generateErrorResponses
        private string generateErrorResponses(List<errorResponses> r)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("\"errorResponses\":[\n");
            int x = 1;
            foreach (errorResponses e in r)
            {
                sb.Append("{\n");
                sb.Append("\"reason\":\"" + e.Reason + "\",");
                sb.Append("\"code\":" + e.Code + "");
                sb.Append("}");
                if (x < r.Count)
                {
                    sb.Append(",\n");
                }
                x++;

            }

            sb.Append("],");

            return sb.ToString();
        }
        #endregion

        #region method:generateModels
        private string generateModels(List<models> models)
        {
            StringBuilder sb = new StringBuilder();

            
            int i = 1;
            foreach (models m in models)
            {
                sb.Append("\"" + m.Id + "\":{\n");
                sb.Append("\"properties\":{\n");
                int x = 1;
                foreach (modelProperties p in m.Properties)
                {
                    sb.Append("\"" + p.Name + "\":{\"type\":\"" + p.Type + "\"");

                    if (p.Type == "array")
                    {
                        sb.Append(",\"items\":{\"type\":\"" + p.ArrayItemsType + "\"}");
                    }

                    if (p.Description != string.Empty)
                    {
                        sb.Append(", \"description\":\"" + p.Description + "\"");
                    }
                    if (p.oEnumValues != null)
                    {
                        if (p.oEnumValues.Length > 0)
                        {
                            sb.Append(",\"enum\":[ ");
                            int xEnum = 0;
                            foreach (string senum in p.oEnumValues)
                            {
                                if (xEnum > 0)
                                {
                                    sb.Append(",");
                                }
                                sb.Append(" \"" + senum + "\"");
                                xEnum++;
                            }
                            sb.Append(" ]");
                        }
                    }
                    sb.Append("}");
                    if (x < m.Properties.Count)
                    {
                        sb.Append(",\n");
                    }
                    x++;
                }
                sb.Append("},\n");

                sb.Append("\"id\":\"" + m.Id + "\"}\n");
                if (i < models.Count)
                {
                    sb.Append(",");
                }
                i++;
            }


            return sb.ToString();
        }
        #endregion

        #endregion


    }

    #region api
    public class api
    {
        public api()
        {
        }

        public api(string path, string description)
        {
            _path = path;
            _Description = description;
        }


        #region properties
        private string _path = string.Empty;

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        private string _Description = string.Empty;

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        //private List<operations> _operations = new List<operations>();

        //public List<operations> Operations
        //{
        //    get { return _operations; }
        //    set { _operations = value; }
        //}

        private operations _Operations = new operations();

        public operations Operations
        {
            get { return _Operations; }
            set { _Operations = value; }
        }


        private List<models> _models = new List<models>();

        public List<models> Models
        {
            get { return _models; }
            set { _models = value; }
        }
        #endregion
    }
    #endregion


    #region operations
    public class operations
    {
        public operations()
        {
        }

        #region properties
        private parameters _parameters = new parameters();

        public parameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }


        private httpMethod _httpMethod = httpMethod.GET;

        public httpMethod HttpMethod
        {
            get { return _httpMethod; }
            set { _httpMethod = value; }
        }
        private string _notes = string.Empty;

        public string Notes
        {
            get { return _notes; }
            set { _notes = value; }
        }
        private string _nickname = string.Empty;

        public string Nickname
        {
            get { return _nickname; }
            set { _nickname = value; }
        }
        private string _responseClass = string.Empty;

        public string ResponseClass
        {
            get { return _responseClass; }
            set { _responseClass = value; }
        }
        private bool _deprecated = false;

        public bool Deprecated
        {
            get { return _deprecated; }
            set { _deprecated = value; }
        }
        private string _summary = string.Empty;

        public string Summary
        {
            get { return _summary; }
            set { _summary = value; }
        }

        private string _defaultValue = string.Empty;

        private List<errorResponses> _errorResponses = new List<errorResponses>();

        public List<errorResponses> ErrorResponses
        {
            get { return _errorResponses; }
            set { _errorResponses = value; }
        }

        #endregion

    }
    #endregion

    #region parameters
    public class parameters
    {
        public parameters()
        {
        }

        #region properties
        private string[] _allowableValues;

        public string[] AllowableValues
        {
            get { return _allowableValues; }
            set { _allowableValues = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        private string _dataType = string.Empty;

        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }
        private bool _required = true;

        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }
        private bool _allowMultiple = false;

        public bool AllowMultiple
        {
            get { return _allowMultiple; }
            set { _allowMultiple = value; }
        }

        private paramType _paramType = new paramType();

        public paramType ParamType
        {
            get { return _paramType; }
            set { _paramType = value; }
        }

        #endregion

    }
    #endregion

    #region errorResponses
    public class errorResponses
    {
        public errorResponses()
        {
        }

        #region properties
        private string _reason = string.Empty;

        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
        private int _code = 0;

        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }
        #endregion
    }
    #endregion

    #region models
    public class models
    {
        public models()
        {
        }

        public models(object outputObject)
        {
            setObject(outputObject);
        }

        #region properties
        private string _id = string.Empty;

        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private List<modelProperties> _properties = new List<modelProperties>();

        public List<modelProperties> Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }


        #endregion

        #region methods
        #region method: private getObjectOutputType
        private string getObjectOutputType(string n)
        {
            string val = null;

            switch (n)
            {
                case "string":
                    val = "string";
                    break;
                case "int32":
                    val = "integer";
                    break;
                case "int64":
                    val = "long";
                    break;
                case "double":
                    val = "double";
                    break;
                case "boolean":
                    val = "boolean";
                    break;
                case "datetime":
                    val = "string";
                    break;
            }

            return val;
        }
        #endregion

        #region method: private
        private void setObject(object o)
        {
            Type objectMe = o.GetType();
            _id = objectMe.Name;

            _properties = readObject(o);
        }

        private List<modelProperties> readObject(object o)
        {
            List<modelProperties> _props = new List<modelProperties>();
            if (o != null)
            {
                Type objectMe = o.GetType();
                //sb.Append(objectMe.ToString());
                //sb.Append(" " + objectMe.IsClass.ToString());
                //PROPERTIES
                if (objectMe.IsClass)
                {
                    //CHECK IF OBJECT IS PRIMITIVE
                    bool mainObjectIsPrimitive = false;
                    if (getObjectOutputType(objectMe.Name) != null)
                    {
                        mainObjectIsPrimitive = true;
                    }

                    if (!mainObjectIsPrimitive)
                    {
                        PropertyInfo[] p = objectMe.GetProperties();

                        foreach (PropertyInfo ps in p)
                        {
                            modelProperties _p = new modelProperties();
                            
                            Type j = ps.PropertyType.UnderlyingSystemType;
                            _p.Name = ps.Name;
                            bool isPrimitive = false;
                            if (getObjectOutputType(j.Name.ToLower()) != null)
                            {
                                isPrimitive = true;
                            }

                            if (isPrimitive)
                            {
                                _p.Type = getObjectOutputType(j.Name.ToLower());
                                _props.Add(_p);
                            }
                            else if (ps.PropertyType.IsClass && ps.PropertyType.IsPublic)
                            {
                                _p.Type = ps.PropertyType.Name;
                                _props.Add(_p);
                            }
                            else if (ps.PropertyType.IsArray && ps.PropertyType.IsPublic)
                            {
                                //ARRAY
                                _p.Type = "array";
                                _p.ArrayItemsType = getObjectOutputType(j.Name.Replace("[]", "").ToLower());
                                _props.Add(_p);
                            }
                            else if (ps.PropertyType.IsEnum && ps.PropertyType.IsPublic)
                            {
                                //IS ENUM
                                _p.Name = j.Name;
                                _p.Type = "string";
                                StringBuilder sbEnumVals = new StringBuilder();
                                int count = 0;
                                foreach (FieldInfo fields in ps.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static))
                                {
                                    if (count > 0)
                                    {
                                        sbEnumVals.Append(",");
                                    }
                                    sbEnumVals.Append(fields.Name);
                                    count++;
                                }

                                string[] _enumsVals = sbEnumVals.ToString().Split(',');
                                _p.oEnumValues = _enumsVals;
                                _props.Add(_p);
                            }
                            else if (ps.PropertyType.IsGenericType && ps.PropertyType.IsPublic)
                            {
                                //LIST
                                _p.Type = "array";

                                Type[] typeArguments = ps.PropertyType.GetGenericArguments();

                                foreach (Type tArg in typeArguments)
                                {
                                    if (getObjectOutputType(tArg.Name.ToString().ToLower()) != null)
                                    {
                                        _p.ArrayItemsType = getObjectOutputType(tArg.Name.ToString().ToLower());
                                        _props.Add(_p);
                                    }
                                    else
                                    {
                                        _p.ArrayItemsType = tArg.Name;
                                        _props.Add(_p);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //sb.Append("");
                }
            }

            return _props;

        }

        #endregion

        #endregion

    }
    #endregion

    #region modelProperties
    public class modelProperties
    {
        public modelProperties()
        {
        }

        #region properties
        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _type = string.Empty;

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private string[] _oEnum;

        public string[] oEnumValues
        {
            get { return _oEnum; }
            set { _oEnum = value; }
        }


        private string _arrayItemsType = string.Empty;

        public string ArrayItemsType
        {
            get { return _arrayItemsType; }
            set { _arrayItemsType = value; }
        }

        private List<modelProperties> _oProperties = new List<modelProperties>();

        public List<modelProperties> oProperties
        {
            get { return _oProperties; }
            set { _oProperties = value; }
        }



        #endregion
    }
    #endregion
}


