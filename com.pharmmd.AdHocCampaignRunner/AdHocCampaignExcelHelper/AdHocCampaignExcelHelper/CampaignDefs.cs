using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHocCampaignExcelHelper
{
    public class CampaignDefs
    {

        private static Dictionary<string, CampaignInfo> _campaigns = new Dictionary<string, CampaignInfo>();

        public static Dictionary<string, CampaignInfo> Campaigns
        {
            get
            {
                if (_campaigns.Count == 0)
                {
                    _campaigns.Add(CampaignDefs.SCAN_PES.Name, CampaignDefs.SCAN_PES);
                    _campaigns.Add(CampaignDefs.SCAN_PHARMACIST.Name, CampaignDefs.SCAN_PHARMACIST);
                    _campaigns.Add(CampaignDefs.SCAN_PES_SPANISH.Name, CampaignDefs.SCAN_PES_SPANISH);
                    _campaigns.Add(CampaignDefs.SCAN_PHARMACIST_SPANISH.Name, CampaignDefs.SCAN_PHARMACIST_SPANISH);
                    _campaigns.Add(CampaignDefs.SUMMACARE_PES.Name, CampaignDefs.SUMMACARE_PES);
                    _campaigns.Add(CampaignDefs.SUMMACARE_PHARMACIST.Name, CampaignDefs.SUMMACARE_PHARMACIST);
                    _campaigns.Add(CampaignDefs.MERIDIAN_PES.Name, CampaignDefs.MERIDIAN_PES);
                    _campaigns.Add(CampaignDefs.MERIDIAN_PHARMACIST.Name, CampaignDefs.MERIDIAN_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_PDP_PES.Name, CampaignDefs.HEALTHSPRING_PDP_PES);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_PDP_PHARMACIST.Name, CampaignDefs.HEALTHSPRING_PDP_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_SURGE.Name, CampaignDefs.HEALTHSPRING_SURGE);
                    _campaigns.Add(CampaignDefs.HEALTHSPRING_SURGE_NONENG.Name, CampaignDefs.HEALTHSPRING_SURGE_NONENG);
                    _campaigns.Add(CampaignDefs.HIGHMARKPDP_PES.Name, CampaignDefs.HIGHMARKPDP_PES);
                    _campaigns.Add(CampaignDefs.HIGHMARKPDP_PHARMACIST.Name, CampaignDefs.HIGHMARKPDP_PHARMACIST);
                    _campaigns.Add(CampaignDefs.HIGHMARKMAPD_PES.Name, CampaignDefs.HIGHMARKMAPD_PES);
                    _campaigns.Add(CampaignDefs.HIGHMARKMAPD_PHARMACIST.Name, CampaignDefs.HIGHMARKMAPD_PHARMACIST);
                    _campaigns.Add(CampaignDefs.CHP_PES.Name, CampaignDefs.CHP_PES);
                    _campaigns.Add(CampaignDefs.CHP_PES_NONENG.Name, CampaignDefs.CHP_PES_NONENG);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Cantonese.Name, CampaignDefs.CHP_PHARMACIST_Cantonese);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Korean.Name, CampaignDefs.CHP_PHARMACIST_Korean);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Mandarin.Name, CampaignDefs.CHP_PHARMACIST_Mandarin);
                    _campaigns.Add(CampaignDefs.CHP_PHARMACIST_Vietnamese.Name, CampaignDefs.CHP_PHARMACIST_Vietnamese);
                }
                return _campaigns;
            }
        }





        public static CampaignInfo SCAN_PES = new CampaignInfo
        {
            Name = "SCAN_PES",
            DialerTable = "MstrCamp_MTMPes_ScanAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_ScanAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SCAN_PES_{0}.xlsx"
        };
        public static CampaignInfo SCAN_PHARMACIST = new CampaignInfo
        {
            Name = "SCAN_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_ScanAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_ScanAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SCAN_PHARMACIST_{0}.xlsx"
        };
        public static CampaignInfo SCAN_PES_SPANISH = new CampaignInfo
        {
            Name = "SCAN_PES_SPANISH",
            DialerTable = "MstrCamp_MTMPes_ScanAdhoc_Spanish",
            DialerTableHx = "MstrCamp_MTMPes_ScanAdhoc_Spanish_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SCAN_PES_SPANISH_{0}.xlsx"
        };
        public static CampaignInfo SCAN_PHARMACIST_SPANISH = new CampaignInfo
        {
            Name = "SCAN_PHARMACIST_SPANISH",
            DialerTable = "MstrCamp_MTMPharmacist_ScanAdhoc_Spanish",
            DialerTableHx = "MstrCamp_MTMPharmacist_ScanAdhoc_Spanish_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SCAN_PHARMACIST_SPANISH_{0}.xlsx"
        };
        public static CampaignInfo SUMMACARE_PES = new CampaignInfo
        {
            Name = "SUMMACARE_PES",
            DialerTable = "MstrCamp_MTMPes_SummaCareAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_SummaCareAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SUMMACARE_PES_{0}.xlsx"
        };
        public static CampaignInfo SUMMACARE_PHARMACIST = new CampaignInfo
        {
            Name = "SUMMACARE_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_SummaCareAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_SummaCareAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\SUMMACARE_PHARMACIST_{0}.xlsx"
        };
        public static CampaignInfo MERIDIAN_PES = new CampaignInfo
        {
            Name = "MERIDIAN_PES",
            DialerTable = "MstrCamp_MTMPes_MeridianAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_MeridianAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\MERIDIAN_PES_{0}.xlsx"
        };
        public static CampaignInfo MERIDIAN_PHARMACIST = new CampaignInfo
        {
            Name = "MERIDIAN_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_MeridianAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_MeridianAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\MERIDIAN_PHARMACIST_{0}.xlsx"
        };
        //public static CampaignInfo HEALTHSPRING_PDP_PES = new CampaignInfo
        //{
        //    Name = "HEALTHSPRING_PDP_PES",
        //    DialerTable = "MstrCamp_MTMPes_HealthSpringPDPAdhoc",
        //    DialerTableHx = "MstrCamp_MTMPes_HealthSpringPDPAdhoc_Hx",
        //    ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_PDP_PES_{0}.xlsx"
        //};
        //public static CampaignInfo HEALTHSPRING_PDP_PHARMACIST = new CampaignInfo
        //{
        //    Name = "HEALTHSPRING_PDP_PHARMACIST",
        //    DialerTable = "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc",
        //    DialerTableHx = "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc_Hx",
        //    ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_PDP_PHARMACIST_{0}.xlsx"
        //};
        public static CampaignInfo HEALTHSPRING_SURGE = new CampaignInfo
        {
            Name = "HEALTHSPRING_SURGE",
            DialerTable = "MstrCamp_MTMPes_HealthSpringSurgeAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_HealthSpringSurgeAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_SURGE_{0}.xlsx"
        };
        public static CampaignInfo HEALTHSPRING_SURGE_NONENG = new CampaignInfo
        {
            Name = "HEALTHSPRING_SURGE_NONENG",
            DialerTable = "MstrCamp_MTMPes_HealthSpringSurgeAdhoc_NonEng",
            DialerTableHx = "MstrCamp_MTMPes_HealthSpringSurgeAdhoc_NonEng_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_SURGE_NONENG_{0}.xlsx"
        };
        public static CampaignInfo HEALTHSPRING_PDP_PES = new CampaignInfo
        {
            Name = "HEALTHSPRING_PDP_PES",
            DialerTable = "MstrCamp_MTMPes_HealthSpringPDPAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_HealthSpringPDPAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_PDP_PES_{0}.xlsx"
        };

        public static CampaignInfo HEALTHSPRING_PDP_PHARMACIST = new CampaignInfo
        {
            Name = "HEALTHSPRING_PDP_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_HealthSpringPDPAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HEALTHSPRING_PDP_PHARMACIST_{0}.xlsx"
        };

        public static CampaignInfo HIGHMARKPDP_PES = new CampaignInfo
        {
            Name = "HIGHMARKPDP_PES",
            DialerTable = "MstrCamp_MTMPes_HighmarkPDPAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_HighmarkPDPAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HIGHMARKPDP_PES_{0}.xlsx"
        };
        public static CampaignInfo HIGHMARKPDP_PHARMACIST = new CampaignInfo
        {
            Name = "HIGHMARKPDP_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_HighmarkPDPAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_HighmarkPDPAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HIGHMARKPDP_PHARMACIST_{0}.xlsx"
        };
        public static CampaignInfo HIGHMARKMAPD_PES = new CampaignInfo
        {
            Name = "HIGHMARKMAPD_PES",
            DialerTable = "MstrCamp_MTMPes_HighmarkMAPDAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_HighmarkMAPDAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HIGHMARKMAPD_PES_{0}.xlsx"
        };
        public static CampaignInfo HIGHMARKMAPD_PHARMACIST = new CampaignInfo
        {
            Name = "HIGHMARKMAPD_PHARMACIST",
            DialerTable = "MstrCamp_MTMPharmacist_HighmarkMAPDAdhoc",
            DialerTableHx = "MstrCamp_MTMPharmacist_HighmarkMAPDAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\HIGHMARKMAPD_PHARMACIST_{0}.xlsx"
        };
        public static CampaignInfo CHP_PES = new CampaignInfo
        {
            Name = "CHP_PES",
            DialerTable = "MstrCamp_MTMPes_CHPAdhoc",
            DialerTableHx = "MstrCamp_MTMPes_CHPAdhoc_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PES_{0}.xlsx"
        };
        public static CampaignInfo CHP_PES_NONENG = new CampaignInfo
        {
            Name = "CHP_PES_NONENG",
            DialerTable = "MstrCamp_MTMPes_CHPAdhoc_NONENG",
            DialerTableHx = "MstrCamp_MTMPes_CHPAdhoc_NONENG_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PES_NONENG_{0}.xlsx"
        };
        public static CampaignInfo CHP_PHARMACIST_Cantonese = new CampaignInfo
        {
            Name = "CHP_PHARMACIST_Cantonese",
            DialerTable = "MstrCamp_MTMPharmacist_CHPAdhoc_Cantonese",
            DialerTableHx = "MstrCamp_MTMPharmacist_CHPAdhoc_Cantonese_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PHARMACIST_Cantonese_{0}.xlsx"
        };
        public static CampaignInfo CHP_PHARMACIST_Korean = new CampaignInfo
        {
            Name = "CHP_PHARMACIST_Korean",
            DialerTable = "MstrCamp_MTMPharmacist_CHPAdhoc_Korean",
            DialerTableHx = "MstrCamp_MTMPharmacist_CHPAdhoc_Korean_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PHARMACIST_Korean_{0}.xlsx"
        };
        public static CampaignInfo CHP_PHARMACIST_Mandarin = new CampaignInfo
        {
            Name = "CHP_PHARMACIST_Mandarin",
            DialerTable = "MstrCamp_MTMPharmacist_CHPAdhoc_Mandarin",
            DialerTableHx = "MstrCamp_MTMPharmacist_CHPAdhoc_Mandarin_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PHARMACIST_Mandarin_{0}.xlsx"
        };
        public static CampaignInfo CHP_PHARMACIST_Vietnamese = new CampaignInfo
        {
            Name = "CHP_PHARMACIST_Vietnamese",
            DialerTable = "MstrCamp_MTMPharmacist_CHPAdhoc_Vietnamese",
            DialerTableHx = "MstrCamp_MTMPharmacist_CHPAdhoc_Vietnamese_Hx",
            ExcelPathTemplate = @"\\pmdnas\Departments\IT - Data Team\Public\AdHocCallLists\{0}\CHP_PHARMACIST_Vietnamese_{0}.xlsx"
        };

        public struct CampaignInfo
        {
            public string Name;
            public string DialerTable;
            public string DialerTableHx;
            public string ExcelPathTemplate;

        }


    }


}
