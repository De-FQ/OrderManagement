
using API.Areas.Backend;
using API.Areas.Frontend.Controllers;
using API.Helpers;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.LoggerService; 
using Utility.Models.Frontend.Common;
using Utility.Models.Frontend.HomePage;
using Utility.ResponseMapper;

namespace API.Areas.Frontend
{
    public class CommonController : BaseController
    {
         


        private readonly ICommonHelper _commonHelper;
        private readonly AppSettingsModel _appSettingsModels;
        
        //Memory cache
        public CommonController(IMemoryCache cache, 
            AppSettingsModel options,
            ICommonHelper commonHelper) : base(options)
        {
            //Memory cache
            base._memoryCache = cache;

            base.ControllerName = typeof(CommonController).Name;
            _commonHelper = commonHelper;
            _appSettingsModels = options;
        }

       

       

        /// <summary>
        /// GET working
        /// </summary>
        /// <param name="appContentTypeId"></param>
        /// <returns></returns>
        

        /// <summary>
        /// GET working
        /// </summary>
        /// <param name="appContentTypeId"></param>
        /// <returns></returns>
        

        /// <summary>
        /// GET working
        /// </summary>
        /// <param name="appContentTypeId"></param>
        /// <returns></returns>
        
       
        


       


        private void SaveInfomationRequestForm(ref InfomationRequestFormDto item)
        {
            if (item.ApplicationDocumentFile != null && item.ApplicationDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.ApplicationDocumentFile, "ApplicationDocument/", _appSettingsModels.ImageSettings.InfomationRequestForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.ApplicationDocument = fileName;
            }
            if (item.SignatoryDocumentFile != null && item.SignatoryDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.SignatoryDocumentFile, "SignatoryDocument/", _appSettingsModels.ImageSettings.InfomationRequestForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.SignatoryDocument = fileName;
            }
            if (item.CivilIdDocumentFile != null && item.CivilIdDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.CivilIdDocumentFile, "CivilIdDocument/", _appSettingsModels.ImageSettings.InfomationRequestForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.CivilIdDocument = fileName;
            }

        }

        private void SaveGrievanceForm(ref GrievanceFormDto item)
        {
            if (item.GrievanceDocumentFile != null && item.GrievanceDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.GrievanceDocumentFile, "GrievanceDocument/", _appSettingsModels.ImageSettings.GrievanceForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.GrievanceDocument = fileName;
            }
            if (item.SignatoryDocumentFile != null && item.SignatoryDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.SignatoryDocumentFile, "SignatoryDocument/", _appSettingsModels.ImageSettings.GrievanceForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.SignatoryDocument = fileName;
            }
            if (item.CivilIdDocumentFile != null && item.CivilIdDocumentFile.Length > 0)
            {
                string fileName = string.Empty;

                fileName = MediaHelper.SaveDocToFile(item.CivilIdDocumentFile, "CivilIdDocument/", _appSettingsModels.ImageSettings.GrievanceForms);

                if (!string.IsNullOrEmpty(fileName))
                    item.CivilIdDocument = fileName;
            }

        }


    }
}
